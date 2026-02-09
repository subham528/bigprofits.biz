using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class RewardInfo
    {
        public int Id { get; set; }
        public int? RankNo { get; set; }
        public string? RankName { get; set; }
        public decimal? LeftBiz { get; set; }
        public decimal? RightBiz { get; set; }
        public int? DirectReq { get; set; }
        public decimal? Amount { get; set; }
    }
}
