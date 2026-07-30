using Hotel.Reservation.Management.API.Filters;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservation.Management.API.Endpoints.Customers
{
    public static class CreateCustomerEndpoint
    {
        public const string Name = "CreateCustomerEndpoint";

        public static RouteGroupBuilder MapToCreateCustomerEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/", async ([FromBody] CustomerRequest request, ICustomerService customerService, CancellationToken cancellationToken) =>
            {
                var createdCustomer = await customerService.CreateAsync(request, cancellationToken);

                return Results.Created($"/api/customer/{createdCustomer.Id}", createdCustomer);
            })
            .WithName(Name)
            .AddEndpointFilter<ValidationFilter<CustomerRequest>>()
            .Produces<CustomerResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}