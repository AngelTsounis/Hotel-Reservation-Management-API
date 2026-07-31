using FluentAssertions;
using Hotel.Reservation.Management.Domain.Exceptions;
using Hotel.Reservation.Management.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hotel.Reservation.Management.UnitTests.Domain;

[TestClass]
public class CustomerEntityTests
{
    [TestMethod]
    public void Constructor_WithValidData_TrimsAndNormalizesValues()
    {
        // Arrange
        var firstName = "  Angel  ";
        var lastName = "  Tsounis  ";
        var email = "  Angel.TSOUNIS@Hotmail.com  ";

        // Act
        var customer = new CustomerEntity(firstName, lastName, email);

        // Assert
        customer.FirstName.Should().Be("Angel");
        customer.LastName.Should().Be("Tsounis");
        customer.Email.Should().Be("angel.tsounis@hotmail.com");
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Constructor_WhenFirstNameIsMissing_ThrowsBusinessRuleException(string firstName)
    {
        // Arrange
        var lastName = "Tsounis";
        var email = "angel.tsounis@hotmail.com";

        // Act
        var act = () => new CustomerEntity(firstName, lastName, email);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("First name is required.");
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Constructor_WhenLastNameIsMissing_ThrowsBusinessRuleException(string lastName)
    {
        // Arrange
        var firstName = "Angel";
        var email = "angel.tsounis@hotmail.com";

        // Act
        var act = () => new CustomerEntity(firstName, lastName, email);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("Last name is required.");
    }

    [TestMethod]
    public void Constructor_WhenEmailIsInvalid_ThrowsBusinessRuleException()
    {
        // Arrange
        var invalidEmail = "not-an-email";

        // Act
        var act = () => new CustomerEntity("Angel", "Tsounis", invalidEmail);

        // Assert
        act.Should().Throw<BusinessRuleException>()
            .WithMessage("A valid email address is required.");
    }
}