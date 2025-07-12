using FluentValidation;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class ProjectModelValidator : AbstractValidator<ProjectRequestDTO>
    {
        private readonly IUserService _userService;

        public ProjectModelValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEqual(0)
                    .WithMessage("O identificador do usuário é inválido.")
                .MustAsync(async (id, ct) => await _userService.Exists(id))
                    .WithMessage("O usuário informado não existe.");

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