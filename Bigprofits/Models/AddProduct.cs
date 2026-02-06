using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bigprofits.Models
{
    public partial class AddProduct
    {
        public int Id { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? Price { get; set; }
        public decimal? Gst { get; set; }
        public decimal? PriceWithGst { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? Weight { get; set; }
        public string? ProductImage { get; set; }
        public decimal? ProductBv { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? PacketType { get; set; }
        public decimal? Dpprise { get; set; }
        public decimal? Field1 { get; set; }
        public string? Field2 { get; set; }
        public string? Field3 { get; set; }
        public int? CatId { get; set; }
        public int? SubCatId { get; set; }
        [NotMapped]
        public IFormFile? Product_Image { get; set; }
    }
}
