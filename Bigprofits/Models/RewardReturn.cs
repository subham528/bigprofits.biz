using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class RewardReturn
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public int? RewardId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCharge { get; set; }
        public decimal? FinalAmount { get; set; }
        public DateTime? Rdate { get; set; }
        public string? RType { get; set; }
        public int? RId { get; set; }
    }
}
