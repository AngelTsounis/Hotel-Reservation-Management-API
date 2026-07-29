using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Interfaces;

public interface IReservationRepository
{
    Task<ReservationEntity> CreateAsync(ReservationEntity entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<ReservationEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<ReservationEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ReservationEntity>> GetActiveByCustomerAsync(long customerId, CancellationToken cancellationToken);
    Task<bool> CancelAsync(long id, CancellationToken cancellationToken);
}