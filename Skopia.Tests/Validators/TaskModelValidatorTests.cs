using FluentValidation.TestHelper;
using Moq;
using Skopia.Application.Contracts;
using Skopia.Application.Validators;
using Skopia.DTOs.Models.Request;

namespace Skopia.Tests.Validators
{
    public class TaskModelValidatorTests
    {
        private readonly Mock<IProjectService> _projectServiceMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ITaskService> _taskServiceMock = new();
        private readonly TaskModelValidator _validator;

        public TaskModelValidatorTests()
        {
            _projectServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(true);
            _userServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(true);
            _taskServiceMock.Setup(s => s.LimitExceeded(It.IsAny<long>())).ReturnsAsync(false);

            _validator = new TaskModelValidator(
                _projectServiceMock.Object,
                _userServiceMock.Object,
                _taskServiceMock.Object
            );
        }

        [Fact(DisplayName = "Deve falhar se o identificador do projeto for inválido")]
        public async Task Should_Have_Error_When_ProjectId_Is_Zero()
        {
            // Arrange
            var model = new TaskRequestDTO { ProjectId = 0 };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Fact(DisplayName = "Deve falhar se o projeto não existir na base de dados")]
        public async Task Should_Have_Error_When_Project_Does_Not_Exist()
        {
            _projectServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(false);
            // Arrange
            var model = new TaskRequestDTO { ProjectId = 1 };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Fact(DisplayName = "Deve falhar se o usuário não existir na base de dados")]
        public async Task Should_Have_Error_When_User_Does_Not_Exist()
        {
            _userServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(false);
            // Arrange
            var model = new TaskRequestDTO { ProjectId = 1, UserId = 1 };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact(DisplayName = "Deve falhar se o limite de 20 tarefas for atingido dentro de um determinado projeto")]
        public async Task Should_Have_Error_When_Task_Limit_Exceeded()
        {
            _taskServiceMock.Setup(s => s.LimitExceeded(It.IsAny<long>())).ReturnsAsync(true);
            // Arrange
            var model = new TaskRequestDTO { ProjectId = 1 };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.ProjectId);
        }

        [Fact(DisplayName = "Deve falhar se o nome da tarefa estiver vazia")]
        public async Task Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var model = new TaskRequestDTO { Name = "" };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact(DisplayName = "Deve falhar se a Data de Expiração for menor que a data corrente")]
        public async Task Should_Have_Error_When_ExpirationDate_Is_Past()
        {
            // Arrange
            var model = new TaskRequestDTO { ExpirationDate = DateTime.UtcNow.AddDays(-5).ToString("yyyy-MM-dd") };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.ExpirationDate);
        }

        [Fact(DisplayName = "Deve passar se o modelo de dados da nova task recebido na requisição for válido")]
        public async Task Should_Pass_When_Model_Is_Valid()
        {
            // Arrange
            var model = new TaskRequestDTO
            {
                ProjectId = 1,
                UserId = 1,
                Name = "Tarefa válida",
                Description = "Descrição opcional",
                Status = "P",
                Priority = "M",
                ExpirationDate = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd"),
                Comment = "Tudo certo"
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}