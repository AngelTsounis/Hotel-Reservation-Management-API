using FluentAssertions;
using Hotel.Reservation.Management.Domain.Model;
using Hotel.Reservation.Management.Infrastructure.Repositories;
using Hotel.Reservation.Management.IntegrationTests.Infrastructure;

namespace Hotel.Reservation.Management.IntegrationTests.Repositories;

[TestClass]
public class CustomerRepositoryTests : IntegrationTestBase
{
    private CustomerRepository _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _sut = new CustomerRepository(DbContext);
    }

    [TestMethod]
    public async Task ExistsByEmailAsync_WhenEmailExists_ReturnsTrue()
    {
        // Arrange
        DbContext.Customers.Add(new CustomerEntity("John", "Doe", "john.doe@example.com"));
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.ExistsByEmailAsync("john.doe@example.com", CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ExistsByEmailAsync_WhenEmailDiffersOnlyByCasingOrWhitespace_ReturnsTrue()
    {
        // Arrange
        DbContext.Customers.Add(new CustomerEntity("John", "Doe", "john.doe@example.com"));
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.ExistsByEmailAsync("  John.DOE@Example.com  ", CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ExistsByEmailAsync_WhenEmailDoesNotExist_ReturnsFalse()
    {
        // Arrange
        DbContext.Customers.Add(new CustomerEntity("John", "Doe", "john.doe@example.com"));
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _sut.ExistsByEmailAsync("someone.else@example.com", CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task CreateAsync_PersistsCustomerAndAssignsId()
    {
        // Arrange
        var customer = new CustomerEntity("Maria", "Papadopoulou", "maria@example.com");

        // Act
        var created = await _sut.CreateAsync(customer, CancellationToken.None);

        // Assert
        created.Id.Should().BeGreaterThan(0);

        await using var verificationContext = DatabaseFixture.CreateDbContext();
        var persisted = await verificationContext.Customers.FindAsync(created.Id);
        persisted.Should().NotBeNull();
        persisted!.Email.Should().Be("maria@example.com");
    }
}