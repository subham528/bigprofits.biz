using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AdminAction
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public int? Astatus { get; set; }
        public DateTime? Adate { get; set; }
        public string? Atype { get; set; }
        public string? HashId { get; set; }
    }
}
