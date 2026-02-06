using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class StakeAmount
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public int? StakeId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TokenAmount { get; set; }
        public int? Sstatus { get; set; }
        public decimal? LiveRate { get; set; }
        public int? Duration { get; set; }
        public DateTime? Sdate { get; set; }
        public DateTime? Mdate { get; set; }
        public decimal? Rate { get; set; }
        public decimal? ReturnAmount { get; set; }
    }
}
