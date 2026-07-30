using Hotel.Reservation.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Hotel.Reservation.Management.IntegrationTests.Infrastructure;

[TestClass]
public static class DatabaseFixture
{
    private static PostgreSqlContainer _container = null!;

    public static string ConnectionString { get; private set; } = string.Empty;

    [AssemblyInitialize]
    public static async Task InitializeAsync(TestContext _)
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("hotel_reservation_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _container.StartAsync();

        ConnectionString = _container.GetConnectionString();

        await using var dbContext = CreateDbContext();
        await dbContext.Database.MigrateAsync();
    }

    [AssemblyCleanup]
    public static async Task CleanupAsync()
    {
        await _container.DisposeAsync();
    }

    public static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new AppDbContext(options);
    }
}