using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Search
{
    public static class SearchReservationsEndpoint
    {
        public const string Name = "SearchReservationsEndpoint";

        public static RouteGroupBuilder MapToSearchReservationsEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/search", async ([AsParameters] ReservationSearchRequest request, IReservationService reservationService, CancellationToken cancellationToken) =>
            {
                var reservations = await reservationService.SearchAsync(request, cancellationToken);

                return Results.Ok(reservations);
            })
            .WithName(Name)
            .Produces<IReadOnlyList<ReservationSearchResponse>>(StatusCodes.Status200OK);

            return group;
        }
    }
}
