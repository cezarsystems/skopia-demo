using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Skopia.Application.Services;
using Skopia.Domain.Models;
using Skopia.Infrastructure.Data;

namespace Skopia.Tests.Services
{
    public class TaskHistoryServiceTests
    {
        private readonly DbContextOptions<SkopiaDbContext> _dbContextOptions;

        public TaskHistoryServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SkopiaDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskHistoryTestDb")
                .Options;
        }

        [Fact(DisplayName = "AddRangeAsync deve adicionar históricos ao contexto com sucesso")]
        public async Task AddRangeAsync_ShouldAddHistoriesToContext()
        {
            // Arrange
            using var context = new SkopiaDbContext(_dbContextOptions);
            var mockMapper = new Mock<IMapper>();

            var fakeHistories = new List<TaskHistoryModel>
            {
                new TaskHistoryModel
                {
                    TaskId = 1,
                    UserId = 2,
                    FieldChanged = "Status",
                    OldValue = "P",
                    NewValue = "C",
                    ModifiedAt = DateTime.UtcNow
                }
            };

            mockMapper.Setup(m => m.Map<IEnumerable<TaskHistoryModel>>(It.IsAny<IEnumerable<TaskHistoryModel>>()))
                      .Returns((IEnumerable<TaskHistoryModel> src) => src);

            var service = new TaskHistoryService(context, mockMapper.Object);

            // Act
            await service.AddRangeAsync(fakeHistories);

            // Assert
            var result = await context.TaskHistories.ToListAsync();
            Assert.Single(result);
            Assert.Equal("Status", result[0].FieldChanged);
        }
    }
}