using FluentAssertions;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Application.Services.Implementations;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hotel.Reservation.Management.UnitTests.Services;

[TestClass]
public class ReservationServiceTests
{
    private Mock<IReservationRepository> _reservationRepository = null!;
    private Mock<IHotelRepository> _hotelRepository = null!;
    private Mock<ICustomerRepository> _customerRepository = null!;
    private ReservationService _sut = null!;

    private static DateTime Today => DateTime.UtcNow.Date;

    [TestInitialize]
    public void Setup()
    {
        _reservationRepository = new Mock<IReservationRepository>();
        _hotelRepository = new Mock<IHotelRepository>();
        _customerRepository = new Mock<ICustomerRepository>();

        _hotelRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HotelEntity("Hilton Athens", "Athens", 5));

        _customerRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerEntity("Angel", "Tsounis", "angel.tsounis@hotmail.com"));

        _reservationRepository
            .Setup(r => r.GetStatusActiveByCustomerAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _sut = new ReservationService(
            _reservationRepository.Object,
            _hotelRepository.Object,
            _customerRepository.Object);
    }

    private static ReservationRequest CreateValidRequest() => new()
    {
        HotelId = 1,
        CustomerId = 1,
        CheckInDate = Today,
        CheckOutDate = Today.AddDays(3),
        TotalPrice = 300m
    };

    [TestMethod]
    public async Task CreateAsync_WhenHotelDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _hotelRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((HotelEntity?)null);

        var request = CreateValidRequest();

        // Act
        var act = () => _sut.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Hotel with ID {request.HotelId} not found.");
    }

    [TestMethod]
    public async Task CreateAsync_WhenCustomerDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _customerRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CustomerEntity?)null);

        var request = CreateValidRequest();

        // Act
        var act = () => _sut.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Customer with ID {request.CustomerId} not found.");
    }

    [TestMethod]
    public async Task CreateAsync_WhenCustomerHasOverlappingReservation_ThrowsConflictException()
    {
        // Arrange
        var existing = new ReservationEntity(1, 1, Today, Today.AddDays(5), 500m);

        _reservationRepository
            .Setup(r => r.GetStatusActiveByCustomerAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([existing]);

        var request = CreateValidRequest();
        request.CheckInDate = Today.AddDays(2);
        request.CheckOutDate = Today.AddDays(7);

        // Act
        var act = () => _sut.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
        _reservationRepository.Verify(
            r => r.CreateAsync(It.IsAny<ReservationEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task CreateAsync_WithValidRequest_PersistsAndReturnsResponse()
    {
        // Arrange
        var request = CreateValidRequest();

        _reservationRepository
            .Setup(r => r.CreateAsync(It.IsAny<ReservationEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ReservationEntity reservation, CancellationToken _) => reservation);

        // Act
        var result = await _sut.CreateAsync(request);

        // Assert
        result.HotelId.Should().Be(request.HotelId);
        result.CustomerId.Should().Be(request.CustomerId);
        result.TotalPrice.Should().Be(request.TotalPrice);

        _reservationRepository.Verify(
            r => r.CreateAsync(It.IsAny<ReservationEntity>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenReservationDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingId = 999L;

        _reservationRepository
            .Setup(r => r.GetByIdAsync(missingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ReservationEntity?)null);

        // Act
        var act = () => _sut.GetByIdAsync(missingId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Reservation with ID {missingId} not found.");
    }
}