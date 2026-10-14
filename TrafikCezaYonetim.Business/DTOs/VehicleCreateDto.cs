using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TrafikCezaYonetim.Business.DTOs;
public class VehicleCreateDto
{
    [Required(ErrorMessage = "Araç/Makina adı zorunludur.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Araç tipi seçilmelidir.")]
    public int VehicleTypeId { get; set; }

    public string? UsageType { get; set; } 
    public string? Ownership { get; set; } 
    public string? Brand { get; set; }
    public string? Model { get; set; }

    [Required(ErrorMessage = "Plaka zorunludur.")]
    public string PlateNumber { get; set; } = string.Empty;

    public string? SerialNumber { get; set; }
    public int? ModelYear { get; set; }
    public string? FuelType { get; set; }
    public decimal? HourlyDailyRate { get; set; }

    // Sigorta & Muayene
    public string? InsuranceCompany { get; set; }
    public string? PolicyNumber { get; set; }
    public DateTime? InsuranceStartDate { get; set; }
    public DateTime? InsuranceEndDate { get; set; }
    public DateTime? LastInspectionDate { get; set; }
    public DateTime? NextInspectionDate { get; set; }

    
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? Notes { get; set; }
    public string? AcquisitionType { get; set; } = "Satın Alma";
    public string? CascoCompany { get; set; }
    public string? CascoPolicyNumber { get; set; }
    public decimal? CascoPremium { get; set; }
    public decimal? TrafficInsurancePremium { get; set; }
}
