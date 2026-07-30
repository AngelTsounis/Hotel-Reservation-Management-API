namespace Hotel.Reservation.Management.Application.Contracts.Request;

public class CustomerRequest
{
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}
