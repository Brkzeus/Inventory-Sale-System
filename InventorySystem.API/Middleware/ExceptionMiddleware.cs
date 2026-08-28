using InventorySystem_Core.DTOs.ErrorDTOs;
using InventorySystem_Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace InventorySystem.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errResponse = new
                {
                    statusCode = 500,
                    message = ex.Message,
                    detail = ex.InnerException?.Message ?? ex.StackTrace
                };

                var json = JsonSerializer.Serialize(errResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Map custom exception types to appropriate HTTP status codes
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,           // 404
                BadRequestException => HttpStatusCode.BadRequest,       // 400
                UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
                _ => HttpStatusCode.InternalServerError                 // 500
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponseDTO
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Details = statusCode == HttpStatusCode.InternalServerError? "An unexpected error occurred on the server." : exception.StackTrace
            };

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
