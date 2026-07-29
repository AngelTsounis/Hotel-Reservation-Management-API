using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Customers;

public static class GetCustomerByIdEndpoint
{
    public const string Name = "GetCustomerByIdEndpoint";

    public static RouteGroupBuilder MapToGetCustomerByIdEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}", async (long id, ICustomerService customerService, CancellationToken cancellationToken) =>
        {
            var customer = await customerService.GetByIdAsync(id, cancellationToken);

            return Results.Ok(customer);
        })
        .WithName(Name)
        .Produces<CustomerResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        return group;
    }
}