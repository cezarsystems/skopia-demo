using FluentValidation;

namespace Skopia.Application.Validators.Shared
{
    public static class ValidationRules
    {
        public static IRuleBuilderOptions<T, long> ValidId<T>(
            this IRuleBuilder<T, long> ruleBuilder,
            string message = "Identificador inválido.")
        {
            return ruleBuilder
                .NotEqual(0)
                .WithMessage(message);
        }

        public static IRuleBuilderOptions<T, string> RequiredName<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int max = 100,
            string name = "nome")
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage($"O {name} deve ser preenchido.")
                .MaximumLength(max)
                    .WithMessage($"O {name} deve ter no máximo {max} caracteres.");
        }

        public static IRuleBuilderOptions<T, string> OptionalDescription<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int max = 250,
            string name = "descrição")
        {
            return ruleBuilder
                .MaximumLength(max)
                .WithMessage($"A {name} deve ter no máximo {max} caracteres.");
        }

        public static IRuleBuilderOptions<T, string> ValidEnum<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string[] validValues,
            string fieldName)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage($"O campo {fieldName} deve ser informado.")
                .Must(v =>
                    !string.IsNullOrWhiteSpace(v) &&
                    validValues.Contains(v.Trim().ToUpperInvariant()))
                .WithMessage($"Valores válidos para {fieldName}: {string.Join(", ", validValues)}.");
        }
    }
}