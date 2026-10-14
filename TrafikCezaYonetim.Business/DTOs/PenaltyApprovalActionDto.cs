using System;
using System.Collections.Generic;
using System.Text;

namespace TrafikCezaYonetim.Business.DTOs
{
    public class PenaltyApprovalActionDto
    {
        public int PenaltyId { get; set; }
        public string ActionBy { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public string? RejectionReason { get; set; }
    }
}
