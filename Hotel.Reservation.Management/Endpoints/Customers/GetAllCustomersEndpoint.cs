using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Customers;

public static class GetAllCustomersEndpoint
{
    public const string Name = "GetAllCustomersEndpoint";

    public static RouteGroupBuilder MapToGetAllCustomersEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (ICustomerService customerService, CancellationToken cancellationToken) =>
        {
            var customers = await customerService.GetAllAsync(cancellationToken);

            return Results.Ok(customers);
        })
        .WithName(Name)
        .Produces<List<CustomerResponse>>(StatusCodes.Status200OK);

        return group;
    }
}
