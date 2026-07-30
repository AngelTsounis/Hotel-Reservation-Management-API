using FluentAssertions;
using Hotel.Reservation.Management.Application.Contracts.Request;
using Hotel.Reservation.Management.Application.Interfaces;
using Hotel.Reservation.Management.Application.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hotel.Reservation.Management.UnitTests.Validations;

[TestClass]
public class CustomerRequestValidatorTests
{
    private Mock<ICustomerRepository> _customerRepository = null!;
    private CustomerRequestValidator _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _customerRepository = new Mock<ICustomerRepository>();

        _customerRepository
            .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _sut = new CustomerRequestValidator(_customerRepository.Object);
    }

    private static CustomerRequest CreateValidRequest() => new()
    {
        firstName = "John",
        lastName = "Doe",
        email = "john.doe@example.com"
    };

    [TestMethod]
    public async Task ValidateAsync_WithValidUniqueRequest_PassesValidation()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = await _sut.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public async Task ValidateAsync_WhenFirstNameIsMissing_FailsValidation(string firstName)
    {
        // Arrange
        var request = CreateValidRequest();
        request.firstName = firstName;

        // Act
        var result = await _sut.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Customer first name is required.");
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public async Task ValidateAsync_WhenLastNameIsMissing_FailsValidation(string lastName)
    {
        // Arrange
        var request = CreateValidRequest();
        request.lastName = lastName;

        // Act
        var result = await _sut.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Customer last name is required.");
    }

    [TestMethod]
    public async Task ValidateAsync_WhenEmailFormatIsInvalid_FailsValidation()
    {
        // Arrange
        var request = CreateValidRequest();
        request.email = "not-an-email";

        // Act
        var result = await _sut.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Customer email must be a valid email address.");
    }

    [TestMethod]
    public async Task ValidateAsync_WhenEmailAlreadyExists_FailsValidation()
    {
        // Arrange
        var request = CreateValidRequest();

        _customerRepository
            .Setup(r => r.ExistsByEmailAsync(request.email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage == $"A customer with email '{request.email}' already exists.");
    }

    [TestMethod]
    public async Task ValidateAsync_WhenEmailIsUnique_DoesNotReportDuplicateError()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = await _sut.ValidateAsync(request);

        // Assert
        _customerRepository.Verify(
            r => r.ExistsByEmailAsync(request.email, It.IsAny<CancellationToken>()),
            Times.Once);
        result.IsValid.Should().BeTrue();
    }
}