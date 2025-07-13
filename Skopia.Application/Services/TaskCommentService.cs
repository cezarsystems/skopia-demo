using Skopia.Application.Contracts;

namespace Skopia.Application.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        public TaskCommentService(/* dependências */)
        {

        }

        public Task AddAsync(long taskId, long userId, string content)
        {
            throw new NotImplementedException();
        }
    }
}