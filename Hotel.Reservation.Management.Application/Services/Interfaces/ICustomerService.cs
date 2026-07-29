using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;

namespace Hotel.Reservation.Management.Application.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(CustomerRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomerResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
