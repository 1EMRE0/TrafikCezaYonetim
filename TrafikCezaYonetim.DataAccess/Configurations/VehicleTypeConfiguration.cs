using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.DataAccess.Configurations;

public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Category).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasData(
            new VehicleType { Id = 1, Name = "Binek Araç (Otomobil)", Category = "KARAYOLU ARAÇLARI", IsActive = true },
            new VehicleType { Id = 2, Name = "Pick-up", Category = "KARAYOLU ARAÇLARI", IsActive = true },
            new VehicleType { Id = 3, Name = "Kamyonet", Category = "KARAYOLU ARAÇLARI", IsActive = true },
            new VehicleType { Id = 4, Name = "Kamyon", Category = "AĞIR VASITA", IsActive = true },
            new VehicleType { Id = 5, Name = "Çekici", Category = "AĞIR VASITA", IsActive = true },
            new VehicleType { Id = 6, Name = "Dorse", Category = "AĞIR VASITA", IsActive = true }
        );
    }
}