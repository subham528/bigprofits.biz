using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ActionLog
    {
        public int Id { get; set; }
        public string ActionType { get; set; } = null!;
        public int? EntityId { get; set; }
        public string? UserType { get; set; }
        public string? Details { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
