using Hotel.Reservation.Management.API.Endpoints.Customers;
using Hotel.Reservation.Management.API.Endpoints.Hotels;
using Hotel.Reservation.Management.API.Endpoints.Reservations;
using Hotel.Reservation.Management.API.Endpoints.Search;

namespace Hotel.Reservation.Management.API.Endpoints
{
    public static class EndpointsExtensions
    {
        public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapHotelEndpoints();
            app.MapCustomerEndpoints();
            app.MapReservationEndpoints();
            app.MapSearchEndpoints();
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

        private static IEndpointRouteBuilder MapReservationEndpoints(this IEndpointRouteBuilder app)
        {
            var reservations = app.MapGroup("/api/reservations")
                                  .WithTags("Reservations");

            reservations.MapToCreateReservationEndpoint();
            reservations.MapToGetAllReservationsEndpoint();
            reservations.MapToGetReservationByIdEndpoint();
            reservations.MapToCancelReservationEndpoint();

            return app;
        }

        private static IEndpointRouteBuilder MapSearchEndpoints(this IEndpointRouteBuilder app)
        {
            var search = app.MapGroup("/api/search")
                            .WithTags("Search");
            search.MapToSearchReservationsEndpoint();

            return app;
        }
    }
}
