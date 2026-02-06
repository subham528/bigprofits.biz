using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class OnlineService
    {
        public int Memid { get; set; }
        public string? Status { get; set; }
        public string? OurTxnNo { get; set; }
        public string? Operatorcode { get; set; }
        public string? Account { get; set; }
        public decimal? Amount { get; set; }
        public string? OperatorRefNo { get; set; }
        public string? UserTxnNo { get; set; }
        public decimal? ClosingBalance { get; set; }
        public string? Messagess { get; set; }
        public string? OprtnTime { get; set; }
        public string? Memberid { get; set; }
        public string? ServiceType { get; set; }
        public int? IsPaid { get; set; }
        public decimal? UsdAmount { get; set; }
        public decimal? ApiCharge { get; set; }
        public DateTime? Odate { get; set; }
    }
}
