using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class BuyToken
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TokenAmount { get; set; }
        public string? TokenName { get; set; }
        public decimal? CRate { get; set; }
        public DateTime? RDate { get; set; }
        public int? AStatus { get; set; }
        public DateTime? ADate { get; set; }
        public string? HashId { get; set; }
        public int? Nonce { get; set; }
    }
}
