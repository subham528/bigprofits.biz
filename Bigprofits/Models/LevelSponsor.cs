using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class LevelSponsor
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? SpoId { get; set; }
        public int? Lvl { get; set; }
        public int? Sstatus { get; set; }
        public decimal? Amount { get; set; }
        public int? DirectRequired { get; set; }
    }
}
