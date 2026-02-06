using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MemberDirect
    {
        public int ChkSno { get; set; }
        public string MemberId { get; set; } = null!;
        public int? ChkL { get; set; }
        public int? ChkR { get; set; }
    }
}
