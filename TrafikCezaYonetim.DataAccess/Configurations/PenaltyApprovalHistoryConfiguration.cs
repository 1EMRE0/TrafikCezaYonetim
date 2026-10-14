using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.DataAccess.Configurations;

public class PenaltyApprovalHistoryConfiguration : IEntityTypeConfiguration<PenaltyApprovalHistory>
{
    public void Configure(EntityTypeBuilder<PenaltyApprovalHistory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActionBy).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Comment).HasMaxLength(500);
        builder.Property(x => x.ActionDate).HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.TrafficPenalty)
               .WithMany(x => x.ApprovalHistories)
               .HasForeignKey(x => x.TrafficPenaltyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}