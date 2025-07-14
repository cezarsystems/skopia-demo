namespace Skopia.Api.Middleware
{
    public class ErrorHandlingInterceptor
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingInterceptor> _logger;

        public ErrorHandlingInterceptor(RequestDelegate next, ILogger<ErrorHandlingInterceptor> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado na API");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new { message = "Ocorreu um erro interno no servidor. Entre em contato com a equipe de suporte Skopia" };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}