using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MemberNotification
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? Ntype { get; set; }
        public string? Nmessage { get; set; }
        public DateTime? Ndate { get; set; }
        public int? IsDeleted { get; set; }
        public string? Greeting { get; set; }
    }
}
