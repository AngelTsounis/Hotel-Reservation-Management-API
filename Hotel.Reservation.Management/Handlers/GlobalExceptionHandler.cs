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
            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                BusinessRuleException => StatusCodes.Status400BadRequest,
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception,
                    "Unhandled exception. Path: {Path}, Method: {Method}",
                    httpContext.Request.Path,
                    httpContext.Request.Method);
            }
            else
            {
                _logger.LogWarning(
                    "Request rejected with {StatusCode}. Path: {Path}, Method: {Method}, Reason: {Reason}",
                    statusCode,
                    httpContext.Request.Path,
                    httpContext.Request.Method,
                    exception.Message);
            }

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
