using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class SmsAdmin
    {
        public int MsgId { get; set; }
        public string? ContectNo { get; set; }
        public DateTime? Date { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }
}
