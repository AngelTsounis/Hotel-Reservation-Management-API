namespace Hotel.Reservation.Management.Application.Contracts.Response;

public class ErrorResponse
{
    public int Status { get; set; }

    public IReadOnlyList<string> Errors { get; set; } = [];
}