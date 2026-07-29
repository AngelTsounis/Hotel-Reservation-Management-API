using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Hotels
{
    public static class GetHotelByIdEndpoint
    {
        public const string Name = "GetHotelByIdEndpoint";

        public static RouteGroupBuilder MapToGetHotelById(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:long}", async (long id, IHotelService hotelService, CancellationToken cancellationToken) =>
            {
                var hotel = await hotelService.GetByIdAsync(id, cancellationToken);

                return hotel is null ? Results.NotFound() : Results.Ok(hotel);
            })
            .WithName(Name)
            .Produces<HotelResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}