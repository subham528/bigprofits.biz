using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ListMatrixLvl
    {
        public int Id { get; set; }
        public int? MatrixNo { get; set; }
        public int? Lvl { get; set; }
        public decimal? Amount { get; set; }
        public int? Downline { get; set; }
    }
}
