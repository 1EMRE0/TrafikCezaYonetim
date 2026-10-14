using TrafikCezaYonetim.Business.DTOs;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.Business.Interfaces;

public interface IVehicleService
{
    Task<List<Vehicle>> GetAllVehiclesAsync();
    Task<Vehicle?> GetVehicleByIdAsync(int id);
    Task<List<VehicleType>> GetAllVehicleTypesAsync();
    Task AddVehicleAsync(VehicleCreateDto dto);
    Task UpdateVehicleAsync(VehicleCreateDto dto);
}