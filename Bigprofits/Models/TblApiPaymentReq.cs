using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class TblApiPaymentReq
    {
        public int Id { get; set; }
        public string? AdminId { get; set; }
        public string? AdminReqid { get; set; }
        public decimal? ReqAmount { get; set; }
        public int? ReqStatus { get; set; }
        public DateTime? ReqDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string? ProjName { get; set; }
        public string? Admindescription { get; set; }
    }
}
