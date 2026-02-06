using Bigprofits.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class IncomeController(ContextClass context, CommonMethods commonMethods, SqlConnectionClass dataAccess) : Controller
    {
        private readonly ContextClass context = context;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private string mango = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            mango = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            ViewBag.MemName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        [HttpGet("account/roi-income")]
        public async Task<IActionResult> DailyROI()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "ROI INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/Dinp-History")]
        public async Task<IActionResult> DinpTokenHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "TOKEN HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/sponosr-Income")]
        public async Task<IActionResult> SponsorIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@hashId", "LEVEL"));
            par.Add(new SqlParameter("@rtype", "LEVEL INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/mytopup-histry")]
        public async Task<IActionResult> Mytopuphistroy()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
       
            par.Add(new SqlParameter("@rtype", "TOPUP HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/Reward-List")]
        public async Task<IActionResult> ReffrealIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@atype", "REWARD"));
            par.Add(new SqlParameter("@rtype", "REWARD HISTORY"));

            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/Achiverment-List/{type}")]
        public async Task<IActionResult> AchivementList( string type)
        {
            if (type== "REWARD")
            {
                ViewBag.name = "Reward Achievement List";
            }
            else
            {
                ViewBag.name = "Salary Achievement List";
            }
                List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@atype", type));
            par.Add(new SqlParameter("@rtype", "REWARD HISTORY"));

            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/CreditDebit-History")]
        public async Task<IActionResult> CreditDebitHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "RECEIVED FUND"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/matching-Income")]
        public async Task<IActionResult> LevelIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
         
            par.Add(new SqlParameter("@rtype", "BINARY INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/Salllry-Income")]
        public async Task<IActionResult> ProductLevelIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));

            par.Add(new SqlParameter("@rtype", "REWARD RETURN"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/matrix-Direct-Income")]
        public async Task<IActionResult> MatirxDirtectIncoem()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
          
            par.Add(new SqlParameter("@rtype", "MATRIX INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/matrix-profit-Income")]
        public async Task<IActionResult> MatirxDirtectIncoemprofit()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));

            par.Add(new SqlParameter("@rtype", "REWARD HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/reward-Income")]
        public async Task<IActionResult> Salary()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "REWARD RETURN"));
          
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/Bonus-Income")]
        public async Task<IActionResult> Bonusincome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "ACHIEVER INCOME"));

            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

    }
}
