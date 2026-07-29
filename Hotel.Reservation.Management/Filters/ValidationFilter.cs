using FluentValidation;
using Hotel.Reservation.Management.Application.Contracts.Response;

namespace Hotel.Reservation.Management.API.Filters
{
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var argument = context.Arguments.OfType<T>().FirstOrDefault();

            if (argument is null)
            {
                return Results.Json(new ErrorResponse
                {
                    Status = StatusCodes.Status400BadRequest,
                    Errors = [$"Expected a {typeof(T).Name} payload."]
                }, statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await _validator.ValidateAsync(argument, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                return Results.Json(new ErrorResponse
                {
                    Status = StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(e => e.ErrorMessage).ToArray()
                }, statusCode: StatusCodes.Status400BadRequest);
            }

            return await next(context);
        }
    }
}