using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;

namespace Hotel.Reservation.Management.Application.Services.Interfaces
{
    public interface IHotelService
    {
        Task<HotelResponse> CreateHotelEntryServiceAsync(CreateHotelRequest request, CancellationToken cancellationToken = default);
    }
}
