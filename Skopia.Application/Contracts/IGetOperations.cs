namespace Skopia.Application.Contracts
{
    public interface IGetOperations<TResponseDTO, TId>
    {
        Task<IEnumerable<TResponseDTO>> GetAllAsync();
        Task<TResponseDTO> GetByIdAsync(TId id);
    }
}