using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.DataAccess.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<TrafficPenalty> TrafficPenalties => Set<TrafficPenalty>();
    public DbSet<PenaltyApprovalHistory> PenaltyApprovalHistories => Set<PenaltyApprovalHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}