using FluentValidation;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class ProjectModelValidator : AbstractValidator<ProjectRequestDTO>
    {
        public ProjectModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("O nome do projeto deve ser preenchido.")
                .MaximumLength(100)
                    .WithMessage("O nome do projeto deve ser menor ou igual a 100 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                    .WithMessage("A descrição do projeto deve ser menor ou igual a 250 caracteres.");
        }
    }
}