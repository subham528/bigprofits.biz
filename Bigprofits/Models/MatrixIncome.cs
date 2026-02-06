using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MatrixIncome
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? ByMemberId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCharge { get; set; }
        public decimal? FinalAmount { get; set; }
        public int? Mststus { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MatrixNo { get; set; }
        public int? Position { get; set; }
        public string? MatrixType { get; set; }
        public string? IncomeTo { get; set; }
    }
}
