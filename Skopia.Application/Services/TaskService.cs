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

            await _dbContext.Entry(newTask)
                .Reference(t => t.Project)
                .LoadAsync();

            return _mapper.Map<TaskResponseDTO>(newTask);
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

        public Task<TaskResponseDTO> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResultModel<TaskResponseDTO>> UpdateAsync(long id, TaskUpdateRequestDTO request)
        {
            throw new NotImplementedException();
        }
    }
}