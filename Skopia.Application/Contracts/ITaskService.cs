namespace Skopia.Application.Contracts
{
    public interface ITaskService
    {
        Task<bool> TaskLimitExceeded(long id);
    }
}