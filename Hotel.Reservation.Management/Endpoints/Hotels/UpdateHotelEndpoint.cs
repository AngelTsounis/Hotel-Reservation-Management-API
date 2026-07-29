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

        public static RouteGroupBuilder MapToUpdateHotel(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:long}", async (long id, [FromBody] HotelRequest request, IHotelService hotelService, CancellationToken cancellationToken) =>
            {
                var updatedHotel = await hotelService.UpdateAsync(id, request, cancellationToken);

                return Results.Ok(updatedHotel);
            })
            .WithName(Name)
            .AddEndpointFilter<ValidationFilter<HotelRequest>>()
            .Produces<HotelResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

            return group;
        }
    }
}