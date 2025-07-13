using FluentValidation;
using Skopia.Application.Contracts;
using Skopia.Application.Validators.Shared;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class TaskModelValidator : AbstractValidator<TaskRequestDTO>
    {
        public TaskModelValidator(IProjectService projectService, IUserService userService, ITaskService taskService)
        {
            RuleFor(x => x.ProjectId)
                .ValidId("O identificador do projeto é inválido.")
                .MustAsync(async (id, _) => await projectService.Exists(id))
                    .WithMessage("O projeto informado não existe.");

            RuleFor(x => x.ProjectId)
                .MustAsync(async (id, _) => !await taskService.LimitExceeded(id))
                    .WithMessage("Este projeto já atingiu o limite de 20 tarefas. Exclua uma tarefa existente para adicionar uma nova.");

            RuleFor(x => x.UserId)
                .ValidId("O identificador do usuário é inválido.")
                .MustAsync(async (id, _) => await userService.Exists(id))
                    .WithMessage("O usuário informado não existe.");

            RuleFor(x => x.Name)
                .RequiredName(100, "nome da tarefa");

            RuleFor(x => x.Description)
                .OptionalDescription(250, "descrição da tarefa");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status da tarefa deve ser informado.");

            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("A prioridade da tarefa deve ser informada.");

            RuleFor(x => x.ExpirationDate)
                .ValidOptionalFutureDate("data de expiração");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                    .WithMessage("O comentário deve ter no máximo 1000 caracteres.");

            RuleFor(x => x.Status)
                .ValidEnum(new[] { "P", "A", "C" }, "status da tarefa");

            RuleFor(x => x.Priority)
                .ValidEnum(new[] { "B", "M", "A" }, "prioridade da tarefa");
        }
    }
}