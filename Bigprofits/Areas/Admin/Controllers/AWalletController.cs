using Bigprofits.Data;
using Bigprofits.Models;
using Microsoft.AspNetCore.Mvc;
using Bigprofits.Repository;
using Microsoft.Data.SqlClient;
using Bigprofits.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Data;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AWalletController(ContextClass context, HomeRepository homeRepository, CommonMethods commonMethods, SqlConnectionClass dataAccess, IConfiguration configuration) : Controller
    {
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly IConfiguration _configuration = configuration;
        private string admingo = "";
        public override void OnActionExecuting(ActionExecutingContext _context)
        {
            admingo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = context.TableSupports.Where(x => x.MyStatus == 0 && x.ToBy == "ADMIN").ToList();
            ViewBag.msgCount = msg.Count;
        }

        [HttpGet("home")]
        public async Task<IActionResult> AdminDashboard()
        {
            List<SqlParameter> par = [];
            //par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SP_AdminInfo]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            var news = await context.TblNews.ToListAsync();
            if (news.Count >= 0)
            {
                ViewBag.newsdata = await context.TblNews.ToListAsync();
            }
            return View(ds);
        }

        [HttpGet("request-fund")]
        public async Task<IActionResult> FundRequest()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "ROI INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Fund-History")]
        public async Task<IActionResult> FundRequestHistory()
        {
            var data = await context.FundAmounts.OrderByDescending(x => x.Id).ToListAsync();
            return View(data);
        }

        [HttpGet("Request")]
        public async Task<IActionResult> Welletrequest()
        {
            var data = await context.FundAmounts.Where(x => x.Status == 0).ToListAsync();
            return View(data);
        }

        [HttpGet("manage-request/{id}/{status}")]
        public async Task<IActionResult> ReqApproveOrReject(int id, int status)
        {
            var data = await context.FundAmounts.Where(x => x.Id == id && x.Status == 0).FirstOrDefaultAsync();
            if (data != null)
            {
                data.Status = status;
                //data.FromAddress = status == 1 ? "APPROVED" : "REJECTED";
                data.ApproveDate = DateTime.Now;

                var meminfo = await context.MemberInfos.Where(x => x.MemberId == data.MemberId).FirstOrDefaultAsync();
                if (meminfo!.MemFather == null)
                {
                    meminfo.MemFather = "INR";
                }

                context.Update(data);
                await context.SaveChangesAsync();

                TempData["success"] = $"Success! The request has been successfully {(status == 1 ? "approved" : "rejected")}.";
                return Redirect("/britglbl253adpnl/Request");
            }

            TempData["error"] = "Failed! We could not find the request.";
            return Redirect("/britglbl253adpnl/Request");
        }

        [HttpGet("FHistory")]
        public async Task<IActionResult> FundDetailsHistory()
        {
            var data = await context.FundAmounts.OrderByDescending(x => x.Id).ToListAsync();
            return View(data);
        }

        [HttpGet("manage-fund")]
        public IActionResult FundAdd()
        {
            return View();
        }

        [HttpGet("manage-fund-Histry")]
        public async Task<IActionResult> CreditDebitHistry()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@hashId", "TOPUP"));
            par.Add(new SqlParameter("@rtype", "ADMIN FUND"));

            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            return View(ds);
        }

        [HttpPost("manage-fund")]
        public async Task<IActionResult> FundAdd(FundAmount model)
        {
            try
            {
                string memberId = model!.MemberId!.Trim();
                if (model.MemberId.Trim() == "") TempData["msg"] = "Member Id Not Found....";
                //else if (ddlPackage.SelectedIndex <= 0) divMsg.InnerText = "Invalid Duration! Please select duration.";
                else if (model.Amount!.ToString()!.Trim() == "") TempData["msg"] = "Please enter a valid amount.";
                else
                {
                    List<SqlParameter> par = [];
                    par.Add(new SqlParameter("@memberId", memberId));
                    par.Add(new SqlParameter("@byMemberId", model.Ftype));
                    par.Add(new SqlParameter("@wType", "TOPUP"));
                    par.Add(new SqlParameter("@amount", model.Amount!.ToString()!.Trim()));
                    par.Add(new SqlParameter("@type", "ADMIN FUND"));
                    var ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);

                    TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    RedirectToAction("FundAdd");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpPost("Eixt-Member")]
        public async Task<string> IsMemberIdExist(String idMember)
        {
            string result = "";
            try
            {
                var data = await context.MemberInfos.Where(x => x.MemberId == idMember).FirstOrDefaultAsync();
                result = data == null ? "0" : "1";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        [HttpGet("admin-topup")]
        public async Task<IActionResult> MemberTopUp()
        {
            ViewBag.PList = new SelectList(await homeRepository.PinDetail(), "PinType", "PinDetail1");
            return View();
        }

        [HttpPost("admin-topup")]
        public async Task<IActionResult> IsTopUpValid(CmitRequest fund)
        {
            try
            {
                string memberId = fund.MemberId!.ToString().Trim();
                var datam = await context.MemberInfos.Where(t => t.MemLogId == memberId || t.MemberId == memberId).FirstOrDefaultAsync();

                if (memberId.ToString().Trim() == "")
                {
                    TempData["msg"] = "Member Id Not Found....";
                    return Redirect("/britglbl253adpnl/admin-topup");
                }

                if (fund.CmitReqAmount.ToString()!.Trim() == "")
                {
                    TempData["msg"] = "Please enter a valid amount";
                    return Redirect("/britglbl253adpnl/admin-topup");
                }

                if (datam == null || datam.MemberId == null || datam.MemberId.ToString() == "")
                {
                    TempData["msg"] = "Invalid Member! Please enter a valid member ID.";
                    return Redirect("/britglbl253adpnl/admin-topup");
                }

                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", datam.MemberId));
                par.Add(new SqlParameter("@pkgAmount", fund.CmitReqAmount.ToString()!.Trim()));
                par.Add(new SqlParameter("@walletType", "ADMIN"));
                par.Add(new SqlParameter("@HashId", "ADMIN"));
                var ds = await _dataAccess.FnRetriveByPro("[cmitRequestsInsert]", par);

                if (ds.Tables[0].Rows[0]["Chk"].ToString() == "1")
                {
                    string email = commonMethods.Decrypt(ds.Tables[0].Rows[0]["memEmail"].ToString()!);
                    string logId = ds.Tables[0].Rows[0]["memLogId"].ToString()!;
                    string name = ds.Tables[0].Rows[0]["memName"].ToString()!;
                    memberId = ds.Tables[0].Rows[0]["memberId"].ToString()!;
                }

                TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                // return Json(ds.Tables[0].Rows[0]["msg"].ToString());
                return Redirect("/britglbl253adpnl/admin-topup");
            }
            catch (Exception)
            {
                return Json("Exception message, please contact to admin");
            }
        }

        [HttpGet("topup-history")]
        public async Task<IActionResult> TopupHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "TOPUP HISTORY"));
            par.Add(new SqlParameter("@atype", "MEMBER"));

            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Today-Deposits-history")]
        public async Task<IActionResult> TODAYDEPOSITS()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "TODAY DEPOSITS"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Total-Deposits-history")]
        public async Task<IActionResult> TOTALDEPOSITS()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "TOTAL DEPOSITS"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

            ViewBag.data = ds;
            return View(ds);
        }

        [HttpGet("Today-Withdrawal-history")]
        public async Task<IActionResult> TODAYWITHDRAWAL()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "TODAY WITHDRAWAL"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("TOTAL-Withdrawal-history")]
        public async Task<IActionResult> TOTALYWITHDRAWAL()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "TOTAL WITHDRAWAL"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("TOTAL-Status--history/{type}")]
        public async Task<IActionResult> WITHDRAWALSTATUS(int type)
        {
            if (type == 0)
            {
                ViewBag.staus = "PENDING";
            }
            else
            {
                ViewBag.staus = "SUCESS";
            }
            List<SqlParameter> par = [];

            par.Add(new SqlParameter("@rtype", "TOTAL SUCESS WITHDRAWAL"));
            par.Add(new SqlParameter("@atype", type));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Change-Password")]
        public async Task<IActionResult> ChangePassword()
        {
            var adminInfo = await context.UserMasters.Where(x => x.UserId == admingo).FirstOrDefaultAsync();
            return View(adminInfo);
        }

        [HttpPost("Change-Password")]
        public async Task<JsonResult> IsPasswordChanged(string op, string np, string cp)
        {
            try
            {
                if (np != cp)
                {
                    return Json("New And Confirm Password Doesn't Match");
                }

                var data = await context.UserMasters.Where(u => u.Role == "SuperAdmin" && u.UserPass == commonMethods.Encrypt(op.ToString().Trim())).FirstOrDefaultAsync();
                if (data != null && data.Role!.Equals("superadmin", StringComparison.CurrentCultureIgnoreCase))
                {
                    data.UserPass = commonMethods.Encrypt(cp.ToString().Trim());
                    context.SaveChanges();
                    return Json("Congratulation!!  password changed successfully");
                }
                else
                {
                    return Json("Old password missmatch, Please try again..");
                }
            }
            catch (Exception)
            {
                return Json("Exception message, please contact to admin");
            }
        }

        [HttpPost("GetName/{memberId}")]
        public string GetMemberName(String memberId)
        {
            string? result = "";
            try
            {
                var data = context.MemberInfos.Where(x => x.MemberId == memberId).FirstOrDefault();
                return data == null ? "❌ No such sponsor exists" : $"✅ {data.MemLogId}";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result!;
        }


    }
}
