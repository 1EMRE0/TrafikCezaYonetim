using Microsoft.AspNetCore.Mvc;
using TrafikCezaYonetim.Business.DTOs;
using TrafikCezaYonetim.Business.Interfaces;

namespace TrafikCezaYonetim.Web.Controllers;

public class PenaltyController : Controller
{
    private readonly ITrafficPenaltyService _penaltyService;
    private readonly IVehicleService _vehicleService;

    public PenaltyController(ITrafficPenaltyService penaltyService, IVehicleService vehicleService)
    {
        _penaltyService = penaltyService;
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var penalties = await _penaltyService.GetAllPenaltiesAsync();
        ViewBag.Vehicles = await _vehicleService.GetAllVehiclesAsync();
        return View(penalties);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrafficPenaltyCreateDto dto)
    {
        if (dto.TotalAmount <= 0)
        {
            TempData["Error"] = "Ceza tutarı sıfırdan büyük olmalıdır.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            dto.CreatedBy = "Emre Ovacık (Personel)";
            await _penaltyService.CreatePenaltyAsync(dto);
            TempData["Success"] = "Yeni ceza kaydı başarıyla oluşturuldu ve onaya gönderildi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, string comment)
    {
        try
        {
            var dto = new PenaltyApprovalActionDto
            {
                PenaltyId = id,
                ActionBy = "Yönetici / Finans Yetkilisi",
                Comment = string.IsNullOrWhiteSpace(comment) ? "Sistem üzerinden onay verildi." : comment
            };

            await _penaltyService.ApprovePenaltyAsync(dto);
            TempData["Success"] = "Ceza kaydı bir sonraki aşamaya onaylandı.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id, string rejectionReason)
    {
        try
        {
            var dto = new PenaltyApprovalActionDto
            {
                PenaltyId = id,
                ActionBy = "Yönetici / Finans Yetkilisi",
                RejectionReason = rejectionReason
            };

            await _penaltyService.RejectPenaltyAsync(dto);
            TempData["Success"] = "Ceza kaydı reddedildi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetDetail(int id)
    {
        var detail = await _penaltyService.GetPenaltyByIdAsync(id);
        if (detail == null) return NotFound();
        return Json(detail);
    }
}