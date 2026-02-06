using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string? Memberid { get; set; }
        public string? OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? Gst { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? AmountWithGst { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? UserType { get; set; }
        public decimal? ProductBv { get; set; }
        public string? ProductName { get; set; }
    }
}
