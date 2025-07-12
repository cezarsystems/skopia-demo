namespace Skopia.Application.Contracts
{
    public interface IProjectService
    {
        Task<bool> Exists(long id);
    }
}