using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ProductRequest
    {
        public int Id { get; set; }
        public string? Memberid { get; set; }
        public string? OrderId { get; set; }
        public int? TotalProducts { get; set; }
        public int? Items { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalGst { get; set; }
        public decimal? FinalAmount { get; set; }
        public int? OrderStatus { get; set; }
        public string? OrderNotes { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? UserType { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public decimal? ProductBv { get; set; }
    }
}
