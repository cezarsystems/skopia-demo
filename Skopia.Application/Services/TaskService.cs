using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skopia.Application.Contracts;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly SkopiaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITaskHistoryService _taskHistoryService;
        private readonly ITaskCommentService _taskCommentService;

        public TaskService(SkopiaDbContext dbContext, IMapper mapper, ITaskHistoryService taskHistoryService, ITaskCommentService taskCommentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _taskHistoryService = taskHistoryService;
            _taskCommentService = taskCommentService;
        }

        public async Task<TaskResponseDTO> CreateAsync(TaskRequestDTO request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            var newTask = _mapper.Map<TaskModel>(request);
            newTask.LastModified = DateTime.Now;

            _dbContext.Tasks.Add(newTask);
            await _dbContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                await _taskCommentService.AddAsync(newTask.Id, request.UserId, request.Comment);
            }

            var fullTask = await _dbContext.Tasks
                .Include(t => t.User)
                .Include(t => t.Project)
                    .ThenInclude(p => p.User)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == newTask.Id);

            return _mapper.Map<TaskResponseDTO>(fullTask);
        }

        public async Task<OperationResultModel> DeleteAsync(long id)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return OperationResultModel.Fail("Tarefa não encontrada. Favor confirmar seu identificador.");

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
                .Include(t => t.Comments)
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
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            return task == null ? null : _mapper.Map<TaskResponseDTO>(task);
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

            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
                return OperationResultModel<TaskResponseDTO>.Fail("Usuário não encontrado.");

            var histories = new List<TaskHistoryModel>();

            if (task.Status != request.Status.ToString())
            {
                histories.Add(new TaskHistoryModel
                {
                    FieldChanged = "Status",
                    OldValue = task.Status,
                    NewValue = request.Status.ToString(),
                    TaskId = task.Id,
                    UserId = user.Id,
                    ModifiedAt = DateTime.Now
                });

                task.Status = request.Status.ToString();
            }

            if (task.ExpirationDate != request.ExpirationDate)
            {
                histories.Add(new TaskHistoryModel
                {
                    FieldChanged = "ExpirationDate",
                    OldValue = task.ExpirationDate?.ToString("yyyy-MM-dd"),
                    NewValue = request.ExpirationDate?.ToString("yyyy-MM-dd"),
                    TaskId = task.Id,
                    UserId = user.Id,
                    ModifiedAt = DateTime.Now
                });

                task.ExpirationDate = request.ExpirationDate;
            }

            task.LastModified = DateTime.Now;
            await _dbContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                await _taskCommentService.AddAsync(task.Id, user.Id, request.Comment);

                histories.Add(new TaskHistoryModel
                {
                    FieldChanged = "Comment",
                    OldValue = "",
                    NewValue = request.Comment,
                    TaskId = task.Id,
                    UserId = user.Id,
                    ModifiedAt = DateTime.Now
                });
            }

            if (histories.Any())
                await _taskHistoryService.AddRangeAsync(histories);

            var response = _mapper.Map<TaskResponseDTO>(task);
            return OperationResultModel<TaskResponseDTO>.Ok(response);
        }
    }
}