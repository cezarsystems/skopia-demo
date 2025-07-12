using Skopia.Domain.Models;

namespace Skopia.Domain.Contracts
{
    public interface IBasicApiOperations<TaskCreateDTO, TaskUpdateDTO, TResponseDTO, TId>
    {
        Task<IEnumerable<TResponseDTO>> GetAllAsync();
        Task<TResponseDTO> GetByIdAsync(TId id);
        Task<TResponseDTO> CreateAsync(TaskCreateDTO request);
        Task<OperationResultModel<TResponseDTO>> UpdateAsync(TaskUpdateDTO request);
        Task<OperationResultModel> DeleteAsync(TId id);
    }
}