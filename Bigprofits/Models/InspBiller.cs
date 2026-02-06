using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class InspBiller
    {
        public int Id { get; set; }
        public string? BillerId { get; set; }
        public string? BillerName { get; set; }
        public string? CategoryKey { get; set; }
        public string? CategoryName { get; set; }
        public int? IsAvailable { get; set; }
        public string? IconUrl { get; set; }
    }
}
