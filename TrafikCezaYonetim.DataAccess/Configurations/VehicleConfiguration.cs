using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.DataAccess.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PlateNumber).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.PlateNumber).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Brand).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.UsageType).HasMaxLength(50);
        builder.Property(x => x.Ownership).HasMaxLength(50);
        builder.Property(x => x.SerialNumber).HasMaxLength(50);
        builder.Property(x => x.FuelType).HasMaxLength(50);

        builder.HasOne(x => x.VehicleType)
               .WithMany(x => x.Vehicles)
               .HasForeignKey(x => x.VehicleTypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}