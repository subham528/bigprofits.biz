using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class TblNews
    {
        public int NewsId { get; set; }
        public string? Name { get; set; }
        public string? News { get; set; }
        public DateTime? NewsDate { get; set; }
        public string? Website { get; set; }
    }
}
