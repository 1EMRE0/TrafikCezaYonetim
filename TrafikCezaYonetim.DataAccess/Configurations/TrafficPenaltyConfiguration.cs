using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.DataAccess.Configurations;

public class TrafficPenaltyConfiguration : IEntityTypeConfiguration<TrafficPenalty>
{
    public void Configure(EntityTypeBuilder<TrafficPenalty> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PenaltyNumber).HasMaxLength(50);
        builder.Property(x => x.DriverName).HasMaxLength(100);
        builder.Property(x => x.Location).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Notes).HasMaxLength(1000);
        builder.Property(x => x.RejectionReason).HasMaxLength(500);

        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(x => x.PayableAmount).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Vehicle)
               .WithMany(x => x.TrafficPenalties)
               .HasForeignKey(x => x.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}