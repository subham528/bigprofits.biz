using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ProductSubCat
    {
        public int Id { get; set; }
        public string? SubCatName { get; set; }
        public int? CatId { get; set; }
        public int? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
