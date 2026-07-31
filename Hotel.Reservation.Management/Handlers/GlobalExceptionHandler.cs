using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Hotel.Reservation.Management.API.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken = default)
        {
            var (statusCode, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
                ConflictException => (StatusCodes.Status409Conflict, "The request conflicts with the current state of the resource."),
                BusinessRuleException => (StatusCodes.Status400BadRequest, "The request violates a business rule."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            _logger.LogError(exception, "An exception occured. Path: {Path}, Method:{Method}, Message:{Message}", httpContext.Request.Path, httpContext.Request.Method, exception.Message);

            var errorResponse = new ErrorResponse
            {
                Status = statusCode,
                Errors = statusCode == StatusCodes.Status500InternalServerError
                     ? ["An unexpected error occurred."]
                     : [exception.Message]
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
