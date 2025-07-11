using Skopia.Domain.Contracts;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Services
{
    public class TaskService : IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long>
    {
        public Task<TaskResponseDTO> CreateAsync(TaskRequestDTO request)
        {
            var dadosRecebidos = request;

            return Task.Run(() => new TaskResponseDTO());
        }

        public Task<OperationResultModel> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskResponseDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
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