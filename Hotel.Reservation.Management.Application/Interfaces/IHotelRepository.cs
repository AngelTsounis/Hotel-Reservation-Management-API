using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<HotelEntity> CreateAsync(HotelEntity hotel, CancellationToken cancellationToken = default);
        Task<List<HotelEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<HotelEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<HotelEntity> UpdateAsync(HotelEntity hotel, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
