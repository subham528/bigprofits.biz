using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class UserMaster
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserPass { get; set; }
        public DateTime? LoginDate { get; set; }
        public string? Role { get; set; }
        public DateTime? DailyClosing { get; set; }
        public string? TokenAddress { get; set; }
        public string? TrxAddress { get; set; }
        public string? ConAddress { get; set; }
        public DateTime? WeeklyClosing { get; set; }
        public string? TrxAddressD { get; set; }
        public int? WidStatus { get; set; }
        public decimal? TokenPrice { get; set; }
    }
}
