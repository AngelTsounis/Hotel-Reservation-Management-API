using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Interfaces;

public interface ICustomerRepository
{
    Task<CustomerEntity> CreateAsync(CustomerEntity entity, CancellationToken cancellationToken);
    Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<CustomerEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
