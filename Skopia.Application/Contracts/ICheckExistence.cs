namespace Skopia.Application.Contracts
{
    public interface ICheckExistence<TId>
    {
        Task<bool> Exists(TId id);
    }
}