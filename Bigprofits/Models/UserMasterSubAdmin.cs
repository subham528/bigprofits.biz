using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class UserMasterSubAdmin
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserPass { get; set; }
        public DateTime? LoginDate { get; set; }
        public string? Role { get; set; }
    }
}
