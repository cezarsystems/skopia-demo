using FluentValidation;
using Skopia.Application.Converters;

namespace Skopia.Application.Validators.Shared
{
    public static class DateValidationRules
    {
        public static IRuleBuilderOptionsConditions<T, string> ValidOptionalFutureDate<T>(
            this IRuleBuilder<T, string> ruleBuilder, string fieldName)
        {
            return ruleBuilder.Custom((data, context) =>
            {
                if (string.IsNullOrWhiteSpace(data))
                    return;

                var parsed = DateConverter.Parse(data);

                if (parsed == null)
                {
                    context.AddFailure($"A {fieldName} é inválida. Informe uma data válida no formato AAAA-MM-DD.");
                    return;
                }

                if (parsed.Value.Date < DateTime.Today)
                {
                    context.AddFailure($"A {fieldName} não pode ser anterior à data atual.");
                }
            });
        }
    }
}