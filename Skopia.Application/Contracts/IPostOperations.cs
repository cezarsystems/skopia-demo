namespace Skopia.Application.Contracts
{
    public interface IPostOperations<TPostRequestDTO, TResponseDTO>
    {
        Task<TResponseDTO> PostAsync(TPostRequestDTO request);
    }
}