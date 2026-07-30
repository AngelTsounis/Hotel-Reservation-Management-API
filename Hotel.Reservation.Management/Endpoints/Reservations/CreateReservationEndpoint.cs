using Hotel.Reservation.Management.API.Filters;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservation.Management.API.Endpoints.Reservations
{
    public static class CreateReservationEndpoint
    {
        public const string Name = "CreateReservationEndpoint";

        public static RouteGroupBuilder MapToCreateReservationEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/", async ([FromBody] ReservationRequest request, IReservationService reservationService, CancellationToken cancellationToken) =>
            {
                var createdReservation = await reservationService.CreateAsync(request, cancellationToken);

                return Results.Created($"/api/reservations/{createdReservation.Id}", createdReservation);
            })
            .WithName(Name)
            .AddEndpointFilter<ValidationFilter<ReservationRequest>>()
            .Produces<ReservationResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

            return group;
        }
    }
}