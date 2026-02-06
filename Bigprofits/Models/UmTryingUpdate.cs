using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class UmTryingUpdate
    {
        public int Id { get; set; }
        public string? TrxAddress { get; set; }
        public string? TrxAddressD { get; set; }
        public DateTime? Udate { get; set; }
        public string? TokenAddress { get; set; }
    }
}
