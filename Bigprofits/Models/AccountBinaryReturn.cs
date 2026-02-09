using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AccountBinaryReturn
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public int? BnrySno { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCharge { get; set; }
        public decimal? FinalAmount { get; set; }
        public decimal? BnryMatch { get; set; }
        public decimal? Rate { get; set; }
        public int? Bstatus { get; set; }
        public DateTime? Bdate { get; set; }
    }
}
