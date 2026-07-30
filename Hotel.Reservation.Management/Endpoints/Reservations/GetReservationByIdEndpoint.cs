using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Reservations
{
    public static class GetReservationByIdEndpoint
    {
        public const string Name = "GetReservationByIdEndpoint";

        public static RouteGroupBuilder MapToGetReservationByIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:long}", async (long id, IReservationService reservationService, CancellationToken cancellationToken) =>
            {
                var reservation = await reservationService.GetByIdAsync(id, cancellationToken);

                return Results.Ok(reservation);
            })
            .WithName(Name)
            .Produces<ReservationResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

            return group;
        }
    }
}