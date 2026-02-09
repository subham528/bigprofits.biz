using Microsoft.AspNetCore.Mvc;
using Bigprofits.Models;
using Bigprofits.Data;
using Bigprofits.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Bigprofits.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class DashboardController(ILogger<HomeController> logger, ContextClass context, HomeRepository homeRepository, MailRepository mailRepository, CommonMethods commonMethods, SqlConnectionClass dataAccess) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly MailRepository mailRepository = mailRepository;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private string mango = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            mango = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            ViewBag.MemName = User.FindFirst(ClaimTypes.GivenName)?.Value;
            ViewBag.memberId = mango;
        }

        [HttpGet("account/home")]
        public async Task<IActionResult> Dashboard()
        {
            TempData["memberid"] = mango;

            TempData["LeftLink"] = $"https://{Global.ProjectUrl}/account/create-account/{mango}/LEFT";
            TempData["RightLink"] = $"https://{Global.ProjectUrl}/account/create-account/{mango}/RIGHT";

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.postdate = ds.Tables[0].Rows[0]["PosDate"];
                ViewBag.SpMember = JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }

            // Get News Data
            var news = await context.TblNews.ToListAsync();
            if (news.Count > 0)
            {
                ViewBag.newsdata = news;
            }

            return View();
        }

        [HttpGet("account/inbox-massege")]
        public async Task<IActionResult> Inbox()
        {
            try
            {
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@atype", "MEMBER"));
                par.Add(new SqlParameter("@rtype", "OUTBOX"));
                var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

                return View(ds);
            }
            catch (Exception)
            {
            }
            return View();
        }

        [HttpGet("account/new-message-outbox")]
        public async Task<IActionResult> Outbox()
        {
            var data = await context.TableSupports.Where(t => t.FromBy == mango).OrderByDescending(t => t.Srno).ToListAsync();
            return View(data);
        }

        [HttpGet("account/new-message")]
        public IActionResult Support()
        {
            return View();
        }

        [HttpPost("account/new-message")]
        public async Task<IActionResult> Support(TableSupport model)
        {
            try
            {
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@memberid1", "ADMIN"));
                par.Add(new SqlParameter("@Subject", model.MySubject));
                par.Add(new SqlParameter("@description", model.MyDetails));
                var ds = await _dataAccess.FnRetriveByPro("[SP_SUPPORTREQUEST]", par);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    return RedirectToAction("support");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }

            return View();
        }

        [HttpGet("account/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserAuth");
            return Redirect("/account/sign-in");
        }

    }




}
