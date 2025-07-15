using FluentValidation.TestHelper;
using Moq;
using Skopia.Application.Contracts;
using Skopia.Application.Validators;
using Skopia.DTOs.Models.Request;

namespace Skopia.Tests.Validators
{
    public class TaskUpdateModelValidatorTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly TaskUpdateModelValidator _validator;

        public TaskUpdateModelValidatorTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _userServiceMock = new Mock<IUserService>();

            _taskServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(true);
            _userServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(true);

            _validator = new TaskUpdateModelValidator(_taskServiceMock.Object, _userServiceMock.Object);
        }

        [Fact(DisplayName = "Validação deve ser bem-sucedida com dados válidos")]
        public async Task Should_Validate_Successfully_When_Valid()
        {
            // Arrange
            var model = new TaskUpdateRequestDTO
            {
                TaskId = 1,
                UserId = 2,
                Status = "P",
                ExpirationDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                Comment = "Comentário válido"
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "Deve retornar erro para TaskId inválido ou inexistente")]
        public async Task Should_Fail_When_TaskIdInvalidOrNotExists()
        {
            _taskServiceMock.Setup(s => s.Exists(0)).ReturnsAsync(false);

            // Arrange
            var model = new TaskUpdateRequestDTO { TaskId = 0, UserId = 1, Status = "A" };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.TaskId);
        }

        [Fact(DisplayName = "Deve retornar erro para UserId inválido ou inexistente")]
        public async Task Should_Fail_When_UserIdInvalidOrNotExists()
        {
            _userServiceMock.Setup(s => s.Exists(0)).ReturnsAsync(false);

            // Arrange
            var model = new TaskUpdateRequestDTO { TaskId = 1, UserId = 0, Status = "A" };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact(DisplayName = "Deve retornar erro para Status vazio ou inválido")]
        public async Task Should_Fail_When_StatusIsEmptyOrInvalid()
        {
            // Arrange
            var model = new TaskUpdateRequestDTO { TaskId = 1, UserId = 1, Status = "" };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact(DisplayName = "Deve retornar erro para Status fora dos valores permitidos")]
        public async Task Should_Fail_When_StatusIsNotAllowed()
        {
            // Arrange
            var model = new TaskUpdateRequestDTO { TaskId = 1, UserId = 1, Status = "Z" };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact(DisplayName = "Deve retornar erro para data de expiração inválida ou passada")]
        public async Task Should_Fail_When_ExpirationDateInvalidOrPast()
        {
            // Arrange
            var model = new TaskUpdateRequestDTO
            {
                TaskId = 1,
                UserId = 1,
                Status = "A",
                ExpirationDate = "2000-01-01"
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.ExpirationDate);
        }

        [Fact(DisplayName = "Deve retornar erro para comentário muito longo")]
        public async Task Should_Fail_When_CommentTooLong()
        {
            // Arrange
            var model = new TaskUpdateRequestDTO
            {
                TaskId = 1,
                UserId = 1,
                Status = "A",
                Comment = new string('A', 1001)
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Comment);
        }
    }
}