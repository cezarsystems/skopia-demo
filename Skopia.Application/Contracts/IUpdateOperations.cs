using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Contracts
{
    public interface IUpdateOperations<TUpdateRequestDTO, TResponseDTO>
    {
        Task<OperationResponseDTO<TResponseDTO>> UpdateAsync(TUpdateRequestDTO request);
    }
}