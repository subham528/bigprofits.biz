using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AchiversIncome
    {
        public int Id { get; set; }
        public string? Memberid { get; set; }
        public int? AchieversId { get; set; }
        public string? Achivers { get; set; }
        public int? Directmem { get; set; }
        public decimal? Turnoverper { get; set; }
        public DateTime? AchivDate { get; set; }
    }
}
