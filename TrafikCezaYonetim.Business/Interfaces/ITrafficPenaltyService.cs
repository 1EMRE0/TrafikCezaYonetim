using System;
using System.Collections.Generic;
using System.Text;
using TrafikCezaYonetim.Business.DTOs;

namespace TrafikCezaYonetim.Business.Interfaces
{
    public interface ITrafficPenaltyService
    {
        Task<List<TrafficPenaltyDetailDto>> GetAllPenaltiesAsync();
        Task<TrafficPenaltyDetailDto?> GetPenaltyByIdAsync(int id);
        Task<int> CreatePenaltyAsync(TrafficPenaltyCreateDto dto);
        Task ApprovePenaltyAsync(PenaltyApprovalActionDto dto);
        Task RejectPenaltyAsync(PenaltyApprovalActionDto dto);
        Task<List<PenaltyApprovalHistoryDto>> GetApprovalHistoriesAsync(int penaltyId);
    }
}
