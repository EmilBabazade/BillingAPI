namespace BillingAPI.Middlewares
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public LogRequestMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public Task InvokeAsync(HttpContext context)
        {
            PathString path = context.Request.Path;
            _logger.LogInformation($"\n****{path}****");
            return Task.CompletedTask;
        }
    }
}
