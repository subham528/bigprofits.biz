using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class UnilevelPurchase
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public int? Ustatus { get; set; }
        public DateTime? Udate { get; set; }
        public decimal? TokenAmount { get; set; }
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
        public string? HashId { get; set; }
        public int? WillCount { get; set; }
        public string? Etype { get; set; }
        public int? Directs { get; set; }
        public string? Cmitid { get; set; }
        public DateTime? Adate { get; set; }
    }
}
