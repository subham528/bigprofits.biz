using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class TokenGrowthRate
    {
        public int Id { get; set; }
        public int? YearNo { get; set; }
        public decimal? Price { get; set; }
        public int? YearName { get; set; }
    }
}
