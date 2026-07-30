using FluentAssertions;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hotel.Reservation.Management.UnitTests.Domain;

[TestClass]
public class HotelEntityTests
{
    [TestMethod]
    public void Constructor_WithValidData_TrimsValues()
    {
        // Arrange
        var name = "  Hilton  ";
        var city = "  Athens  ";
        var stars = 5;

        // Act
        var hotel = new HotelEntity(name, city, stars);

        // Assert
        hotel.Name.Should().Be("Hilton");
        hotel.City.Should().Be("Athens");
        hotel.Stars.Should().Be(5);
    }

    [TestMethod]
    public void Constructor_WhenNameIsMissing_ThrowsBusinessRuleException()
    {
        // Arrange
        var emptyName = "   ";

        // Act
        var act = () => new HotelEntity(emptyName, "Athens", 5);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Hotel name is required.");
    }

    [TestMethod]
    public void Constructor_WhenCityIsMissing_ThrowsBusinessRuleException()
    {
        // Arrange
        var emptyCity = "   ";

        // Act
        var act = () => new HotelEntity("Hilton", emptyCity, 5);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Hotel city is required.");
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(6)]
    public void Constructor_WhenStarsAreOutOfRange_ThrowsBusinessRuleException(int stars)
    {
        // Arrange
        var name = "Hilton";
        var city = "Athens";

        // Act
        var act = () => new HotelEntity(name, city, stars);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Hotel stars must be between 1 and 5.");
    }

    [TestMethod]
    public void Update_WithValidData_OverwritesExistingValues()
    {
        // Arrange
        var hotel = new HotelEntity("Hilton", "Athens", 5);

        // Act
        hotel.Update("Marriott", "Patras", 4);

        // Assert
        hotel.Name.Should().Be("Marriott");
        hotel.City.Should().Be("Patras");
        hotel.Stars.Should().Be(4);
    }
}