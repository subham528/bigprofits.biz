using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class Achiever
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public int? Astatus { get; set; }
        public DateTime? Adate { get; set; }
        public int? Direct { get; set; }
        public int? SLage { get; set; }
        public int? OLage { get; set; }
        public string? Atype { get; set; }
        public int? Icount { get; set; }
        public int? TillCount { get; set; }
        public decimal? Amount { get; set; }
        public int? Lvl { get; set; }
    }
}
