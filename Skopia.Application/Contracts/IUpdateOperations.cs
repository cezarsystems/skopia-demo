using Skopia.Domain.Models;

namespace Skopia.Application.Contracts
{
    public interface IUpdateOperations<TUpdateRequestDTO, TResponseDTO>
    {
        Task<OperationResultModel<TResponseDTO>> UpdateAsync(TUpdateRequestDTO request);
    }
}