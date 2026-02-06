using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AddressInfo
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }
        public string? Base58Address { get; set; }
        public string? HexAddress { get; set; }
        public int? Cstatus { get; set; }
        public DateTime? Cdate { get; set; }
    }
}
