namespace Hotel.Reservation.Management.Application.Contracts.Request;

public class HotelRequest
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Stars { get; set; }
}
