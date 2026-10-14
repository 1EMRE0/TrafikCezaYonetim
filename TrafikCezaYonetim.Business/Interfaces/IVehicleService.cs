using System;
using System.Collections.Generic;
using System.Text;
using TrafikCezaYonetim.Core.Entities;

namespace TrafikCezaYonetim.Business.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(int id);
    }
}
