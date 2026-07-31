using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Application.Mapping;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Hotel.Reservation.Management.Domain.Exceptions;
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

        public async Task<HotelResponse> CreateAsync(HotelRequest request, CancellationToken cancellationToken = default)
        {
            var hotel = new HotelEntity(request.Name, request.City, request.Stars);

            var createdHotel = await _hotelRepository.CreateAsync(hotel, cancellationToken);

            return createdHotel.ToHotelResponse();
        }

        public async Task<IReadOnlyList<HotelResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var hotels = await _hotelRepository.GetAllAsync(cancellationToken);

            return hotels.Select(h => h.ToHotelResponse()).ToList();
        }

        public async Task<HotelResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);

            if (hotel is null)
            {
                throw new NotFoundException($"Hotel with ID {id} not found.");
            }

            return hotel.ToHotelResponse();
        }

        public async Task<HotelResponse> UpdateAsync(long id, HotelRequest request, CancellationToken cancellationToken = default)
        {
            var existingHotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);

            if (existingHotel is null)
            {
                throw new NotFoundException($"Hotel with ID {id} not found.");
            }

            existingHotel.Update(request.Name, request.City, request.Stars);

            var updatedHotel = await _hotelRepository.UpdateAsync(existingHotel, cancellationToken);

            return updatedHotel.ToHotelResponse();
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id, cancellationToken);

            if (hotel is null)
            {
                return false;
            }

            if (await _hotelRepository.HasReservationsAsync(id, cancellationToken))
            {
                throw new ConflictException(
                    $"Hotel with ID {id} cannot be deleted because it has existing reservations.");
            }

            return await _hotelRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
