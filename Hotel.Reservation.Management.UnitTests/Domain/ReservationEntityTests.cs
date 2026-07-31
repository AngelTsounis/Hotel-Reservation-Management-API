using FluentAssertions;
using Hotel.Reservation.Management.Domain.Enums;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hotel.Reservation.Management.UnitTests.Domain;

[TestClass]
public class ReservationEntityTests
{
    private static DateTime Today => DateTime.UtcNow.Date;

    private static ReservationEntity CreateValidReservation() =>
        new(hotelId: 1, customerId: 1, checkInDate: Today, checkOutDate: Today.AddDays(3), totalPrice: 150m);

    [TestMethod]
    public void Constructor_WithValidData_CreatesActiveReservation()
    {
        // Arrange
        var checkInDate = Today;
        var checkOutDate = Today.AddDays(3);

        // Act
        var reservation = new ReservationEntity(1, 1, checkInDate, checkOutDate, 150m);

        // Assert
        reservation.Status.Should().Be(ReservationStatus.Active);
        reservation.CheckInDate.Should().Be(checkInDate);
        reservation.CheckOutDate.Should().Be(checkOutDate);
        reservation.TotalPrice.Should().Be(150m);
    }

    [TestMethod]
    public void Constructor_WhenCheckOutIsNotAfterCheckIn_ThrowsBusinessRuleException()
    {
        // Arrange
        var sameDate = Today.AddDays(5);

        // Act
        var act = () => new ReservationEntity(1, 1, sameDate, sameDate, 100m);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Check-out date must be after the check-in date.");
    }

    [TestMethod]
    public void Constructor_WhenTotalPriceIsNegative_ThrowsBusinessRuleException()
    {
        // Arrange
        var negativePrice = -1m;

        // Act
        var act = () => new ReservationEntity(1, 1, Today, Today.AddDays(2), negativePrice);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Total price cannot be negative.");
    }

    [TestMethod]
    public void Cancel_WhenActive_SetsStatusToCancelled()
    {
        // Arrange
        var reservation = CreateValidReservation();

        // Act
        reservation.Cancel();

        // Assert
        reservation.Status.Should().Be(ReservationStatus.Cancelled);
        reservation.IsActive.Should().BeFalse();
    }

    [TestMethod]
    public void Cancel_WhenAlreadyCancelled_ThrowsBusinessRuleException()
    {
        // Arrange
        var reservation = CreateValidReservation();
        reservation.Cancel();

        // Act
        var act = () => reservation.Cancel();

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Reservation has already been cancelled.");
    }

    [DataTestMethod]
    [DataRow(2, 5, true)]    // starts inside existing range
    [DataRow(3, 6, false)]   // starts exactly when existing ends
    [DataRow(10, 12, false)] // completely after
    public void OverlapsWith_ReturnsExpectedResult(int startOffset, int endOffset, bool expected)
    {
        // Arrange
        var reservation = CreateValidReservation(); // Today -> Today + 3
        var checkIn = Today.AddDays(startOffset);
        var checkOut = Today.AddDays(endOffset);

        // Act
        var result = reservation.OverlapsWith(checkIn, checkOut);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    public void OverlapsWith_WhenReservationIsCancelled_ReturnsFalse()
    {
        // Arrange
        var reservation = CreateValidReservation();
        reservation.Cancel();

        // Act
        var result = reservation.OverlapsWith(Today, Today.AddDays(3));

        // Assert
        result.Should().BeFalse();
    }
}