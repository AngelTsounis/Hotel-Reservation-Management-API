using Hotel.Reservation.Management.API.Endpoints.Customers;
using Hotel.Reservation.Management.API.Endpoints.Hotels;

namespace Hotel.Reservation.Management.API.Endpoints
{
    public static class EndpointsExtensions
    {
        public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapHotelEndpoints();
            app.MapCustomerEndpoints();
            return app;
        }

        private static IEndpointRouteBuilder MapHotelEndpoints(this IEndpointRouteBuilder app)
        {
            var hotels = app.MapGroup("/api/hotels")
                            .WithTags("Hotels");

            hotels.MapToCreateHotelEntity();
            hotels.MapToGetAllHotels();
            hotels.MapToGetHotelById();
            hotels.MapToUpdateHotel();
            hotels.MapToDeleteHotel();

            return app;
        }

        private static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var customers = app.MapGroup("/api/customer")
                               .WithTags("Customer");

            customers.MapToCreateCustomerEndpoint();
            customers.MapToGetAllCustomersEndpoint();
            customers.MapToGetCustomerByIdEndpoint();

            return app;
        }
    }
}
