using Hotel.Reservation.Management.Domain.Enums;

namespace Hotel.Reservation.Management.Application.Contracts.Response;

public class ReservationSearchResponse
{
    public long Id { get; set; }

    public long HotelId { get; set; }

    public string HotelName { get; set; } = string.Empty;

    public long CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; }
}