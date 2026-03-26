using Newtonsoft.Json;
using System.Net;

namespace Backend.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError(exception, "An unhandled exception occurred while processing the request.");
            var result = JsonConvert.SerializeObject(new
            {
                Success = false,
                Message = "An error occurred while processing your request.",
                Error = exception.Message //need to remove this in production for security reasons
            });

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }

}
