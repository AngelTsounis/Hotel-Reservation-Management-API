using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;

namespace Hotel.Reservation.Management.Application.Services.Interfaces
{
    public interface IHotelService
    {
        Task<HotelResponse> CreateAsync(HotelRequest request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<HotelResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<HotelResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<HotelResponse> UpdateAsync(long id, HotelRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
