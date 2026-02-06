using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AccountSummary
    {
        public int SmrySno { get; set; }
        public string? MemberId { get; set; }
        public decimal? SmryRetBnry { get; set; }
        public decimal? SmryRetLvl { get; set; }
        public decimal? SmryRetClub { get; set; }
        public decimal? NewIncome { get; set; }
        public decimal? PrimaryFund { get; set; }
    }
}
