using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AccountLevel
    {
        public int Id { get; set; }
        public string? Memberid { get; set; }
        public string? Bymemberid { get; set; }
        public decimal? Ammount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCharge { get; set; }
        public DateTime? Ddate { get; set; }
        public decimal? FinalAmount { get; set; }
        public decimal? Mypackage { get; set; }
        public decimal? Rpcharge { get; set; }
        public decimal? Package { get; set; }
        public int? Crntslab { get; set; }
        public int? LvlNo { get; set; }
        public string? HasId { get; set; }
        public string? CmitId { get; set; }
    }
}
