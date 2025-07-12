using FluentValidation;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class TaskModelValidator : AbstractValidator<TaskRequestDTO>
    {
        private readonly IProjectService _projectService;

        public TaskModelValidator(IProjectService projectService)
        {
            _projectService = projectService;

            RuleFor(x => x.ProjectId)
                .NotEqual(0)
                    .WithMessage("O identificador do projeto é inválido.");

            RuleFor(x => x.ProjectId)
                .MustAsync(async (id, ct) => await _projectService.Exists(id))
                    .WithMessage("O projeto informado não existe.")
                .When(x => x.ProjectId != 0);

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("O nome da tarefa deve ser preenchido.")
                .MaximumLength(100)
                    .WithMessage("O nome da tarefa deve ser menor ou igual a 100 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                    .WithMessage("A descrição da tarefa deve ser menor ou igual a 500 caracteres.");

            RuleFor(x => x.Priority)
                .NotEmpty()
                    .WithMessage("O nível da prioridade da tarefa deve ser informado.")
                .Must(priority => new[] { 'B', 'M', 'A' }.Contains(priority))
                    .WithMessage("Os níveis de prioridade válidos são B, M e A.");

            RuleFor(x => x.Status)
                .NotEmpty()
                    .WithMessage("O status da tarefa deve ser informado.")
                .Must(status => new[] { 'P', 'A', 'C' }.Contains(status))
                    .WithMessage("Os status válidos são P, A e C.");

            RuleFor(x => x.ExpirationData)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.ExpirationData.HasValue)
                    .WithMessage("A data de expiração da tarefa, se informada, não pode ser anterior à data atual.");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.Comment))
                    .WithMessage("O comentário deve ter no máximo 1000 caracteres.");
        }
    }
}