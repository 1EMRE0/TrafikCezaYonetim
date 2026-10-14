using System;
using System.Collections.Generic;
using System.Text;
using TrafikCezaYonetim.Core.Enums;

namespace TrafikCezaYonetim.Core.Entities
{
    public class PenaltyApprovalHistory
    {
        public int Id { get; set; }
        public int TrafficPenaltyId { get; set; }
        public TrafficPenalty TrafficPenalty { get; set; } = null!;
        public string ActionBy { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public ActionType ActionType { get; set; }
        public PenaltyStatus OldStatus { get; set; }
        public PenaltyStatus NewStatus { get; set; }
        public string? Comment { get; set; }
    }
}
