using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Ecommerce_project.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // Default to 500 if unexpected
            var message = exception.Message;
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case NotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case BadRequestException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case ValidationException _:
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    break;
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    _logger.LogError(exception, "Unhandled exception occurred.");
                    message = "An unexpected error occurred";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}

