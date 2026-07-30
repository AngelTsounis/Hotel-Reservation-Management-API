using Hotel.Reservation.Management.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Reservation.Management.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.HasIndex(c => c.Email)
               .IsUnique();
    }
}