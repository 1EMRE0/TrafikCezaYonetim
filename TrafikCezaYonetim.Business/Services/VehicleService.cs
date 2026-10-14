using Microsoft.EntityFrameworkCore;
using TrafikCezaYonetim.Business.Interfaces;
using TrafikCezaYonetim.Core.Entities;
using TrafikCezaYonetim.DataAccess.Context;

namespace TrafikCezaYonetim.Business.Services;

public class VehicleService : IVehicleService
{
    private readonly AppDbContext _context;

    public VehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllVehiclesAsync()
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .OrderBy(v => v.PlateNumber)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}