using FluentValidation;
using Skopia.Application.Contracts;
using Skopia.Application.Validators.Shared;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class ProjectModelValidator : AbstractValidator<ProjectRequestDTO>
    {
        public ProjectModelValidator(IUserService userService)
        {
            RuleFor(x => x.UserId)
                .ValidId("O identificador do usuário é inválido.")
                .MustAsync(async (id, _) => await userService.Exists(id))
                    .WithMessage("O usuário informado não existe.");

            RuleFor(x => x.Name)
                .RequiredName(100, "nome do projeto");

            RuleFor(x => x.Description)
                .OptionalDescription(250, "descrição do projeto");
        }
    }
}