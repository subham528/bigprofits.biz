using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class TableSupport
    {
        public int Srno { get; set; }
        public string? MySubject { get; set; }
        public string? MyDetails { get; set; }
        public string? FromBy { get; set; }
        public string? ToBy { get; set; }
        public DateTime? Sdate { get; set; }
        public int? MyStatus { get; set; }
        public string? Reply { get; set; }
        public string? Img { get; set; }
    }
}
