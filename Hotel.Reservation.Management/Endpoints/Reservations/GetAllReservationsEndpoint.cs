using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Reservations
{
    public static class GetAllReservationsEndpoint
    {
        public const string Name = "GetAllReservationsEndpoint";

        public static RouteGroupBuilder MapToGetAllReservationsEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IReservationService reservationService, CancellationToken cancellationToken) =>
            {
                var reservations = await reservationService.GetAllAsync(cancellationToken);

                return Results.Ok(reservations);
            })
            .WithName(Name)
            .Produces<IReadOnlyList<ReservationResponse>>(StatusCodes.Status200OK);

            return group;
        }
    }
}