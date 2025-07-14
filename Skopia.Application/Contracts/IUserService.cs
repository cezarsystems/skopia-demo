namespace Skopia.Application.Contracts;

public interface IUserService : ICheckExistence<long>
{
    Task<bool> IsManager(long userId);
}