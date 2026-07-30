using Hotel.Reservation.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Reservation.Management.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase
{
    protected AppDbContext DbContext { get; private set; } = null!;

    [TestInitialize]
    public async Task BaseSetupAsync()
    {
        DbContext = DatabaseFixture.CreateDbContext();

        await DbContext.Database.ExecuteSqlRawAsync(
            """TRUNCATE TABLE "Reservations", "Hotels", "Customers" RESTART IDENTITY CASCADE;""");
    }

    [TestCleanup]
    public async Task BaseCleanupAsync()
    {
        await DbContext.DisposeAsync();
    }
}