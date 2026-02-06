global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;

class Global
{
    public const string projectName = "Bigprofits";
    public const string ProjectUrl = "bigprofits.biz";

    //Session Value
    public const string userid = "userid";
    public const string memlogid = "memlogid";
    public const string username = "username";
    public const string userpass = "userpass";
    public const string role = "role";
    public const string Layout = "Layout";

    //Session Value For Admin
    public const string AdminId = "AdminId";
    public const string AdminPass = "AdminPass";
    public const string AdminRole = "AdminRole";

    //Type Of Role
    public const string Member = "Member";
    public const string Admin = "Admin";

    //Layout Variable
    public const string memberLayout = "~/Views/Shared/_Layout.cshtml";
    public const string adminLayout = "~/Areas/Admin/Views/Shared/_AALayout.cshtml";

    //API DETAILS
    public const string ApiKey = "mVX/A7tldLhq84G6qSrstQ==";
    public const string ApiUserId = "jaydeepsingh347@gmail.com";
    public const string ApiUrl = "https://www.worldpayment.in";

    //Referral Link
    public const string RefLink = "https://www.technotycoon.in/Auth/Registration?spoId=";
    //BEP-20 address
    public const string Bp20Address = "0xDc93CB0521930b72Da1f0020a2c2099eD71a8cF6";
    public const string Dp20address = "0x6009af4a30c39CD79561BaBd5291746178d0d6F5";

}
