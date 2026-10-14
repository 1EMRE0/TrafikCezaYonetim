using System;
using System.Collections.Generic;
using System.Text;

namespace TrafikCezaYonetim.Core.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; } = null!;
        public string? UsageType { get; set; }
        public string? Ownership { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string? SerialNumber { get; set; }
        public int? ModelYear { get; set; }
        public string? FuelType { get; set; }

        public ICollection<TrafficPenalty> TrafficPenalties { get; set; } = new List<TrafficPenalty>();
    }
}
