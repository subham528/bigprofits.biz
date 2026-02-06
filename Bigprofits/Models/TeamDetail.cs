using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class TeamDetail
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? SpoId { get; set; }
        public string? RefPos { get; set; }
        public decimal? Amount { get; set; }
        public int? PinType { get; set; }
        public DateTime? Edate { get; set; }
        public int? Lvl { get; set; }
        public string? Itype { get; set; }
    }
}
