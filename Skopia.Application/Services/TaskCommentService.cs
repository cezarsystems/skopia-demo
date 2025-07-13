using Skopia.Application.Contracts;
using Skopia.Domain.Models;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly SkopiaDbContext _dbContext;

        public TaskCommentService(SkopiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(long taskId, long userId, string content)
        {
            var comment = new TaskCommentModel
            {
                TaskId = taskId,
                UserId = userId,
                Content = content,
                CreationDate = DateTime.Now
            };

            _dbContext.TaskComments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}