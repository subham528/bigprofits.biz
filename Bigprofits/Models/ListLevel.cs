using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ListLevel
    {
        public int Id { get; set; }
        public int? Lvl { get; set; }
        public decimal? Lrate { get; set; }
        public decimal? Amount { get; set; }
        public int? DirectReq { get; set; }
        public decimal? ExtAmount { get; set; }
    }
}
