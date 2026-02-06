using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MemPageOlog
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? PageName { get; set; }
        public DateTime? ODate { get; set; }
    }
}
