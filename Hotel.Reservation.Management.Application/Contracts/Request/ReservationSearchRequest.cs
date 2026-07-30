using Hotel.Reservation.Management.Domain.Enums;

namespace Hotel.Reservation.Management.Application.Contracts.Request;

public class ReservationSearchRequest
{
    public string? HotelName { get; set; }

    public string? CustomerName { get; set; }

    public string? City { get; set; }

    public ReservationStatus? Status { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }
}