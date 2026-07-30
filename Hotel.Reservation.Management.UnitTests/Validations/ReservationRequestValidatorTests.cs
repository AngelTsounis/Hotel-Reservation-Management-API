using FluentAssertions;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Hotel.Reservation.Management.UnitTests.Validations;

[TestClass]
public class ReservationRequestValidatorTests
{
    private ReservationRequestValidator _sut = null!;

    private static DateTime Today => DateTime.UtcNow.Date;

    [TestInitialize]
    public void Setup()
    {
        _sut = new ReservationRequestValidator();
    }

    private static ReservationRequest CreateValidRequest() => new()
    {
        HotelId = 1,
        CustomerId = 1,
        CheckInDate = Today,
        CheckOutDate = Today.AddDays(3),
        TotalPrice = 150m
    };

    [TestMethod]
    public void Validate_WithValidRequest_PassesValidation()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Validate_WhenHotelIdIsNotPositive_FailsValidation(long hotelId)
    {
        // Arrange
        var request = CreateValidRequest();
        request.HotelId = hotelId;

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Hotel ID must be a valid identifier.");
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Validate_WhenCustomerIdIsNotPositive_FailsValidation(long customerId)
    {
        // Arrange
        var request = CreateValidRequest();
        request.CustomerId = customerId;

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Customer ID must be a valid identifier.");
    }

    [TestMethod]
    public void Validate_WhenCheckInDateIsInThePast_FailsValidation()
    {
        // Arrange
        var request = CreateValidRequest();
        request.CheckInDate = Today.AddDays(-1);

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Check-in date cannot be in the past.");
    }

    [TestMethod]
    public void Validate_WhenCheckOutDateEqualsCheckInDate_FailsValidation()
    {
        // Arrange
        var request = CreateValidRequest();
        request.CheckOutDate = request.CheckInDate;

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Check-out date must be after the check-in date.");
    }

    [TestMethod]
    public void Validate_WhenCheckOutDateIsBeforeCheckInDate_FailsValidation()
    {
        // Arrange
        var request = CreateValidRequest();
        request.CheckInDate = Today.AddDays(5);
        request.CheckOutDate = Today.AddDays(2);

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Check-out date must be after the check-in date.");
    }

    [TestMethod]
    public void Validate_WhenTotalPriceIsNegative_FailsValidation()
    {
        // Arrange
        var request = CreateValidRequest();
        request.TotalPrice = -1m;

        // Act
        var result = _sut.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Total price cannot be negative.");
    }
}