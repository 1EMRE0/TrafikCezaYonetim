using Microsoft.EntityFrameworkCore;
using TrafikCezaYonetim.Business.DTOs;
using TrafikCezaYonetim.Business.Interfaces;
using TrafikCezaYonetim.Core.Entities;
using TrafikCezaYonetim.Core.Enums;
using TrafikCezaYonetim.DataAccess.Context;

namespace TrafikCezaYonetim.Business.Services;

public class TrafficPenaltyService : ITrafficPenaltyService
{
    private readonly AppDbContext _context;

    public TrafficPenaltyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrafficPenaltyDetailDto>> GetAllPenaltiesAsync()
    {
        return await _context.TrafficPenalties
            .Include(p => p.Vehicle)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new TrafficPenaltyDetailDto
            {
                Id = p.Id,
                VehicleId = p.VehicleId,
                PlateNumber = p.Vehicle.PlateNumber,
                VehicleName = p.Vehicle.Name,
                PenaltyNumber = p.PenaltyNumber,
                DriverName = p.DriverName,
                PenaltyDate = p.PenaltyDate,
                NoticeDate = p.NoticeDate,
                TotalAmount = p.TotalAmount,
                DiscountAmount = p.DiscountAmount,
                PayableAmount = p.PayableAmount,
                Location = p.Location,
                Description = p.Description,
                Notes = p.Notes,
                Status = p.Status,
                RejectionReason = p.RejectionReason,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<TrafficPenaltyDetailDto?> GetPenaltyByIdAsync(int id)
    {
        var p = await _context.TrafficPenalties
            .Include(x => x.Vehicle)
            .Include(x => x.ApprovalHistories)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null) return null;

        return new TrafficPenaltyDetailDto
        {
            Id = p.Id,
            VehicleId = p.VehicleId,
            PlateNumber = p.Vehicle.PlateNumber,
            VehicleName = p.Vehicle.Name,
            PenaltyNumber = p.PenaltyNumber,
            DriverName = p.DriverName,
            PenaltyDate = p.PenaltyDate,
            NoticeDate = p.NoticeDate,
            TotalAmount = p.TotalAmount,
            DiscountAmount = p.DiscountAmount,
            PayableAmount = p.PayableAmount,
            Location = p.Location,
            Description = p.Description,
            Notes = p.Notes,
            Status = p.Status,
            RejectionReason = p.RejectionReason,
            CreatedAt = p.CreatedAt,
            ApprovalHistories = p.ApprovalHistories
                .OrderBy(h => h.ActionDate)
                .Select(h => new PenaltyApprovalHistoryDto
                {
                    Id = h.Id,
                    ActionBy = h.ActionBy,
                    ActionDate = h.ActionDate,
                    ActionType = h.ActionType,
                    OldStatus = h.OldStatus,
                    NewStatus = h.NewStatus,
                    Comment = h.Comment
                }).ToList()
        };
    }

    public async Task<int> CreatePenaltyAsync(TrafficPenaltyCreateDto dto)
    {
        if (dto.TotalAmount <= 0)
            throw new ArgumentException("Ceza tutarı sıfırdan büyük olmalıdır.");

        if (dto.PenaltyDate > DateTime.Now)
            throw new ArgumentException("Ceza tarihi gelecekteki bir tarih olamaz.");

        var payableAmount = dto.TotalAmount - dto.DiscountAmount;
        if (payableAmount < 0) payableAmount = 0;

        var penalty = new TrafficPenalty
        {
            VehicleId = dto.VehicleId,
            PenaltyNumber = dto.PenaltyNumber,
            DriverName = dto.DriverName,
            PenaltyDate = dto.PenaltyDate,
            NoticeDate = dto.NoticeDate,
            TotalAmount = dto.TotalAmount,
            DiscountAmount = dto.DiscountAmount,
            PayableAmount = payableAmount,
            Location = dto.Location,
            Description = dto.Description,
            Notes = dto.Notes,
            Status = PenaltyStatus.YoneticiOnayinda,
            CreatedAt = DateTime.Now
        };

        _context.TrafficPenalties.Add(penalty);
        await _context.SaveChangesAsync();

        var history = new PenaltyApprovalHistory
        {
            TrafficPenaltyId = penalty.Id,
            ActionBy = string.IsNullOrWhiteSpace(dto.CreatedBy) ? "Sistem / Personel" : dto.CreatedBy,
            ActionDate = DateTime.Now,
            ActionType = ActionType.Olusturuldu,
            OldStatus = PenaltyStatus.Yeni,
            NewStatus = PenaltyStatus.YoneticiOnayinda,
            Comment = "Sisteme yeni ceza kaydı girildi ve yönetici onayına sunuldu."
        };

        _context.PenaltyApprovalHistories.Add(history);
        await _context.SaveChangesAsync();

        return penalty.Id;
    }

    public async Task ApprovePenaltyAsync(PenaltyApprovalActionDto dto)
    {
        var penalty = await _context.TrafficPenalties.FirstOrDefaultAsync(p => p.Id == dto.PenaltyId);
        if (penalty == null)
            throw new InvalidOperationException("Ceza kaydı bulunamadı.");

        if (penalty.Status == PenaltyStatus.Tamamlandi)
            throw new InvalidOperationException("Tamamlanmış bir ceza üzerinde değişiklik yapılamaz.");

        if (penalty.Status == PenaltyStatus.Reddedildi)
            throw new InvalidOperationException("Reddedilmiş bir ceza doğrudan onaylanamaz.");

        var oldStatus = penalty.Status;
        PenaltyStatus newStatus;

        if (oldStatus == PenaltyStatus.YoneticiOnayinda)
        {
            newStatus = PenaltyStatus.FinansOnayinda;
        }
        else if (oldStatus == PenaltyStatus.FinansOnayinda)
        {
            newStatus = PenaltyStatus.Tamamlandi;
        }
        else
        {
            throw new InvalidOperationException("Bu statüdeki bir kayıt için onay adımı bulunmamaktadır.");
        }

        penalty.Status = newStatus;

        var history = new PenaltyApprovalHistory
        {
            TrafficPenaltyId = penalty.Id,
            ActionBy = dto.ActionBy,
            ActionDate = DateTime.Now,
            ActionType = ActionType.Onaylandi,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Comment = dto.Comment
        };

        _context.PenaltyApprovalHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task RejectPenaltyAsync(PenaltyApprovalActionDto dto)
    {
        var penalty = await _context.TrafficPenalties.FirstOrDefaultAsync(p => p.Id == dto.PenaltyId);
        if (penalty == null)
            throw new InvalidOperationException("Ceza kaydı bulunamadı.");

        if (penalty.Status == PenaltyStatus.Tamamlandi)
            throw new InvalidOperationException("Tamamlanmış bir ceza reddedilemez.");

        if (string.IsNullOrWhiteSpace(dto.RejectionReason) || dto.RejectionReason.Trim().Length < 10)
            throw new ArgumentException("Ret gerekçesi zorunludur ve en az 10 karakter olmalıdır.");

        var oldStatus = penalty.Status;
        var newStatus = PenaltyStatus.Reddedildi;

        penalty.Status = newStatus;
        penalty.RejectionReason = dto.RejectionReason;

        var history = new PenaltyApprovalHistory
        {
            TrafficPenaltyId = penalty.Id,
            ActionBy = dto.ActionBy,
            ActionDate = DateTime.Now,
            ActionType = ActionType.Reddedildi,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Comment = dto.RejectionReason
        };

        _context.PenaltyApprovalHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PenaltyApprovalHistoryDto>> GetApprovalHistoriesAsync(int penaltyId)
    {
        return await _context.PenaltyApprovalHistories
            .Where(h => h.TrafficPenaltyId == penaltyId)
            .OrderBy(h => h.ActionDate)
            .Select(h => new PenaltyApprovalHistoryDto
            {
                Id = h.Id,
                ActionBy = h.ActionBy,
                ActionDate = h.ActionDate,
                ActionType = h.ActionType,
                OldStatus = h.OldStatus,
                NewStatus = h.NewStatus,
                Comment = h.Comment
            })
            .ToListAsync();
    }
}