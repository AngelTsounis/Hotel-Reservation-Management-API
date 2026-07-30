using FluentAssertions;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Domain.Enums;
using Hotel.Reservation.Management.Domain.Model;
using Hotel.Reservation.Management.Infrastructure.Repositories;
using Hotel.Reservation.Management.IntegrationTests.Infrastructure;

namespace Hotel.Reservation.Management.IntegrationTests.Repositories;

[TestClass]
public class ReservationRepositoryTests : IntegrationTestBase
{
    private ReservationRepository _sut = null!;

    private static DateTime Today => DateTime.UtcNow.Date;

    [TestInitialize]
    public void Setup()
    {
        _sut = new ReservationRepository(DbContext);
    }

    private async Task<(HotelEntity Hotel, CustomerEntity Customer)> SeedHotelAndCustomerAsync(
        string hotelName = "Hilton Athens",
        string city = "Athens",
        string firstName = "John",
        string lastName = "Doe",
        string email = "john.doe@example.com")
    {
        var hotel = new HotelEntity(hotelName, city, 5);
        var customer = new CustomerEntity(firstName, lastName, email);

        DbContext.Hotels.Add(hotel);
        DbContext.Customers.Add(customer);
        await DbContext.SaveChangesAsync();

        return (hotel, customer);
    }

    [TestMethod]
    public async Task SearchAsync_WithNoFilters_ReturnsAllReservations()
    {
        // Arrange
        var (hotel, customer) = await SeedHotelAndCustomerAsync();
        DbContext.Reservations.Add(new ReservationEntity(hotel.Id, customer.Id, Today, Today.AddDays(3), 300m));
        DbContext.Reservations.Add(new ReservationEntity(hotel.Id, customer.Id, Today.AddDays(10), Today.AddDays(12), 200m));
        await DbContext.SaveChangesAsync();

        var request = new ReservationSearchRequest();

        // Act
        var result = await _sut.SearchAsync(request, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task SearchAsync_ByHotelName_IsCaseInsensitiveAndPartial()
    {
        // Arrange
        var (hotel, customer) = await SeedHotelAndCustomerAsync(hotelName: "Hilton Athens");
        DbContext.Reservations.Add(new ReservationEntity(hotel.Id, customer.Id, Today, Today.AddDays(3), 300m));
        await DbContext.SaveChangesAsync();

        var request = new ReservationSearchRequest { HotelName = "hilTON" };

        // Act
        var result = await _sut.SearchAsync(request, CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Hotel.Name.Should().Be("Hilton Athens");
    }

    [TestMethod]
    public async Task SearchAsync_ByCustomerName_MatchesFirstOrLastName()
    {
        // Arrange
        var (hotel, customer) = await SeedHotelAndCustomerAsync(firstName: "Maria", lastName: "Papadopoulou");
        DbContext.Reservations.Add(new ReservationEntity(hotel.Id, customer.Id, Today, Today.AddDays(3), 300m));
        await DbContext.SaveChangesAsync();

        var request = new ReservationSearchRequest { CustomerName = "papado" };

        // Act
        var result = await _sut.SearchAsync(request, CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Customer.LastName.Should().Be("Papadopoulou");
    }

    [TestMethod]
    public async Task SearchAsync_ByStatus_ReturnsOnlyMatchingStatus()
    {
        // Arrange
        var (hotel, customer) = await SeedHotelAndCustomerAsync();

        var active = new ReservationEntity(hotel.Id, customer.Id, Today, Today.AddDays(3), 300m);
        var cancelled = new ReservationEntity(hotel.Id, customer.Id, Today.AddDays(10), Today.AddDays(12), 200m);
        cancelled.Cancel();

        DbContext.Reservations.AddRange(active, cancelled);
        await DbContext.SaveChangesAsync();

        var request = new ReservationSearchRequest { Status = ReservationStatus.Cancelled };

        // Act
        var result = await _sut.SearchAsync(request, CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Status.Should().Be(ReservationStatus.Cancelled);
    }

    [TestMethod]
    public async Task SearchAsync_WithMultipleFilters_AppliesAllOfThem()
    {
        // Arrange
        var (athensHotel, customer) = await SeedHotelAndCustomerAsync(hotelName: "Hilton Athens", city: "Athens");

        var patrasHotel = new HotelEntity("Hilton Patras", "Patras", 4);
        DbContext.Hotels.Add(patrasHotel);
        await DbContext.SaveChangesAsync();

        DbContext.Reservations.Add(new ReservationEntity(athensHotel.Id, customer.Id, Today, Today.AddDays(3), 300m));
        DbContext.Reservations.Add(new ReservationEntity(patrasHotel.Id, customer.Id, Today.AddDays(10), Today.AddDays(12), 200m));
        await DbContext.SaveChangesAsync();

        var request = new ReservationSearchRequest
        {
            HotelName = "Hilton",
            City = "Patras",
            Status = ReservationStatus.Active
        };

        // Act
        var result = await _sut.SearchAsync(request, CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Hotel.City.Should().Be("Patras");
    }

    [TestMethod]
    public async Task CancelAsync_WhenReservationExists_PerformsLogicalCancellation()
    {
        // Arrange
        var (hotel, customer) = await SeedHotelAndCustomerAsync();
        var reservation = new ReservationEntity(hotel.Id, customer.Id, Today, Today.AddDays(3), 300m);
        DbContext.Reservations.Add(reservation);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.CancelAsync(reservation.Id, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        await using var verificationContext = DatabaseFixture.CreateDbContext();
        var persisted = await verificationContext.Reservations.FindAsync(reservation.Id);
        persisted.Should().NotBeNull();
        persisted!.Status.Should().Be(ReservationStatus.Cancelled);
    }

    [TestMethod]
    public async Task CancelAsync_WhenReservationDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = 9999L;

        // Act
        var result = await _sut.CancelAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetStatusActiveByCustomerAsync_ExcludesCancelledReservations()
    {
        // Arrange
        var (hotel, customer) = await SeedHotelAndCustomerAsync();

        var active = new ReservationEntity(hotel.Id, customer.Id, Today, Today.AddDays(3), 300m);
        var cancelled = new ReservationEntity(hotel.Id, customer.Id, Today.AddDays(10), Today.AddDays(12), 200m);
        cancelled.Cancel();

        DbContext.Reservations.AddRange(active, cancelled);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetStatusActiveByCustomerAsync(customer.Id, CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Status.Should().Be(ReservationStatus.Active);
    }
}