using Microsoft.EntityFrameworkCore;
using Skopia.Application.Services;
using Skopia.Domain.Models;
using Skopia.Infrastructure.Data;
using Skopia.Tests.Helpers;

namespace Skopia.Tests.Services
{
    public class ReportsServiceTests
    {
        private readonly DbContextOptions<SkopiaDbContext> _dbContextOptions;

        public ReportsServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SkopiaDbContext>()
                .UseInMemoryDatabase(databaseName: "SkopiaTestDb_" + Guid.NewGuid())
                .Options;
        }

        [Fact(DisplayName = "GetUserPerformanceAsync traz os dados obtidos pelo relatório com o número de tarefas completadas")]
        public async Task GetUserPerformanceAsync_ReturnsCorrectData()
        {
            // Arrange
            using var context = new SkopiaDbContext(_dbContextOptions);
            context.Users.Add(new UserModel { Id = 1, Name = "User A", Role = "manager" });
            context.Tasks.Add(new TaskModel
            {
                Id = 1,
                UserId = 1,
                Status = StatusEnum.C.ToString(),
                CompletedAt = DateTime.UtcNow,
                Description = "Tarefa de teste",
                Name = "Tarefa 1",
                Priority = "B"
            });

            await ContextHelper.SaveChangesSafeAsync(context);

            var service = new ReportsService(context);

            // Act
            var result = await service.GetUserPerformanceAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().TotalCompletedTasks);
        }

        [Fact(DisplayName = "GetProjectStatsAsync traz os dados obtidos pelo relatório com a média de conclusão das tarefas por período")]
        public async Task GetProjectStatsAsync_ReturnsCorrectAverages()
        {
            // Arrange
            using var context = new SkopiaDbContext(_dbContextOptions);
            context.Projects.Add(new ProjectModel
            {
                Id = 1,
                Name = "Projeto A",
                Description = "Descrição do Projeto A",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                Tasks = new List<TaskModel> {
                    new TaskModel {
                        Name = "Tarefa 1", Description = "", Priority = "", Status = ""
                    },
                    new TaskModel {
                        Name = "Tarefa 2", Description = "", Priority = "", Status = ""
                    }
                }
            });

            await ContextHelper.SaveChangesSafeAsync(context);

            // Act
            var service = new ReportsService(context);
            var result = await service.GetProjectStatsAsync();

            // Assert
            Assert.Equal(1, result.TotalProjectsLast30Days);
            Assert.Equal(2, result.TotalTasksLast30Days);
            Assert.Equal(2.0, result.AverageTasksPerProject);
        }

        [Fact(DisplayName = "GetAverageTaskCompletionTime traz os dados obtidos pelo relatório com o tempo médio de forma agrupada")]
        public async Task GetAverageTaskCompletionTime_ReturnsGroupedAverages()
        {
            // Arrange
            using var context = new SkopiaDbContext(_dbContextOptions);
            context.Projects.Add(new ProjectModel { Id = 1, Name = "Projeto Teste", Description = "Descrição do Projeto Teste" });
            await ContextHelper.SaveChangesSafeAsync(context);

            context.Tasks.Add(new TaskModel
            {
                Name = "Tarefa teste",
                ProjectId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CompletedAt = DateTime.UtcNow,
                Status = StatusEnum.C.ToString(),
                Project = context.Projects.First(),
                Description = "",
                Priority = "",
                Comments = []
            });

            await ContextHelper.SaveChangesSafeAsync(context);

            // Act
            var service = new ReportsService(context);
            var result = service.GetAverageTaskCompletionTime();

            // Assert
            Assert.Single(result);
            Assert.Equal("Projeto Teste", result.First().ProjectName);
        }

        [Fact(DisplayName = "GetProjectTasksReportAsync traz os dados obtidos pelo relatório baseado no range de período com base no status e prioridade das tarefas registradas")]
        public async Task GetProjectTasksReportAsync_ReturnsFilteredTasks()
        {
            // Arrange
            using var context = new SkopiaDbContext(_dbContextOptions);
            context.Projects.Add(new ProjectModel { Id = 1, Name = "Projeto Relatório", Description = "" });
            await ContextHelper.SaveChangesSafeAsync(context);

            context.Tasks.Add(new TaskModel
            {
                Name = "Tarefa Exemplo",
                Status = StatusEnum.P.ToString(),
                Priority = PriorityEnum.M.ToString(),
                CreatedAt = DateTime.UtcNow,
                ProjectId = 1,
                Project = context.Projects.First(),
                Description = ""
            });

            await ContextHelper.SaveChangesSafeAsync(context);

            // Act
            var service = new ReportsService(context);
            var result = await service.GetProjectTasksReportAsync(StatusEnum.P.ToString(), PriorityEnum.M.ToString());

            // Assert
            Assert.Single(result);
            Assert.Equal("Projeto Relatório", result.First().ProjectName);
            Assert.Single(result.First().Tasks);
            Assert.Equal("Tarefa Exemplo", result.First().Tasks.First().TaskName);
        }
    }
}