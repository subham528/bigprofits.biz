using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class FundAmount
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public decimal? Amount { get; set; }
        public int? Status { get; set; }
        public DateTime? Fdate { get; set; }
        public string? ByMemberId { get; set; }
        public string? RefId { get; set; }
        public string? Details { get; set; }
        public string? Ftype { get; set; }
        public string? HashId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string? WType { get; set; }
        public decimal? AdminCharge { get; set; }
        public decimal? FinalAmount { get; set; }
        public string? TokenAmount { get; set; }
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
    }
}
