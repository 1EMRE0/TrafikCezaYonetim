using System;
using System.Collections.Generic;
using System.Text;
using TrafikCezaYonetim.Core.Enums;

namespace TrafikCezaYonetim.Business.DTOs
{
    public class PenaltyApprovalHistoryDto
    {
        public int Id { get; set; }
        public string ActionBy { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public ActionType ActionType { get; set; }
        public PenaltyStatus OldStatus { get; set; }
        public PenaltyStatus NewStatus { get; set; }
        public string? Comment { get; set; }
    }
}
