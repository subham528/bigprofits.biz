using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class Achivement
    {
        public int AchvSno { get; set; }
        public string MemberId { get; set; } = null!;
        public int? Dcount { get; set; }
        public int? Tlcount { get; set; }
        public int? Trcount { get; set; }
        public int? Plcount { get; set; }
        public int? Prcount { get; set; }
        public int? Lpoints { get; set; }
        public int? Rpoints { get; set; }
        public int? Ppoints { get; set; }
        public decimal? Lcmit { get; set; }
        public decimal? Rcmit { get; set; }
        public decimal? Lcnfrm { get; set; }
        public decimal? Rcnfrm { get; set; }
        public int? Pcmit { get; set; }
        public decimal? Lvl1 { get; set; }
        public decimal? Lvl2 { get; set; }
        public decimal? Lvl3 { get; set; }
        public string? WalAddress { get; set; }
    }
}
