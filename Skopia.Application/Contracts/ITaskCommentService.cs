namespace Skopia.Application.Contracts
{
    public interface ITaskCommentService
    {
        Task AddAsync(long taskId, long userId, string content);
    }
}