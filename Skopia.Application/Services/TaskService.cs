using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skopia.Application.Contracts;
using Skopia.Application.Helpers;
using Skopia.Domain.Contracts;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class TaskService : IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long>, ITaskService
    {
        private readonly SkopiaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITaskHistoryService _taskHistoryService;

        public TaskService(SkopiaDbContext dbContext, IMapper mapper, ITaskHistoryService taskHistoryService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _taskHistoryService = taskHistoryService;
        }

        public async Task<TaskResponseDTO> CreateAsync(TaskRequestDTO request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);

            request.Comment = CommentHelper.AddComment(request.Comment, user.Name);

            var newTask = _mapper.Map<TaskModel>(request);
            newTask.LastModified = DateTime.Now;

            _dbContext.Tasks.Add(newTask);
            await _dbContext.SaveChangesAsync();

            var fullTask = await _dbContext.Tasks
                .Include(t => t.User)
                .Include(t => t.Project)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(t => t.Id == newTask.Id);

            return _mapper.Map<TaskResponseDTO>(fullTask);
        }

        public async Task<OperationResultModel> DeleteAsync(long id)
        {
            var task = await _dbContext.Tasks
                .FirstOrDefaultAsync(p => p.Id == id);

            if (task == null)
            {
                return OperationResultModel.Fail("Tarefa não encontrado. Favor confirmar seu identificador.");
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return OperationResultModel.Ok();
        }

        public async Task<IEnumerable<TaskResponseDTO>> GetAllAsync()
        {
            var tasks = await _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.Project)
                    .ThenInclude(p => p.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskResponseDTO>>(tasks);
        }

        public async Task<TaskResponseDTO> GetByIdAsync(long id)
        {
            var task = await _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.Project)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return null;

            return _mapper.Map<TaskResponseDTO>(task);
        }

        public async Task<bool> TaskLimitExceeded(long id)
        {
            return await _dbContext.Tasks.CountAsync(t => t.ProjectId == id) >= 20;
        }

        public async Task<OperationResultModel<TaskResponseDTO>> UpdateAsync(TaskUpdateRequestDTO request)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);
            if (task == null)
                return OperationResultModel<TaskResponseDTO>.Fail("Tarefa não encontrada.");

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
                return OperationResultModel<TaskResponseDTO>.Fail("Usuário não encontrado.");

            var histories = new List<TaskHistoryEntryModel>();

            if (task.Status != request.Status.ToString())
            {
                histories.Add(new TaskHistoryEntryModel
                {
                    FieldChanged = "Status",
                    OldValue = task.Status,
                    NewValue = request.Status.ToString(),
                    TaskId = task.Id,
                    UserId = user.Id
                });
                task.Status = request.Status.ToString();
            }

            if (task.ExpirationData != request.ExpirationDate)
            {
                histories.Add(new TaskHistoryEntryModel
                {
                    FieldChanged = "ExpirationData",
                    OldValue = task.ExpirationData?.ToString("yyyy-MM-dd"),
                    NewValue = request.ExpirationDate?.ToString("yyyy-MM-dd"),
                    TaskId = task.Id,
                    UserId = user.Id
                });
                task.ExpirationData = request.ExpirationDate;
            }

            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                task.Comments = CommentHelper.AppendComment("", request.Comment, user.Name);

                histories.Add(new TaskHistoryEntryModel
                {
                    FieldChanged = "Comment",
                    OldValue = "",
                    NewValue = request.Comment,
                    TaskId = task.Id,
                    UserId = user.Id
                });
            }

            task.LastModified = DateTime.Now;

            await _dbContext.SaveChangesAsync();


            if (histories.Any())
                await _taskHistoryService.AddRangeAsync(histories);

            var response = _mapper.Map<TaskResponseDTO>(task);
            return OperationResultModel<TaskResponseDTO>.Ok(response);
        }
    }
}