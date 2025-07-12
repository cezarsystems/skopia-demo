namespace Skopia.Application.Contracts
{
    public interface IUserService
    {
        Task<bool> Exists(long id);
    }
}