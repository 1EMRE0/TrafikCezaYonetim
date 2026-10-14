using System;
using System.Collections.Generic;
using System.Text;
using TrafikCezaYonetim.Core.Enums;

namespace TrafikCezaYonetim.Business.DTOs
{
    public class TrafficPenaltyDetailDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string? PenaltyNumber { get; set; }
        public string? DriverName { get; set; }
        public DateTime PenaltyDate { get; set; }
        public DateTime? NoticeDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public PenaltyStatus Status { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<PenaltyApprovalHistoryDto> ApprovalHistories { get; set; } = new();
    }
}
