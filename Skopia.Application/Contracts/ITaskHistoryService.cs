using Skopia.Domain.Models;

namespace Skopia.Application.Contracts
{
    public interface ITaskHistoryService
    {
        Task AddRangeAsync(IEnumerable<TaskHistoryModel> entries);
    }
}