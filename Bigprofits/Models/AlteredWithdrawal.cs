using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AlteredWithdrawal
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public decimal? WithdrawalUsdt { get; set; }
        public string? Atype { get; set; }
        public DateTime? Adate { get; set; }
        public int? WidId { get; set; }
    }
}
