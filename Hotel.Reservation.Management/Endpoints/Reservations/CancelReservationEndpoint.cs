using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Reservations
{
    public static class CancelReservationEndpoint
    {
        public const string Name = "CancelReservationEndpoint";

        public static RouteGroupBuilder MapToCancelReservationEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:long}", async (long id, IReservationService reservationService, CancellationToken cancellationToken) =>
            {
                var cancelled = await reservationService.CancelAsync(id, cancellationToken);

                return cancelled ? Results.NoContent() : Results.NotFound();
            })
            .WithName(Name)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}