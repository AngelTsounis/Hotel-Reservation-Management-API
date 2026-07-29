using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Hotels
{
    public static class GetAllHotelsEndpoint
    {
        public const string Name = "GetAllHotelsEndpoint";
        public static IEndpointRouteBuilder MapToGetAllHotels(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/hotels", async (IHotelService hotelService, CancellationToken cancellationToken) =>
            {
                var hotels = await hotelService.GetAllAsync(cancellationToken);
                return Results.Ok(hotels);
            })
            .WithName(Name)
            .WithTags("Hotels")
            .Produces<List<HotelResponse>>(StatusCodes.Status200OK);
            return app;
        }
    }
}
