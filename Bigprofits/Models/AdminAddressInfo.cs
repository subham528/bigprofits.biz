using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class AdminAddressInfo
    {
        public int Id { get; set; }
        public string? Ctype { get; set; }
        public string? DepositAddress { get; set; }
        public string? WithdrawalAddress { get; set; }
        public string? ContractAddress { get; set; }
        public string? TokenAddress { get; set; }
        public string? ContractAbi { get; set; }
    }
}
