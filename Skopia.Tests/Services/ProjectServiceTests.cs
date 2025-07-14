using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skopia.Application.Services;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.Infrastructure.Data;

namespace Skopia.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly IMapper _mapper;
        private readonly SkopiaDbContext _dbContext;
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<Application.Mappers.MappingProfile>());
            _mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<SkopiaDbContext>()
                .UseInMemoryDatabase(databaseName: "SkopiaTestDb_" + Guid.NewGuid())
                .Options;

            _dbContext = new SkopiaDbContext(options);

            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _service = new ProjectService(_dbContext, _mapper);
        }

        [Fact(DisplayName = "PostAsync cria um novo projeto e traz os dados com sucesso")]
        public async Task PostAsync_ShouldCreateNewProject()
        {
            // Arrange
            var request = new ProjectRequestDTO
            {
                UserId = 1,
                Name = "Projeto Teste",
                Description = "Descrição teste"
            };

            // Act
            var result = await _service.PostAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Projeto Teste", result.Name);
            Assert.Equal("Descrição teste", result.Description);

            var projectInDb = await _dbContext.Projects.FindAsync(result.Id);
            Assert.NotNull(projectInDb);
        }

        [Fact(DisplayName = "GetByIdAsync obtêm os dados do projeto pelo identificador e retorna sucesso")]
        public async Task GetByIdAsync_ShouldReturnProject_WhenExists()
        {
            // Arrange
            var project = new ProjectModel { Name = "Projeto X", Description = "Desc", UserId = 1 };
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.Name, result.Name);
        }

        [Fact(DisplayName = "GetAllAsync retorna os dados de todos os projetos com sucesso")]
        public async Task GetAllAsync_ShouldReturnAllProjects()
        {
            // Arrange
            _dbContext.Projects.AddRange(
                new ProjectModel { Name = "P1", Description = "D1", UserId = 1 },
                new ProjectModel { Name = "P2", Description = "D2", UserId = 1 }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "DeleteAsync remove um projeto com sucesso se não houver tarefas com status 'Pendente'")]
        public async Task DeleteAsync_ShouldRemoveProject_WhenNoPendingTasks()
        {
            // Arrange
            var project = new ProjectModel
            {
                Id = 99,
                Name = "Remover",
                Description = "Teste",
                UserId = 1
            };

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.DeleteAsync(project.Id);

            // Assert
            Assert.True(result.Success);
            var deleted = await _dbContext.Projects.FindAsync(project.Id);
            Assert.Null(deleted);
        }
    }
}