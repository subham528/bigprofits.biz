using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ContryCode
    {
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? InternationalDialing { get; set; }
        public string? ConCode { get; set; }
    }
}
