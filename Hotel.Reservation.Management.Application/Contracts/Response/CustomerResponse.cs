namespace Hotel.Reservation.Management.Application.Contracts.Response
{
    public class CustomerResponse
    {
        public long Id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}
