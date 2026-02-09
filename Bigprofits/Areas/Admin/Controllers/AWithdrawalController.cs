using Bigprofits.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AWithdrawalController(ContextClass context, CommonMethods commonMethods, SqlConnectionClass dataAccess) : Controller
    {
        private readonly ContextClass context = context;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private string admingo = "";
        public override void OnActionExecuting(ActionExecutingContext _context)
        {
            admingo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = context.TableSupports.Where(x => x.MyStatus == 0 && x.ToBy == "ADMIN").ToList();
            ViewBag.msgCount = msg.Count;
        }

        [HttpGet("manage-withdrawal/{type}")]
        public async Task<IActionResult> WidManage(string type)
        {
            List<SqlParameter> par = [];
            if (type != "TOKEN" && type != "USDT")
            {
                return BadRequest("Invalid type value. Must be either 'TOKEN' or 'USDT'.");
            }

            par.Add(new SqlParameter("@hashId", type));
            par.Add(new SqlParameter("@rtype", "MANAGE WITHDRAWAL"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

            return View(ds);
        }

        [HttpPost("manage-withdrawal")]
        public async Task<JsonResult> WidManage(string widId, string action)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", widId));
            par.Add(new SqlParameter("@atype", action));
            par.Add(new SqlParameter("@type", "MANAGE WITHDRAWAL"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_Action]", par);

            return Json(ds.Tables[0].Rows[0]["msg"].ToString());
        }

        //[HttpGet("withdrawal-request/{type}")]
        //public async Task<IActionResult> MemberWidReq(string type)
        //{
        //    ViewBag.type = type == "TOKEN" ? "CPC" : "USDT";
        //    List<SqlParameter> par = [];

        //    par.Add(new SqlParameter("@hashId", type));
        //    par.Add(new SqlParameter("@rtype", "WITHDRAWAL REQUEST"));
        //    var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

        //    return View(ds);
        //}

        //[HttpGet("withdrawal-Request-Dp")]
        //public async Task<IActionResult> MemberWidReqDp()
        //{
        //    List<SqlParameter> par = [];   
        //    par.Add(new SqlParameter("@rtype", "TOKEN REQUEST"));
        //    var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

        //    return View(ds);
        //}

        //[HttpGet("withdrawal-Request-usdt")]
        //public async Task<IActionResult> MemberWidReq()
        //{
        //    List<SqlParameter> par = [];
        //    par.Add(new SqlParameter("@hashId", "USDT"));
        //    par.Add(new SqlParameter("@rtype", "WITHDRAWAL REQUEST"));
        //    var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

        //    return View(ds);
        //}

        [HttpGet("withdrawal-requestFinal/{type}")]
        public async Task<IActionResult> MemberWidReqFinal(string type)
        {
            ViewBag.type = type == "TOKEN" ? "CPC" : "USDT";

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@hashId", type));
            par.Add(new SqlParameter("@rtype", "WITHDRAWAL REQUEST"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

            return View(ds);
        }

        [HttpGet("manage-withdrawal-inr")]
        public async Task<IActionResult> WidManageInr()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@hashId", "INR"));
            par.Add(new SqlParameter("@rtype", "MANAGE WITHDRAWAL"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

            return View(ds);
        }

        [HttpPost("manage-withdrawal-inr")]
        public async Task<JsonResult> WidManageInr(string widId, string action)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", widId));
            par.Add(new SqlParameter("@atype", action));
            par.Add(new SqlParameter("@type", "MANAGE WITHDRAWAL INR"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_Action]", par);

            return Json(ds.Tables[0].Rows[0]["msg"].ToString());
        }

        [HttpGet("withdrawal-history")]
        public async Task<IActionResult> WithdrawalHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "WITHDRAWAL HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        //[HttpGet("Today-withdrawal-history")]
        //public async Task<IActionResult> TodatWithdrawalHistory()
        //{
        //    List<SqlParameter> par = [];
        //    par.Add(new SqlParameter("@rtype", "TODAY WITHDRAWAL HISTORY"));
        //    var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
        //    ViewBag.data = ds;

        //    return View(ds);
        //}

        [HttpGet("DINP-withdrawal-history")]
        public async Task<IActionResult> DINPWithdrawalHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "TOKEN HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Pending-withdrawal-Request")]
        public async Task<IActionResult> PendWithdrawalHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "PENDING WITHDRAWAL HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpPost("approve-withdrawal-usdt")]
        public async Task<JsonResult> AdminApproveWid(string hashId, string widId)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@hasId", hashId));
            par.Add(new SqlParameter("@widId", widId));
            await _dataAccess.FnRetriveByPro("[SP_ApproveWid]", par);

            return Json("1");
        }

        [HttpPost("approve-withdrawal-Dp")]
        public async Task<JsonResult> AdminApproveWidDp(string hashId, string widId)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@hasId", hashId));
            par.Add(new SqlParameter("@detl", "DRONTOKEN"));
            par.Add(new SqlParameter("@widId", widId));
            await _dataAccess.FnRetriveByPro("[SP_ApproveWid]", par);

            return Json("1");
        }

        [HttpPost("address-info")]
        public async Task<JsonResult> GetAddressInfo(string currency)
        {
            currency = currency == "USDT" ? "USDT-BEP-20" : "CPC-BEP-20";
            var data = await context.AdminAddressInfos.Where(x => x.Ctype == "USDT-BEP-20").FirstOrDefaultAsync();
            if (data != null)
            {
                currency = "USDT-BEP-20";
                return Json(new
                {
                    depositAddress = commonMethods.Decrypt(data.DepositAddress!),
                    tokenAddress = data.TokenAddress,
                    withdrawalAddress = commonMethods.Decrypt(data.WithdrawalAddress!),
                    privateKey = (currency == "CPC-BEP-20" ? "b4f1e4fbe6a9bb915a4675f4d3984a7b9282b529cfee055e9a004c065a397e8d" : "0x00036c9bd440385d5d86589512347ff4f74b64b2ccbf93e034b1ff8c18f9fc63"),
                    contractAbi = data.ContractAbi,
                    contractAddress = data.ContractAddress
                });
            }
            else return Json("Failed! Could not find address info.");
        }

        [HttpGet("pay-token/{id}")]
        public async Task<IActionResult> PayToken(string id)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", id));
            par.Add(new SqlParameter("@status", "0"));
            par.Add(new SqlParameter("@rtype", "TOKEN PAY LIST"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

            return View(ds);
        }

        [HttpGet("pay-token-lit/{status}")]
        public async Task<IActionResult> TokenHistory(int status)
        {
            ViewBag.Status = status;

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@status", status));
            par.Add(new SqlParameter("@rtype", "TOKEN PAY LIST"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpPost("approve-token-pay")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<JsonResult> TokenPayApprove([FromForm] string hashId, [FromForm] string from, [FromForm] string to, [FromForm] string id)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", id));
            par.Add(new SqlParameter("@fromAddress", from));
            par.Add(new SqlParameter("@toAddress", to));
            par.Add(new SqlParameter("@hashId", hashId));
            par.Add(new SqlParameter("@type", "PAY TOKEN"));
            await _dataAccess.FnRetriveByPro("[SP_Action]", par);

            return Json("1");
        }

    }
}
