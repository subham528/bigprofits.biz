using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class ComplainMail
    {
        public int MailSno { get; set; }
        public string? MemberId { get; set; }
        public string? MailId { get; set; }
        public string? MailSubject { get; set; }
        public string? MaiBody { get; set; }
        public string? MailReply { get; set; }
        public byte? MailStatus { get; set; }
        public string? ToMemberId { get; set; }
    }
}
