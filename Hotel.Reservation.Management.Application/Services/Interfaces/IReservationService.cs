using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;

namespace Hotel.Reservation.Management.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponse> CreateAsync(ReservationRequest request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ReservationResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ReservationResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> CancelAsync(long id, CancellationToken cancellationToken = default);
    }
}