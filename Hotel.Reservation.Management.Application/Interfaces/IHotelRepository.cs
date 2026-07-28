using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<HotelEntity> CreateHotelEntryAsync(HotelEntity hotel, CancellationToken cancellationToken = default);
    }
}
