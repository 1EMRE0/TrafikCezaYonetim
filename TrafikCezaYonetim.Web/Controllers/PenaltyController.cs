using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafikCezaYonetim.Business.DTOs;
using TrafikCezaYonetim.Business.Interfaces;
using TrafikCezaYonetim.Core.Enums;

namespace TrafikCezaYonetim.Web.Controllers;

[Authorize]
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
    [Authorize(Roles = "Personel,Yonetici")]
    public async Task<IActionResult> Create(TrafficPenaltyCreateDto dto)
    {
        if (dto.TotalAmount <= 0)
        {
            TempData["Error"] = "Ceza tutarı sıfırdan büyük olmalıdır.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // Giriş yapan aktif kullanıcının tam adını Claim'den alıyoruz
            var userFullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Personel";
            dto.CreatedBy = userFullName;

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
    [Authorize(Roles = "Yonetici,Finans")]
    public async Task<IActionResult> Approve(int id, string? comment)
    {
        try
        {
            var penalty = await _penaltyService.GetPenaltyByIdAsync(id);
            if (penalty == null)
            {
                TempData["Error"] = "Ceza kaydı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Statü ve rol yetki eşleşmesi kontrolü
            if (penalty.Status == PenaltyStatus.YoneticiOnayinda && !User.IsInRole("Yonetici"))
            {
                TempData["Error"] = "Bu cezayı onaylamak için Yönetici yetkisine sahip olmalısınız.";
                return RedirectToAction(nameof(Index));
            }

            if (penalty.Status == PenaltyStatus.FinansOnayinda && !User.IsInRole("Finans"))
            {
                TempData["Error"] = "Bu cezayı onaylamak için Finans yetkisine sahip olmalısınız.";
                return RedirectToAction(nameof(Index));
            }

            var userFullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Yetkili";

            var dto = new PenaltyApprovalActionDto
            {
                PenaltyId = id,
                ActionBy = userFullName,
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
    [Authorize(Roles = "Yonetici,Finans")]
    public async Task<IActionResult> Reject(int id, string rejectionReason)
    {
        try
        {
            var penalty = await _penaltyService.GetPenaltyByIdAsync(id);
            if (penalty == null)
            {
                TempData["Error"] = "Ceza kaydı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (penalty.Status == PenaltyStatus.YoneticiOnayinda && !User.IsInRole("Yonetici"))
            {
                TempData["Error"] = "Bu cezayı reddetmek için Yönetici yetkisine sahip olmalısınız.";
                return RedirectToAction(nameof(Index));
            }

            if (penalty.Status == PenaltyStatus.FinansOnayinda && !User.IsInRole("Finans"))
            {
                TempData["Error"] = "Bu cezayı reddetmek için Finans yetkisine sahip olmalısınız.";
                return RedirectToAction(nameof(Index));
            }

            var userFullName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name ?? "Yetkili";

            var dto = new PenaltyApprovalActionDto
            {
                PenaltyId = id,
                ActionBy = userFullName,
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