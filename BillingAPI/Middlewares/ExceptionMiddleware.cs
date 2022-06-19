using BillingAPI.Errors;
using System.Net;
using System.Text.Json;

namespace BillingAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        private async Task Respond(HttpContext context, int statusCode, string msg,
            Exception ex, string? stackTrace = null)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            ApiException response = new ApiException(context.Response.StatusCode, msg, stackTrace);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await Respond(context, (int)HttpStatusCode.NotFound, ex.Message, ex);
            }
            catch (BadRequestException ex)
            {
                await Respond(context, (int)HttpStatusCode.BadRequest, ex.Message, ex);
            }
            catch (Exception ex)
            {
                string? stackTrace = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null;
                await Respond(context, (int)HttpStatusCode.InternalServerError, ex.Message, ex, stackTrace);
            }
        }
    }
}
