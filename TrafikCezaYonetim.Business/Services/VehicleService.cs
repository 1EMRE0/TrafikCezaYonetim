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
            FuelType = dto.FuelType
        };

        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVehicleAsync(VehicleCreateDto dto)
    {
        var vehicle = await _context.Vehicles.FindAsync(dto.Id);
        if (vehicle == null)
            throw new Exception("Araç bulunamadı.");

        vehicle.Name = dto.Name;
        vehicle.VehicleTypeId = dto.VehicleTypeId;
        vehicle.PlateNumber = dto.PlateNumber.Trim().ToUpper();
        vehicle.Brand = dto.Brand ?? string.Empty;
        vehicle.Model = dto.Model ?? string.Empty;
        vehicle.UsageType = dto.UsageType ?? "KM Bazlı (Araçlar)";
        vehicle.Ownership = dto.Ownership ?? "Kendi";
        vehicle.SerialNumber = dto.SerialNumber;
        vehicle.FuelType = dto.FuelType;

        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }
}