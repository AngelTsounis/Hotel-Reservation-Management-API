using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Mapping;

public static class ReservationMapping
{
    public static ReservationResponse ToReservationResponse(this ReservationEntity entity)
    {
        return new ReservationResponse
        {
            Id = entity.Id,
            HotelId = entity.HotelId,
            CustomerId = entity.CustomerId,
            CheckInDate = entity.CheckInDate,
            CheckOutDate = entity.CheckOutDate,
            TotalPrice = entity.TotalPrice,
            Status = entity.Status,
            Nights = entity.Nights
        };
    }
}