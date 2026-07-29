using Hotel.Reservation.Management.API.Filters;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservation.Management.API.Endpoints.Hotels
{
    public static class CreateHotelEntryEndpoint
    {
        public const string Name = "CreateHotelEntryEndpoint";

        public static IEndpointRouteBuilder MapToCreateHotelEntity(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/hotels", async ([FromBody] HotelRequest request, IHotelService hotelService, CancellationToken cancellationToken) =>
            {
                var createdHotel = await hotelService.CreateAsync(request, cancellationToken);

                return Results.Created($"/api/hotels/{createdHotel.Id}", createdHotel);
            })
            .WithName(Name)
            .WithTags("Hotels")
            .AddEndpointFilter<ValidationFilter<HotelRequest>>()
            .Produces<HotelResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            return app;
        }
    }
}