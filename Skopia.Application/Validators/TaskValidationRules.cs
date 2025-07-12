using FluentValidation;

namespace Skopia.Application.Validators
{
    public static class TaskValidationRules
    {
        public static IRuleBuilderOptions<T, char> ValidStatus<T>(this IRuleBuilder<T, char> rule)
        {
            return rule
                .Must(status => new[] { 'P', 'A', 'C' }.Contains(char.ToUpperInvariant(status)))
                .WithMessage("Os status válidos são P (Pendente), A (Em andamento) ou C (Concluído).");
        }

        public static IRuleBuilderOptions<T, char> ValidPriority<T>(this IRuleBuilder<T, char> rule)
        {
            return rule
                .Must(priority => new[] { 'B', 'M', 'A' }.Contains(char.ToUpperInvariant(priority)))
                .WithMessage("Os níveis de prioridade válidos são B (Baixa), M (Média) ou A (Alta).");
        }

        public static IRuleBuilderOptions<T, string> ValidComment<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .MaximumLength(1000)
                .WithMessage("O comentário deve ter no máximo 1000 caracteres.");
        }

        public static void ValidExpirationDate<T>(this IRuleBuilderInitial<T, string> rule)
        {
            rule.Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                if (!DateTime.TryParse(value, out var parsed))
                {
                    context.AddFailure("A data de expiração da tarefa é inválida. Informe uma data no formato AAAA-MM-DD.");
                    return;
                }

                if (parsed.Date < DateTime.Today)
                {
                    context.AddFailure("A data de expiração da tarefa, se informada, não pode ser anterior à data atual.");
                }
            });
        }
    }
}