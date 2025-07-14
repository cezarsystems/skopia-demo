using Microsoft.EntityFrameworkCore;
using Skopia.Application.Services;
using Skopia.Domain.Models;
using Skopia.Infrastructure.Data;

namespace Skopia.Tests.Services
{
    public class TaskCommentServiceTests
    {
        private readonly DbContextOptions<SkopiaDbContext> _dbContextOptions;

        public TaskCommentServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SkopiaDbContext>()
                .UseInMemoryDatabase(databaseName: $"SkopiaDb_{Guid.NewGuid()}")
                .Options;
        }

        [Fact(DisplayName = "AddAsync deve adicionar um comentário com sucesso")]
        public async Task AddAsync_ShouldAddCommentSuccessfully()
        {
            // Arrange
            using var context = new SkopiaDbContext(_dbContextOptions);

            var task = new TaskModel { Id = 1, Name = "Tarefa Teste", Description = "", Priority = "", Status = "" };
            var user = new UserModel { Id = 1, Name = "Usuário Teste", Role = "user" };

            context.Tasks.Add(task);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var service = new TaskCommentService(context);

            // Act
            await service.AddAsync(task.Id, user.Id, "Comentário de teste");

            // Assert
            var comentario = context.TaskComments.FirstOrDefault();
            Assert.NotNull(comentario);
            Assert.Equal(task.Id, comentario.TaskId);
            Assert.Equal(user.Id, comentario.UserId);
            Assert.Equal("Comentário de teste", comentario.Content);
            Assert.True(comentario.CreationDate <= DateTime.Now);
        }
    }
}