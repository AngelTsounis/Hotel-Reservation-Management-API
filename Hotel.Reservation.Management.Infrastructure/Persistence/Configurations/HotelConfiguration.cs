using Hotel.Reservation.Management.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Reservation.Management.Infrastructure.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<HotelEntity>
{
    public void Configure(EntityTypeBuilder<HotelEntity> builder)
    {
        builder.ToTable("Hotels");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(h => h.City)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(h => h.Stars)
               .IsRequired();

        builder.HasIndex(h => h.City);
    }
}