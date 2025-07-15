using FluentValidation.TestHelper;
using Moq;
using Skopia.Application.Contracts;
using Skopia.Application.Validators;
using Skopia.DTOs.Models.Request;

namespace Skopia.Tests.Validators
{
    public class ProjectModelValidatorTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly ProjectModelValidator _validator;

        public ProjectModelValidatorTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _userServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(true);

            _validator = new ProjectModelValidator(_userServiceMock.Object);
        }

        [Fact(DisplayName = "Deve validar projeto válido com sucesso")]
        public async Task Should_Validate_Valid_Project()
        {
            // Arrange
            var model = new ProjectRequestDTO
            {
                UserId = 1,
                Name = "Projeto Teste",
                Description = "Descrição válida"
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "Deve falhar se o ID do usuário for 0")]
        public async Task Should_Have_Error_When_UserId_Is_Zero()
        {
            // Arrange
            var model = new ProjectRequestDTO { UserId = 0 };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact(DisplayName = "Deve falhar se o usuário não existir")]
        public async Task Should_Have_Error_When_User_Not_Exists()
        {
            _userServiceMock.Setup(s => s.Exists(It.IsAny<long>())).ReturnsAsync(false);

            // Arrange
            var model = new ProjectRequestDTO { UserId = 99 };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact(DisplayName = "Deve falhar se o nome do projeto estiver vazio")]
        public async Task Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var model = new ProjectRequestDTO { Name = "" };
            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact(DisplayName = "Deve falhar se o nome exceder 100 caracteres")]
        public async Task Should_Have_Error_When_Name_Too_Long()
        {
            // Arrange
            var model = new ProjectRequestDTO
            {
                Name = new string('A', 101)
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact(DisplayName = "Deve falhar se a descrição exceder 250 caracteres")]
        public async Task Should_Have_Error_When_Description_Too_Long()
        {
            // Arrange
            var model = new ProjectRequestDTO
            {
                Description = new string('B', 251)
            };

            // Arrange
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}