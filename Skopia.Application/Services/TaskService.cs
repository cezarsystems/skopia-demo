using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skopia.Domain.Contracts;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class TaskService : IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long>
    {
        private readonly SkopiaDbContext _dbContext;

        private readonly IMapper _mapper;

        public TaskService(SkopiaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TaskResponseDTO> CreateAsync(TaskRequestDTO request)
        {
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

        public Task<OperationResultModel> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TaskResponseDTO>> GetAllAsync()
        {
            var taskList = await _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.Project)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskResponseDTO>>(taskList);
        }

        public async Task<TaskResponseDTO> GetByIdAsync(long id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return null;

            return _mapper.Map<TaskResponseDTO>(task);
        }

        public Task<OperationResultModel<TaskResponseDTO>> UpdateAsync(long id, TaskUpdateRequestDTO request)
        {
            throw new NotImplementedException();
        }
    }
}