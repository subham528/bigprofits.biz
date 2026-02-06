using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ClientSystemInfo
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? ClientIp { get; set; }
        public int? Error { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? Rdate { get; set; }
        public string? AType { get; set; }
    }
}
