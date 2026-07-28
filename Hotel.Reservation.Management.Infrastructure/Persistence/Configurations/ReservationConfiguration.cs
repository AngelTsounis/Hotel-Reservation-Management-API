using Hotel.Reservation.Management.Domain.Enums;
using Hotel.Reservation.Management.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Reservation.Management.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<ReservationEntity>
{
    public void Configure(EntityTypeBuilder<ReservationEntity> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CheckInDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(r => r.CheckOutDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(r => r.TotalPrice)
               .IsRequired()
               .HasPrecision(18, 2);

        builder.Property(r => r.Status)
              .IsRequired()
              .HasConversion(
                  status => status.ToString().ToUpperInvariant(),
                  value => Enum.Parse<ReservationStatus>(value, true))
              .HasMaxLength(20);

        builder.HasOne(r => r.Hotel)
               .WithMany(h => h.Reservations)
               .HasForeignKey(r => r.HotelId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
               .WithMany(c => c.Reservations)
               .HasForeignKey(r => r.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.CustomerId, r.Status });

        builder.Ignore(r => r.IsActive);
        builder.Ignore(r => r.Nights);
    }
}