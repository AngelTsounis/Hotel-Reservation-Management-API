using Hotel.Reservation.Management.Domain.Enums;

namespace Hotel.Reservation.Management.Application.Contracts.Response;

public class ReservationResponse
{
    public long Id { get; set; }

    public long HotelId { get; set; }

    public long CustomerId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; }

    public int Nights { get; set; }
}
