using Hotel.Reservation.Management.API.Filters;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservation.Management.API.Endpoints.Hotels
{
    public static class UpdateHotelEndpoint
    {
        public const string Name = "UpdateHotelEndpoint";

        public static IEndpointRouteBuilder MapToUpdateHotel(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/hotels/{id:long}", async (long id, [FromBody] HotelRequest request, IHotelService hotelService, CancellationToken cancellationToken) =>
            {
                var updatedHotel = await hotelService.UpdateAsync(id, request, cancellationToken);

                return Results.Ok(updatedHotel);
            })
            .WithName(Name)
            .WithTags("Hotels")
            .AddEndpointFilter<ValidationFilter<HotelRequest>>()
            .Produces<HotelResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

            return app;
        }
    }
}