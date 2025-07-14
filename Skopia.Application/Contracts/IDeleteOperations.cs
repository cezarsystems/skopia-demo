using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts
{
    public interface IDeleteOperations<TId>
    {
        Task<OperationResponseDTO> DeleteAsync(TId id);
    }
}