using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AccountVirtual
    {
        public int VrtlSno { get; set; }
        public string? MemberId { get; set; }
        public decimal? RoiAmount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? Admincharge { get; set; }
        public decimal? FinalAmount { get; set; }
        public DateTime? Roidate { get; set; }
        public decimal? Mempackage { get; set; }
        public string? Cmtid { get; set; }
        public int? Booster { get; set; }
        public decimal? Rpcharge { get; set; }
        public int? Sroi { get; set; }
        public string? Bymemberid { get; set; }
        public decimal? MRoiAmount { get; set; }
        public int? Lvl { get; set; }
    }
}
