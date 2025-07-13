using FluentValidation;
using Skopia.Application.Contracts;
using Skopia.Application.Validators.Shared;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class TaskUpdateModelValidator : AbstractValidator<TaskUpdateRequestDTO>
    {
        public TaskUpdateModelValidator(ITaskService taskService, IUserService userService)
        {
            RuleFor(x => x.TaskId)
                .ValidId("O identificador da tarefa é inválido.")
                .MustAsync(async (id, _) => await taskService.Exists(id))
                    .WithMessage("A tarefa informada não existe.");

            RuleFor(x => x.UserId)
                .ValidId("O identificador do usuário é inválido.")
                .MustAsync(async (id, _) => await userService.Exists(id))
                    .WithMessage("O usuário informado não existe.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status da tarefa deve ser informado.");

            RuleFor(x => x.Status)
                .ValidEnum(new[] { "P", "A", "C" }, "status da tarefa");

            RuleFor(x => x.ExpirationDate)
                .ValidOptionalFutureDate("data de expiração");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                    .WithMessage("O comentário deve ter no máximo 1000 caracteres.");
        }
    }
}