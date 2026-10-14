using Microsoft.EntityFrameworkCore;
using TrafikCezaYonetim.Business.DTOs;
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

    public async Task<List<VehicleType>> GetAllVehicleTypesAsync()
    {
        return await _context.VehicleTypes
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task AddVehicleAsync(VehicleCreateDto dto)
    {
        var vehicle = new Vehicle
        {
            Name = dto.Name,
            VehicleTypeId = dto.VehicleTypeId,
            PlateNumber = dto.PlateNumber.Trim().ToUpper(),
            Brand = dto.Brand ?? string.Empty,
            Model = dto.Model ?? string.Empty,
            UsageType = dto.UsageType ?? "KM Bazlı (Araçlar)",
            Ownership = dto.Ownership ?? "Kendi",
            SerialNumber = dto.SerialNumber,
            FuelType = dto.FuelType,
            
        };

        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }
}