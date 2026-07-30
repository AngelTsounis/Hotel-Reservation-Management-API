using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Contracts.Response;
using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Application.Mapping;
using Hotel.Reservation.Management.Application.Services.Interfaces;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Domain.Model;

namespace Hotel.Reservation.Management.Application.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ICustomerRepository _customerRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IHotelRepository hotelRepository,
        ICustomerRepository customerRepository)
    {
        _reservationRepository = reservationRepository;
        _hotelRepository = hotelRepository;
        _customerRepository = customerRepository;
    }

    public async Task<ReservationResponse> CreateAsync(ReservationRequest request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
        {
            throw new NotFoundException($"Hotel with ID {request.HotelId} not found.");
        }

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            throw new NotFoundException($"Customer with ID {request.CustomerId} not found.");
        }

        await EnsureNoOverlappingReservationAsync(
            request.CustomerId,
            request.CheckInDate,
            request.CheckOutDate,
            cancellationToken);

        var reservation = new ReservationEntity(
            request.HotelId,
            request.CustomerId,
            request.CheckInDate,
            request.CheckOutDate,
            request.TotalPrice);

        var createdReservation = await _reservationRepository.CreateAsync(reservation, cancellationToken);

        return createdReservation.ToReservationResponse();
    }

    public async Task<IReadOnlyList<ReservationResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var reservations = await _reservationRepository.GetAllAsync(cancellationToken);

        return reservations.Select(r => r.ToReservationResponse()).ToList();
    }

    public async Task<ReservationResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id, cancellationToken);

        if (reservation is null)
        {
            throw new NotFoundException($"Reservation with ID {id} not found.");
        }

        return reservation.ToReservationResponse();
    }

    public async Task<bool> CancelAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _reservationRepository.CancelAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<ReservationSearchResponse>> SearchAsync(ReservationSearchRequest request, CancellationToken cancellationToken = default)
    {
        var reservations = await _reservationRepository.SearchAsync(request, cancellationToken);

        return reservations.Select(r => r.ToReservationSearchResponse()).ToList();
    }

    private async Task EnsureNoOverlappingReservationAsync(
        long customerId,
        DateTime checkInDate,
        DateTime checkOutDate,
        CancellationToken cancellationToken)
    {
        var existingReservations = await _reservationRepository
            .GetStatusActiveByCustomerAsync(customerId, cancellationToken);

        var hasOverlap = existingReservations.Any(r => r.OverlapsWith(checkInDate, checkOutDate));

        if (hasOverlap)
        {
            throw new ConflictException(
                $"Customer with ID {customerId} already has a reservation overlapping the requested dates.");
        }
    }
}