using FluentValidation;
using FluentValidation.TestHelper;
using Skopia.Application.Validators.Shared;

namespace Skopia.Tests.Validators
{
    public class SharedRulesValidatorTests
    {
        private class DataEntryRequestModel
        {
            public long Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
        }

        private class DataEntryRequestModelValidator : AbstractValidator<DataEntryRequestModel>
        {
            public DataEntryRequestModelValidator()
            {
                RuleFor(x => x.Id).ValidId();
                RuleFor(x => x.Name).RequiredName();
                RuleFor(x => x.Description).OptionalDescription();
                RuleFor(x => x.Status).ValidEnum(new[] { "A", "B", "C" }, "Status");
            }
        }

        private readonly DataEntryRequestModelValidator _validator = new();

        [Fact(DisplayName = "Deve falhar se o identificador for zero")]
        public void ValidId_Should_Fail_When_Id_Is_Zero()
        {
            // Arrange
            var model = new DataEntryRequestModel { Id = 0 };
            // Arrange
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact(DisplayName = "Deve falhar se o nome do projeto ou tarefa for vazio")]
        public void RequiredName_Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var model = new DataEntryRequestModel { Name = "" };
            // Arrange
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact(DisplayName = "Deve falhar se o identificador for zero")]
        public void RequiredName_Should_Fail_When_Name_Too_Long()
        {
            // Arrange
            var model = new DataEntryRequestModel { Name = new string('x', 101) };
            // Arrange
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact(DisplayName = "Deve falhar se a descrição tiver mais de 250 caracteres")]
        public void OptionalDescription_Should_Fail_When_Too_Long()
        {
            // Arrange
            var model = new DataEntryRequestModel { Description = new string('x', 251) };
            // Arrange
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact(DisplayName = "Deve falhar se o valor do status for inválido")]
        public void ValidEnum_Should_Fail_When_Value_Is_Invalid()
        {
            // Arrange
            var model = new DataEntryRequestModel { Status = "X" };
            // Arrange
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact(DisplayName = "Deve passar se o valor do status for válido")]
        public void ValidEnum_Should_Pass_When_Value_Is_Valid()
        {
            // Arrange
            var model = new DataEntryRequestModel { Status = "B" };
            // Arrange
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Status);
        }
    }
}