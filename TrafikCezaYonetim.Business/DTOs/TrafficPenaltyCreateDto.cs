using System;
using System.Collections.Generic;
using System.Text;

namespace TrafikCezaYonetim.Business.DTOs
{
    public class TrafficPenaltyCreateDto
    {
        public int VehicleId { get; set; }
        public string? PenaltyNumber { get; set; }
        public string? DriverName { get; set; }
        public DateTime PenaltyDate { get; set; }
        public DateTime? NoticeDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
