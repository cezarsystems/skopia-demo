using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Skopia.Application.Contracts;

namespace Skopia.Api.Middleware.Filters
{
    public class OnlyManagerAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            long userId = 0;

            var hasUserIdInQuery = context.HttpContext.Request.Query.TryGetValue("userId", out var userIdStr);
            var isUserIdValid = hasUserIdInQuery && long.TryParse(userIdStr, out userId);

            if (!isUserIdValid || !await userService.IsManager(userId))
            {
                context.Result = new UnauthorizedObjectResult("Acesso restrito a gerentes de projetos da Skopia.");
                return;
            }

            await next();
        }
    }
}