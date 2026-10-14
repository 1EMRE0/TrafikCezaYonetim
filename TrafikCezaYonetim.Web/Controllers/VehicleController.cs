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
}