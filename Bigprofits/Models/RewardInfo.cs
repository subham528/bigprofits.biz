using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class RewardInfo
    {
        public int Id { get; set; }
        public int? Direct { get; set; }
        public decimal? SelfPackage { get; set; }
        public int? TeamSize { get; set; }
        public decimal? TeamVolume { get; set; }
        public decimal? Reward { get; set; }
        public decimal? Salary { get; set; }
        public string? Type { get; set; }
        public int? RId { get; set; }
    }
}
