using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class TableCart
    {
        public int Id { get; set; }
        public string? Memberid { get; set; }
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public string? UserType { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
