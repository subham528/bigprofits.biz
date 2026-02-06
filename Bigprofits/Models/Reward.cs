using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class Reward
    {
        public int Id { get; set; }
        public int? Rewardid { get; set; }
        public string? Memberid { get; set; }
        public decimal? RwrdIncome { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCrg { get; set; }
        public decimal? FinalIncome { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Adate { get; set; }
        public int? Pstatus { get; set; }
        public string? Rdescription { get; set; }
        public decimal? Rpcharge { get; set; }
        public string? Memrank { get; set; }
        public string? HashId { get; set; }
        public int? ICount { get; set; }
        public decimal? Retrunamount { get; set; }
    }
}
