using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ListOperator
    {
        public int Id { get; set; }
        public string? OpName { get; set; }
        public int? OpCode { get; set; }
        public string? OpType { get; set; }
    }
}
