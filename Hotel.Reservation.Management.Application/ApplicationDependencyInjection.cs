using Hotel.Reservation.Management.Application.Services.Implementations;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Reservation.Management.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IHotelService, HotelService>();

        return services;
    }
}