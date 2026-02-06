using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class CmitRequest
    {
        public int CmitSno { get; set; }
        public string? MemberId { get; set; }
        public string? CmitId { get; set; }
        public decimal? CmitReqAmount { get; set; }
        public DateTime? CmitSdate { get; set; }
        public int? CmitStatus { get; set; }
        public string? PinNumber { get; set; }
        public decimal? PinAmount { get; set; }
        public DateTime? RoiDate { get; set; }
        public int? IsPinType { get; set; }
        public int? IsActive { get; set; }
        public string? Bymemberid { get; set; }
        public int? RoiStatus { get; set; }
        public string? PlanType { get; set; }
        public int? PlanNo { get; set; }
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
        public string? HashId { get; set; }
        public string? WalletType { get; set; }
        public decimal? RoiReturn { get; set; }
        public decimal? RoiTill { get; set; }
        public decimal? RoiRate { get; set; }
        public decimal? TopupWallet { get; set; }
        public decimal? UsdtWalelt { get; set; }
    }
}
