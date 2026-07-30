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
        };
    }

    public static ReservationSearchResponse ToReservationSearchResponse(this ReservationEntity entity)
    {
        return new ReservationSearchResponse
        {
            Id = entity.Id,
            HotelId = entity.HotelId,
            HotelName = entity.Hotel.Name,
            CustomerId = entity.CustomerId,
            CustomerName = $"{entity.Customer.FirstName} {entity.Customer.LastName}",
            City = entity.Hotel.City,
            CheckInDate = entity.CheckInDate,
            CheckOutDate = entity.CheckOutDate,
            TotalPrice = entity.TotalPrice,
            Status = entity.Status,
        };
    }
}