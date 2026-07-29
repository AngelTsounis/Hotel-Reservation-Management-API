using Hotel.Reservation.Management.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Hotel.Reservation.Management.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<HotelEntity> Hotels => Set<HotelEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAuditUtc = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAuditUtc = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
