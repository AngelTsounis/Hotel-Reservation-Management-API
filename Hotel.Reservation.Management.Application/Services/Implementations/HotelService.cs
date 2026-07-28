using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Application.Mapping;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<HotelResponse> CreateHotelEntryServiceAsync(CreateHotelRequest request, CancellationToken cancellationToken = default)
        {
            var hotel = new HotelEntity(request.Name, request.City, request.Stars);

            var createdHotel = await _hotelRepository.CreateHotelEntryAsync(hotel, cancellationToken);

            return createdHotel.ToHotelResponse();
        }
    }
}
