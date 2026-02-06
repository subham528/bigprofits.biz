using System;
using System.Collections.Generic;

namespace Bigprofits.Models
{
    public partial class MemberInfoHistory
    {
        public int MemId { get; set; }
        public string? MemberId { get; set; }
        public string? MemLogId { get; set; }
        public string? MemLogPass { get; set; }
        public string? SpoId { get; set; }
        public string? RefId { get; set; }
        public string? RefPos { get; set; }
        public string? MemName { get; set; }
        public string? LastName { get; set; }
        public string? MemAddress { get; set; }
        public string? MemMobile { get; set; }
        public string? MemEmail { get; set; }
        public string? MemGender { get; set; }
        public string? MemDob { get; set; }
        public string? MemFather { get; set; }
        public string? MemPanNo { get; set; }
        public string? MemCity { get; set; }
        public string? MemState { get; set; }
        public string? MemCountry { get; set; }
        public byte? MemActive { get; set; }
        public string? PinCode { get; set; }
        public string? MemAcName { get; set; }
        public string? MemAcNo { get; set; }
        public string? MemAcBank { get; set; }
        public string? MemAcBranch { get; set; }
        public string? MemAcCity { get; set; }
        public string? MemAcIfsc { get; set; }
        public string? MemAcType { get; set; }
        public string? MemAcAtm { get; set; }
        public DateTime? MemReg { get; set; }
        public byte? MemRank { get; set; }
        public string? MemnominiName { get; set; }
        public string? MemnominiRel { get; set; }
        public string? MemnominiAdd { get; set; }
        public string? MemnominiPhone { get; set; }
        public string? MemniminiCity { get; set; }
        public DateTime? PosDate { get; set; }
        public string? MemPackage { get; set; }
        public decimal? MemPackageValue { get; set; }
        public DateTime? PanApproveDate { get; set; }
        public string? Idtype { get; set; }
        public string? Idnumber { get; set; }
        public string? Kycstatus { get; set; }
        public string? ResidencyIdtype { get; set; }
        public string? ResidencyIdnumber { get; set; }
        public string? MemAdharNo { get; set; }
        public int? PanStatus { get; set; }
        public string? ProfileImage { get; set; }
        public string? Kycimgname { get; set; }
        public string? KycimgnameOther { get; set; }
        public string? KycDocumentname { get; set; }
        public string? KycDocumentnameothr { get; set; }
        public string? KycDocumentNo { get; set; }
        public string? KycDocumentNoothr { get; set; }
        public int? Memposno { get; set; }
        public string? BlockName { get; set; }
        public int? ClubNo { get; set; }
        public string? ClubName { get; set; }
    }
}
