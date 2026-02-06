using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class StakeInfo
    {
        public int Id { get; set; }
        public string? PlanDetail { get; set; }
        public string? PlanName { get; set; }
        public int? Duration { get; set; }
        public decimal? Rate { get; set; }
    }
}
