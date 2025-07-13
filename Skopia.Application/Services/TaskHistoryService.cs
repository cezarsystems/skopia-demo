using AutoMapper;
using Skopia.Application.Contracts;
using Skopia.Domain.Models;
using Skopia.Infrastructure.Data;

namespace Skopia.Application.Services
{
    public class TaskHistoryService : ITaskHistoryService
    {
        private readonly SkopiaDbContext _dbContext;

        private readonly IMapper _mapper;

        public TaskHistoryService(SkopiaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddRangeAsync(IEnumerable<TaskHistoryModel> entries)
        {
            var histories = _mapper.Map<IEnumerable<TaskHistoryModel>>(entries);

            await _dbContext.TaskHistories.AddRangeAsync(histories);
            await _dbContext.SaveChangesAsync();
        }
    }
}