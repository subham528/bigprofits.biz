using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AchieversIncome
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCharge { get; set; }
        public decimal? FinalAmount { get; set; }
        public int? Astatus { get; set; }
        public int? Lvl { get; set; }
        public DateTime? Adate { get; set; }
        public string? Atype { get; set; }
        public string? CmitId { get; set; }
    }
}
