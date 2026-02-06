using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class PinDetail
    {
        public int Sono { get; set; }
        public string? PinType { get; set; }
        public decimal? Bvpoint { get; set; }
        public decimal? PairValue { get; set; }
        public string? PinDetail1 { get; set; }
        public decimal? PinAmout { get; set; }
        public decimal? ReturnAmount { get; set; }
        public int? ReturnMonth { get; set; }
        public int? IsActive { get; set; }
        public decimal? PackagePercent { get; set; }
        public string? Bv { get; set; }
    }
}
