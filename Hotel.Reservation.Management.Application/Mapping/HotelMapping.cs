using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Mapping
{
    public static class HotelMapping
    {
        public static HotelResponse ToHotelResponse(this HotelEntity hotelEntity)
        {
            return new HotelResponse
            {
                Id = hotelEntity.Id,
                Name = hotelEntity.Name,
                City = hotelEntity.City,
                Stars = hotelEntity.Stars
            };
        }
    }
}
