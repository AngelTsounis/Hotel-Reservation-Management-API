using Hotel.Reservation.Management.Application.Services.Interfaces;

namespace Hotel.Reservation.Management.API.Endpoints.Hotels
{
    public static class DeleteHotelEndpoint
    {
        public const string Name = "DeleteHotelEndpoint";

        public static IEndpointRouteBuilder MapToDeleteHotel(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/hotels/{id:long}", async (long id, IHotelService hotelService, CancellationToken cancellationToken) =>
            {
                var deleted = await hotelService.DeleteAsync(id, cancellationToken);

                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName(Name)
            .WithTags("Hotels")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

            return app;
        }
    }
}