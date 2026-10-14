using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafikCezaYonetim.Business.DTOs;
using TrafikCezaYonetim.Business.Interfaces;

namespace TrafikCezaYonetim.Web.Controllers;

[Authorize]
public class VehicleController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vehicles = await _vehicleService.GetAllVehiclesAsync();
        return View(vehicles);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.VehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehicleCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.VehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
            return View(dto);
        }

        try
        {
            await _vehicleService.AddVehicleAsync(dto);
            TempData["Success"] = "Yeni araç/makina başarıyla sisteme kaydedildi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Araç eklenirken hata oluştu: " + ex.Message;
            ViewBag.VehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
        if (vehicle == null) return NotFound();

        ViewBag.VehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();

        var dto = new VehicleCreateDto
        {
            Id = vehicle.Id,
            Name = vehicle.Name,
            VehicleTypeId = vehicle.VehicleTypeId,
            PlateNumber = vehicle.PlateNumber,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            UsageType = vehicle.UsageType,
            Ownership = vehicle.Ownership,
            SerialNumber = vehicle.SerialNumber,
            FuelType = vehicle.FuelType
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VehicleCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.VehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
            return View(dto);
        }

        try
        {
            await _vehicleService.UpdateVehicleAsync(dto);
            TempData["Success"] = "Araç bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Güncelleme sırasında hata oluştu: " + ex.Message;
            ViewBag.VehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
            return View(dto);
        }
    }
}