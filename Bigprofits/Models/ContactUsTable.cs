using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ContactUsTable
    {
        public long Sono { get; set; }
        public string? MemName { get; set; }
        public string? Mobile { get; set; }
        public string? EmailId { get; set; }
        public string? City { get; set; }
        public string? Subject { get; set; }
        public string? Smg { get; set; }
        public DateTime? ContactDate { get; set; }
        public int? RStatus { get; set; }
    }
}
