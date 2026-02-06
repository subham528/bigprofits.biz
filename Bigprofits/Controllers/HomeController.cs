using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Bigprofits.Models;
using Bigprofits.Repository;
using System.Diagnostics;
using Bigprofits.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

namespace Bigprofits.Controllers
{
    public class HomeController(ILogger<HomeController> logger, ContextClass context, HomeRepository homeRepository, MailRepository mailRepository, CommonMethods commonMethods, SqlConnectionClass dataAccess, IConfiguration configuration) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly MailRepository mailRepository = mailRepository;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly IConfiguration _configuration = configuration;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            TempData["ProjectName"] = Global.projectName;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("account/sign-in")]
        public IActionResult Login(string? ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost("account/sign-in")]
        public async Task<IActionResult> LoginPost()
        {
            string Walletaddress = Request.Form["WalletAddress"].ToString().Trim();
            if (Walletaddress == "")
            {
                TempData["loginError"] = "Failed : Please Connect Wallet Frist.";
                return RedirectToAction("Login");
            }

            var allMembers = await context.MemberInfos.ToListAsync();
            var data = allMembers.FirstOrDefault(x => commonMethods.Decrypt(x.MemAddress!).Equals(Walletaddress, StringComparison.CurrentCultureIgnoreCase));
            if (data != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, data.MemberId!),
                    new(ClaimTypes.NameIdentifier, data.MemberId!),
                    new(ClaimTypes.GivenName, data.MemName!),
                    new(ClaimTypes.Email, commonMethods.Decrypt(data.MemEmail!)),
                    new(ClaimTypes.MobilePhone, commonMethods.Decrypt(data.MemMobile!))
                };

                var identity = new ClaimsIdentity(claims, "UserAuth");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync("UserAuth", principal, authProperties);
                return Redirect((TempData["ReturnUrl"] == null ? "/account/home" : TempData["ReturnUrl"]!.ToString()!));
            }
            TempData["loginError"] = "Wallet Address MisMatch..";
            return View();
        }

        [HttpGet]
        [Route("account/create-account")]
        [Route("account/create-account/{SpoId}")]
        [Route("account/create-account/{SpoId}/{RefPos}")]
        public async Task<IActionResult> Registration(string? SpoId, string? RefPos)
        {
            ViewBag.depositAdsress = Global.Bp20Address;
            ViewBag.SpoId = SpoId;
            ViewBag.RefPos = RefPos;

            ViewBag.SList = new SelectList(await homeRepository.StateList(), "Sname", "Sname");
            ViewBag.CList = new SelectList(await homeRepository.CountryList(), "Cname", "Cname");
            return View();
        }

        [HttpPost]
        [Route("account/create-account")]
        public async Task<IActionResult> Registration(MemberInfo memberInfo)
        {
            try
            {
                string Walletaddress = Request.Form["WalletAddress"].ToString().Trim();
                if (memberInfo.SpoId == null)
                {
                    TempData["msgDR"] = "Please fill required field..!!";
                    return RedirectToAction("Registration");
                }

                if (Walletaddress == "")
                {
                    TempData["msgDR"] = "Failed : Please Connect Wallet Frist.";
                    return RedirectToAction("Registration");
                }

                if (memberInfo.RefPos != "LEFT" && memberInfo.RefPos != "RIGHT")
                {
                    TempData["msgDR"] = "Failed : Invalid postion! Please Enter Select Postion.";
                    return RedirectToAction("Registration");
                }

                var isSpo = await context.MemberInfos.Where(t => t.MemLogId == memberInfo.SpoId!.Trim() && t.MemRank == 1).FirstOrDefaultAsync();
                if (isSpo == null)
                {
                    TempData["msgDR"] = "Failed : Invalid sponsor ID entered. Please enter a valid sponsor ID..";
                    return RedirectToAction("Registration");
                }

                //var isWal = await context.MemberInfos.Where(t => t.MemAddress == commonMethods.Encrypt(Walletaddress)).ToListAsync();
                //if (isWal.Count > 0)
                //{
                //    TempData["msgDR"] = "Failed : This wallet address is already registerd.";
                //    return RedirectToAction("Registration");
                //}

                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@spoId", isSpo.MemberId));
                par.Add(new SqlParameter("@address", commonMethods.Encrypt(Walletaddress)));
                par.Add(new SqlParameter("@refPos", memberInfo.RefPos.Trim()));
                var ds = await _dataAccess.FnRetriveByPro("[SP_Registration]", par);

                TempData["Ssuc"] = ds.Tables[0].Rows[0]["MSG"].ToString();

                string mailMsg = "Hello " + memberInfo.MemName + " , and Welcome to Dronip! <br><br> YOUR MEMBERSHIP PROFILE HAS BEEN SUCCESSFULLY CREATED! <br><br> You need this membership information to participate in Dronip! <br><br> So feel free to LOG IN, NOW, and decide how you are going to participate!: <br><br> http://www.bigprofits.biz <br><br><br> Below is the account information you just saved: <br><br> CONTACT NAME: " + memberInfo.MemName + " <br> CONTACT E-MAIL: " + memberInfo.MemEmail + " <br><br> USER ID: " + ds.Tables[0].Rows[0]["memLogId"].ToString() + " <br> PASSWORD: " + memberInfo.MemLogPass + " <br><br><br> Your account number here on file is always your USER ID, so please provide this on all correspondence to us. <br><br> For any further questions, please e-mail our Support Department at mailto:support@bigprofits.biz <br><br> Best Regards, <br><br><br> Support <br> Dronip";

                //mailRepository.SendMail(memberInfo.MemEmail, mailMsg, "Registration Successfull!");

                return RedirectToAction("Registration");
            }
            catch (Exception)
            {
                TempData["msgDR"] = "Something Went Wrong..!!";
            }
            return RedirectToAction("Registration");
        }

        [HttpPost]
        [Route("account/SponsorName/{spoId}")]
        public string GetSponsorName(String spoId)
        {
            string? result = "";
            try
            {
                var data = context.MemberInfos.Where(x => x.MemLogId == spoId).FirstOrDefault();
                return data == null ? "❌ No such sponsor exists" : $"✅ {data.MemLogId}";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result!;
        }

        [HttpGet("account/forget-password")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost("account/forget-password")]
        public async Task<IActionResult> ForgetPasswordPost()
        {
            var MemberId = Request.Form["memberid"].ToString();
            var data = await context.MemberInfos.Where(x => x.MemberId == MemberId).FirstOrDefaultAsync();

            string memEmail = commonMethods.Decrypt(data!.MemEmail!);
            string? password = data.MemLogPass;
            string? mobile = commonMethods.Decrypt(data.MemMobile!);

            string mailMsg = "Your Requested Log-in Information <br><br> Hi " + data.MemName + ", <br><br> You have requested to retrieve your log-in data for your Bigprofits Account. <br><br> Your log-in data is: <br><br> USER ID: " + data.MemLogId + " <br> Password: " + commonMethods.Decrypt(data.MemLogPass!) + " <br><br> Login at the following link: <br> https://Bigprofits.io/account/sign-in <br><br> Best regards, <br><br> Member Support <br> Bigprofits";

            if (await mailRepository.SendMail(memEmail, mailMsg.ToString(), "Recover Password!") == "1")
                TempData["forgetMsg"] = "Your account login credentials has been sent to your registered email. Thank You!";
            else TempData["forgetMsg"] = "Failed : Opps! Something went wrong. Could not send email.";

            //string website, msg, thanks, dltId;
            //website = "Dear Member";
            //msg = " Your password is =" + password + ", Thanks From ";
            //thanks = Global.ProjectUrl;
            //dltId = "1207161788485001062";

            //string mailmsg = website + msg + thanks; //" Welcome To  www.increazer.in !!!   Dear Member Your  Registration Successfully...! Your  Userid='" + Session["logid1"] + "' and Password='" + Session["Pass"] + "' Thanks From ";
            //ViewBag.ru = mailmsg.ToString();

            //TempData["msg"] = mailmsg.ToString();
            //TempData["forgetMsg"] = "Your Password has been sent to your Mobile No";

            return RedirectToAction("ForgetPassword", "Home");
        }

        //[HttpPost("account/withdrawal-info")]
        //public async Task<IActionResult> IsWidValid([FromBody] JsonElement par)
        //{
        //    try
        //    {
        //        string userId = par.GetProperty("userId").GetString()!;
        //        int orderId = (int)par.GetProperty("orderId").GetDecimal();

        //        var data = await context.MemberWidInfos.Where(x => x.Id == orderId && x.MemberId == userId && x.Status == "0").FirstOrDefaultAsync();
        //        if (data == null) return Json(new { success = false, message = "Opps! Something went wrong. We could not find the required data, please try again sometime." });
        //        else
        //        {
        //            var memInfo = await context.MemberInfos.Where(x => x.MemberId == userId).FirstOrDefaultAsync();
        //            return Json(new { success = true, userId = memInfo!.MemberId, address = commonMethods.Decrypt(memInfo.MemAddress!), amount = string.Format("{0:0.####}", data.FinalAmount) });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { success = false, message = "Exception message, please contact to admin" });
        //    }
        //}

        //[HttpPost("account/update-withdrawal")]
        //public async Task<IActionResult> UpdateWithdrawal([FromBody] JsonElement par)
        //{
        //    try
        //    {
        //        string userId = par.GetProperty("userId").GetString()!;
        //        int orderId = (int)par.GetProperty("orderId").GetInt16();
        //        string hashId = par.GetProperty("hashId").GetString()!;
        //        string status = par.GetProperty("status").GetString()!;
        //        string message = par.GetProperty("message").GetString()!;

        //        var data = await context.MemberWidInfos.Where(x => x.Id == orderId && x.MemberId == userId && x.Status == "0").FirstOrDefaultAsync();
        //        if (data == null) return Json(new { success = false, message = "Opps! Something went wrong. We could not find the required data, please try again sometime." });
        //        else
        //        {
        //            data.Status = status;
        //            data.HashId = hashId;
        //            data.CmitId = message;
        //            data.AdminApprovedDate = DateTime.Now;

        //            await context.SaveChangesAsync();

        //            return Json(new { success = true, message = "Withdrawal request updated successfully!" });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { success = false, message = "Exception message, please contact to admin" });
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}