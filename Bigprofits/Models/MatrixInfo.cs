using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MatrixInfo
    {
        public int Id { get; set; }
        public int? MatrixNo { get; set; }
        public string? MatrixName { get; set; }
        public int? Matrix { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Total { get; set; }
        public decimal? Upgrade { get; set; }
        public decimal? LvlAmount { get; set; }
        public decimal? Income { get; set; }
        public string? Etype { get; set; }
    }
}
