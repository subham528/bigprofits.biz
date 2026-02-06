using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class Matrix2
    {
        public int Id { get; set; }
        public string? MemberId { get; set; }
        public string? RefId { get; set; }
        public int? RefTblId { get; set; }
        public int? Position { get; set; }
        public int? Fill { get; set; }
        public int? Mststus { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Dcount { get; set; }
        public int? Recycle { get; set; }
        public int? MatrixNo { get; set; }
        public string? IncomeTo { get; set; }
        public string? RootId { get; set; }
        public int? Birth { get; set; }
        public string? Etype { get; set; }
        public int? Icount { get; set; }
        public string? SpoIncome { get; set; }
    }
}
