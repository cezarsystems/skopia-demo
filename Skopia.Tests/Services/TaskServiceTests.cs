using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Skopia.Application.Contracts;
using Skopia.Application.Services;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.Infrastructure.Data;

namespace Skopia.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly SkopiaDbContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<ITaskHistoryService> _historyMock;
        private readonly Mock<ITaskCommentService> _commentMock;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<SkopiaDbContext>()
                .UseInMemoryDatabase(databaseName: "SkopiaTestDb_" + Guid.NewGuid())
                .Options;

            _context = new SkopiaDbContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<Skopia.Application.Mappers.MappingProfile>());
            _mapper = config.CreateMapper();

            _historyMock = new Mock<ITaskHistoryService>();
            _commentMock = new Mock<ITaskCommentService>();

            _service = new TaskService(_context, _mapper, _historyMock.Object, _commentMock.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var user = new UserModel
            {
                Id = 1,
                Name = "Tester",
                Role = "usr"
            };

            var project = new ProjectModel
            {
                Id = 1,
                Name = "Projeto Teste",
                Description = "Descrição do Projeto Teste",
                CreatedAt = DateTime.Now,
                User = user
            };

            _context.Users.Add(user);
            _context.Projects.Add(project);

            _context.Tasks.Add(new TaskModel
            {
                Id = 1,
                Name = "Tarefa Inicial",
                Description = "Descrição da Tarefa Inicial",
                ProjectId = 1,
                UserId = 1,
                Status = "P",
                Priority = "A",
                CreatedAt = DateTime.Now
            });

            _context.SaveChanges();
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar os dados da tarefa quando existir")]
        public async Task GetByIdAsync_ReturnsTask_WhenExists()
        {
            // Act & Assert
            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Tarefa Inicial", result.Name);
        }

        [Fact(DisplayName = "Exists deve retornar true quando a tarefa existir")]
        public async Task Exists_ReturnsTrue_WhenTaskExists()
        {
            // Act & Assert
            var result = await _service.Exists(1);

            Assert.True(result);
        }

        [Fact(DisplayName = "LimitExceeded deve retornar true quando um projeto conter 20 ou mais tarefas")]
        public async Task LimitExceeded_ReturnsFalse_WhenUnderLimit()
        {
            // Act & Assert
            var result = await _service.LimitExceeded(1);

            Assert.False(result);
        }

        [Fact(DisplayName = "PostAsync deve retornar os dados da nova tarefa ao criar com sucesso")]
        public async Task PostAsync_CreatesTaskSuccessfully()
        {
            // Arrange
            var request = new TaskRequestDTO
            {
                Name = "Nova Tarefa",
                Description = "Descrição",
                ProjectId = 1,
                UserId = 1,
                Priority = "M",
                Status = "P"
            };

            // Act
            var result = await _service.PostAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Nova Tarefa", result.Name);
        }

        [Fact(DisplayName = "DeleteAsync deve retornar sucesso quando uma tarefa for removida da base de dados")]
        public async Task DeleteAsync_RemovesTask_WhenExists()
        {
            // Act
            var response = await _service.DeleteAsync(1);
            var exists = await _context.Tasks.AnyAsync(t => t.Id == 1);

            // Assert
            Assert.True(response.Success);
            Assert.False(exists);
        }

        [Fact(DisplayName = "GetAllAsync obtém todos as tarefas registradas na base de dados")]
        public async Task GetAllAsync_ReturnsAllTasks()
        {
            // Act & Assert
            var result = await _service.GetAllAsync();

            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "UpdateAsync deve retornar os dados atualizados se houver sucesso na operação")]
        public async Task UpdateAsync_ChangesStatusAndAddsComment()
        {
            // Arrange
            var request = new TaskUpdateRequestDTO
            {
                TaskId = 1,
                UserId = 1,
                Status = "C",
                ExpirationDate = "2025-07-14",
                Comment = "Comentário de atualização"
            };

            // Act
            var result = await _service.UpdateAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ToolsServiceExtension.GetEnumDescription<StatusEnum>(request.Status), result.Data.Status);
            _commentMock.Verify(m => m.AddAsync(1, 1, "Comentário de atualização"), Times.Once);
        }
    }
}