using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Models;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class WithdrawalController(ContextClass context, IConfiguration configuration, CommonMethods commonMethods, SqlConnectionClass dataAccess, IAuditRepository auditRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private string mango = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            mango = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            ViewBag.MemName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        [HttpGet("account/withdrawal")]
        public async Task<IActionResult> Withdrawal()
        {
            decimal num = 0;
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                num = Convert.ToDecimal(ds.Tables[0].Rows[0]["Available"]);
                ViewBag.NumValue = num;

                ViewBag.memaddress = commonMethods.Decrypt(ds.Tables[0].Rows[0]["memAddress"].ToString()!);
                ViewBag.acNo = commonMethods.Decrypt(ds.Tables[0].Rows[0]["memAcNo"].ToString()!);
                ViewBag.ifsc = ds.Tables[0].Rows[0]["memAcIfsc"];
                ViewBag.acName = ds.Tables[0].Rows[0]["memAcName"];
            }

            return View();
        }

        [HttpPost("account/withdrawal")]
        public async Task<IActionResult> WithdrawalPost()
        {
            try
            {
                decimal num = 0;
                string amount = Request.Form["WdrlAmount"].ToString();

                if (amount == null || amount == "")
                {
                    TempData["msg"] = "Invalid amount! Please enter a valid amount.";
                    return RedirectToAction("Withdrawal");
                }

                //if (Convert.ToDecimal(amount) < 10 || Convert.ToDecimal(amount) % 1 != 0)
                //{
                //    TempData["msg"] = "Invalid Amount! Amount should be minimum 10 and in multiple of 1.";
                //    return RedirectToAction("Withdrawal");
                //}

                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    num = Convert.ToDecimal(ds.Tables[0].Rows[0]["Available"]);
                }

                if (commonMethods.Decrypt(ds!.Tables[0].Rows[0]["memAddress"].ToString()!) == "")
                {
                    TempData["msg"] = "Update your withdrawal address first. <a href='/account/profile'>Click here to update</a>";
                    return RedirectToAction("Withdrawal");
                }

                if (Convert.ToDecimal(amount) > num)
                {
                    TempData["msg"] = "Insufficient balance! Amount should be less than or equal to available amount.";
                    return RedirectToAction("Withdrawal");
                }

                //string trxValue = (Convert.ToDecimal(amount) - 2).ToString();
                par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@TOTALAmount", num.ToString()));
                par.Add(new SqlParameter("@withdrawal", amount));
                par.Add(new SqlParameter("@wType", "USDT"));
                par.Add(new SqlParameter("@Dtel", ""));
                par.Add(new SqlParameter("@usdPrice", _configuration.GetValue<string>("usdRate")));
                ds = await _dataAccess.FnRetriveByPro("[Amountinsert]", par);

                int chk = Convert.ToInt16(ds.Tables[0].Rows[0]["msg"]);
                string msg = ds.Tables[0].Rows[0]["msg"].ToString()!;

                TempData["success"] = msg;

                await _auditRepository.LogActionAsync($"MEMBER MAIN WITHDRAWAL", chk, mango, $"Member main withdrawal (all income), member ID : {mango}, amount is {amount}, message from our side : {msg}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                return RedirectToAction("Withdrawal");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message.Contains("No connection could be made") ? "Opps! Blockchain API is currently not responding! Please try after sometime." : "Exception message, please contact to admin";
            }
            return RedirectToAction("Withdrawal");

        }

        [HttpGet("account/withdrawal-history")]
        public async Task<IActionResult> WithdrawalHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "WITHDRAWAL HISTORY MEMBER"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

       

    }
}
