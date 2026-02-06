using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ProductCat
    {
        public int Id { get; set; }
        public string? CatName { get; set; }
        public int? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
