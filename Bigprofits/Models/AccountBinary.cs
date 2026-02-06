using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AccountBinary
    {
        public int BnrySno { get; set; }
        public string? MemberId { get; set; }
        public decimal? BnryMatch { get; set; }
        public int? BnryCfleft { get; set; }
        public int? BnryCfright { get; set; }
        public decimal? BnryAmount { get; set; }
        public DateTime? BnryCdate { get; set; }
        public decimal? Tds { get; set; }
        public decimal? Admincharge { get; set; }
        public decimal? Rpcharge { get; set; }
        public decimal? Finalamount { get; set; }
        public decimal? LeftBv { get; set; }
        public decimal? RightBv { get; set; }
        public int? Bcount { get; set; }
        public string? CmitId { get; set; }
    }
}
