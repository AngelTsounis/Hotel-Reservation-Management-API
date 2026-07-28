namespace Hotel.Reservation.Management.Domain.Model;

public abstract class BaseEntity
{
    public long Id { get; set; }

    public DateTime CreatedAuditUtc { get; set; }

    public DateTime? UpdatedAuditUtc { get; set; }
}
