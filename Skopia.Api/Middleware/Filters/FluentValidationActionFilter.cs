using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Skopia.Api.Middleware.Filters
{
    public class FluentValidationActionFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationActionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = _serviceProvider.GetService(validatorType);

                if (validator is not null)
                {
                    var validateMethod = validatorType.GetMethod("ValidateAsync", new[] { argument.GetType(), typeof(CancellationToken) });
                    var validationTask = (Task)validateMethod.Invoke(validator, new object[] { argument, CancellationToken.None });
                    await validationTask.ConfigureAwait(false);

                    var resultProperty = validationTask.GetType().GetProperty("Result");
                    var result = resultProperty.GetValue(validationTask);
                    var isValid = (bool)result.GetType().GetProperty("IsValid").GetValue(result);

                    if (!isValid)
                    {
                        var errors = (FluentValidation.Results.ValidationResult)result;

                        var fiedErrors = errors.Errors
                            .Select(err => new
                            {
                                field = err.PropertyName,
                                message = err.ErrorMessage
                            });

                        context.Result = new BadRequestObjectResult(fiedErrors);
                        return;
                    }
                }
            }

            await next();
        }
    }
}