using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MemberWidInfoBackup
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public decimal? DirectIncome { get; set; }
        public decimal? Roiincome { get; set; }
        public decimal? BinaryIncome { get; set; }
        public decimal? Totalamount { get; set; }
        public decimal? Tds { get; set; }
        public decimal? AdminCharge { get; set; }
        public DateTime? Wdldate { get; set; }
        public string? Status { get; set; }
        public decimal? Withdrawal { get; set; }
        public string? Details { get; set; }
        public DateTime? AdminApprovedDate { get; set; }
        public decimal? AmountBeforeTds { get; set; }
        public string? PaymentMode { get; set; }
        public string? HashId { get; set; }
        public decimal? FinalAmount { get; set; }
        public string? TransactionNo { get; set; }
        public string? OrderId { get; set; }
        public string? Wtype { get; set; }
    }
}
