namespace Hotel.Reservation.Management.Application.Contracts.Request;

public class ReservationRequest
{
    public long HotelId { get; set; }

    public long CustomerId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public decimal TotalPrice { get; set; }
}
