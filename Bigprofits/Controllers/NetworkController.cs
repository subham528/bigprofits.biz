using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using System.Data;
using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Models;
using Bigprofits.Repository;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Bigprofits.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class NetworkController(ContextClass context, HomeRepository homeRepository, CommonMethods commonMethods, SqlConnectionClass dataAccess) : Controller
    {
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private string mango = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            mango = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            ViewBag.MemName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        [HttpGet("account/Direct-Member")]
        public async Task<IActionResult> DirectMember()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "MEMBER DIRECT"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpGet("account/DownLine-Line")]
        public async Task<IActionResult> DownLine()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[memberDownline]", par);
            ViewBag.data = ds.Tables[0];
            return View(ds);
        }

        [HttpGet("account/todatbiz-list")]
        public async Task<IActionResult> TodaybizlistDownLine()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SP_TodayTeamBusiness]", par);

            DataTable filteredTable = ds.Tables[0].Clone();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (decimal.TryParse(row["todayBusiness"].ToString(), out decimal biz) && biz > 0)
                {
                    filteredTable.ImportRow(row);
                }
            }

            ViewBag.TeamList = filteredTable;
            return View();
        }

        [HttpGet("account/TeamBusiness-list")]
        public async Task<IActionResult> TeamBusinessList(string fromDate, string toDate)
        {
            List<SqlParameter> par =
            [
                new SqlParameter("@memberId", mango),
                new SqlParameter("@rtype", "TEAM BUSINESS HISTORY"),
                new SqlParameter("@fromDate", string.IsNullOrEmpty(fromDate) ? (object)DBNull.Value : fromDate),
                new SqlParameter("@toDate", string.IsNullOrEmpty(toDate) ? (object)DBNull.Value : toDate)
            ];

            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            return View(ds);
        }

        [HttpGet("account/DownlineLevel-Line")]
        public async Task<IActionResult> DownlineLevel()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "MEMBER DIRECT"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            try
            {
                par = [new SqlParameter("@memberId", mango)];
                ds = await _dataAccess.FnRetriveByPro("[memberDownline]", par);
                ViewBag.data = ds.Tables[1];
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View(ds);
        }

        [HttpGet("account/View/{MemId}")]
        public async Task<IActionResult> ViewDownline(string MemId)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@level", MemId));
            var ds = await _dataAccess.FnRetriveByPro("[memberDownline]", par);
            return View(ds);
        }

        [HttpGet("account/pool-entry/{type}/{matrixNo}")]
        public async Task<IActionResult> MatrixEntry(string type, int matrixNo)
        {
            ViewBag.member = mango;
            // Fix title assignment to avoid duplicates
            ViewBag.title = type switch
            {
                "SELF" => "SELF Level Bonus",
                "GLOBAL" => "Global Rank Bonus",
                "NEXTPOOL_GLOBAL" => "NEXT POOL Global",
                "NEXTPOOL_SELF" => "NEXT POOL Self",
                _ => "Downline Overview"
            };

            // Retrieve downline data using SP_GetDownlines
            var par = new List<SqlParameter>
            {
                new("@memberId", mango ?? string.Empty), // Ensure mango is not null
                new("@matrixNo", matrixNo),
                new("@etype", type),
                new("@level", DBNull.Value) // NULL to get all levels
            };
            var ds = await _dataAccess.FnRetriveByPro("[SP_Reentry_main_GetDownlines]", par);

            // Assign result sets to ViewBag
            ViewBag.data = ds.Tables.Count > 1 ? ds.Tables[1] : new DataTable(); // Summary (level counts)
            ViewBag.dataLevel = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable(); // Detailed downlines

            return View();
        }

        [HttpGet]
        [Route("account/pool-downline/{member}/{type}/{level}")]
        [Route("account/pool-downline/{member}/{type}/{level}/{matrixNo}")]
        public async Task<IActionResult> MatrixDownline(string member, string type, string level, int? matrixNo = 1)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(member) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(level))
            {
                TempData["msg"] = "Invalid parameters provided.";
                return View(new DataSet());
            }

            // Set ViewBag properties
            ViewBag.title = type switch
            {
                "SELF" => "Auto Level Bonus",
                "GLOBAL" => "Global Rank Bonus",
                "NEXTPOOL_GLOBAL" => "Next Pool Global",
                "NEXTPOOL_SELF" => "Next Pool Self",
                _ => "Downline Members"
            };
            ViewBag.member = member;
            ViewBag.matrixNo = matrixNo ?? 1;

            // Parse level
            int? parsedLevel = int.TryParse(level, out int lvl) ? lvl : (int?)null;

            // Call SP_Reentry_main_GetDownlines
            var par = new List<SqlParameter>
            {
                new("@memberId", member),
                new("@matrixNo", matrixNo ?? 1),
                new("@etype", type),
                new("@level", parsedLevel.HasValue ? parsedLevel : (object)DBNull.Value)
            };

            var ds = await _dataAccess.FnRetriveByPro("[SP_Reentry_main_GetDownlines]", par);
            DataSet model = new();

            if (ds != null && ds.Tables.Count > 0)
            {
                // Prefer third table (specific MemberId downlines) if available and has rows
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    model.Tables.Add(ds.Tables[2].Copy());
                }
                // Fall back to first table (incomeTo-based downlines) if third is empty
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    model.Tables.Add(ds.Tables[0].Copy());
                }
                else
                {
                    // Add an empty table if no data
                    model.Tables.Add(new DataTable());
                }
            }
            else
            {
                // Add an empty table if no dataset
                model.Tables.Add(new DataTable());
            }

            return View("MatrixDownline", model);
        }

        [HttpGet("account/Tree-structure")]
        public async Task<IActionResult> Genealogy(string? Id)
        {
            string memberId = mango;
            if (Id != null)
            {
                var memId = await context.MemberInfos.Where(x => x.MemLogId == Id).FirstOrDefaultAsync();
                if (memId == null)
                {
                    base.TempData["msg"] = "Invalid member ID! Please enter a  valid member ID.";
                }
                else if ((await context.TeamInfos.Where((TeamInfo x) => x.MemberId == memId.MemberId && x.SpoId == memberId).ToListAsync()).Count <= 0)
                {
                    base.TempData["msg"] = "Invalid downline! This member is not your downline.";
                }
                else
                {
                    memberId = memId.MemberId!;
                }
            }

            string config2 = "<script>var config = {container: '#nodetree', rootOrientation: 'NORTH', levelSeparation: 60, siblingSeparation: 60, subTeeSeparation: 60, node: { collapsable: true\t}, connectors: { type: 'step'}, animateOnInit: true, animation: { nodeAnimation: 'easeOutBounce', nodeSpeed: 700, connectorsAnimation: 'bounce',\tconnectorsSpeed: 700 }},";
            string chart_config2 = "chart_config =  [config, ";
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", memberId));
            par.Add(new SqlParameter("@type", "BINARY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_TreeView]", par);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    bool isEmpty = Convert.ToBoolean(ds.Tables[0].Rows[i]["isEmpty"]);
                    string parent = ds.Tables[0].Rows[i]["parent"].ToString()!;
                    if (isEmpty)
                    {
                        ds.Tables[0].Rows[i]["spoLogId"].ToString();
                        ds.Tables[0].Rows[i]["name"].ToString();
                        config2 = config2 + ds.Tables[0].Rows[i]["memberId"].ToString() + " = {'parent': M" + parent + ",'innerHTML': \"<a href='#'><div class='card p-1 bg-default text-white text-center' title='Empty Position' style='margin-bottom:0px;'><div class='card-body' style='display:grid;text-align:center;padding: 10px 5px;place-items: center;'><img src='//assets/img/logo.png' alt='User-Image' style='width: 100px;height: 100px;border-radius: 50%;object-fit: cover;z-index: 99;box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);border: 3px solid #828389;background-color: white;'><p>Empty</p><p>Position</p></div></div></a>\"},";
                        chart_config2 = chart_config2 + ds.Tables[0].Rows[i]["memberId"].ToString() + ",";
                        continue;
                    }
                    string memLogId = ds.Tables[0].Rows[i]["logId"].ToString()!;
                    bool isActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["isActive"]);
                    config2 = config2 + "M" + ds.Tables[0].Rows[i]["memberId"].ToString() + " = {" + ((ds.Tables[0].Rows[i]["memberId"].ToString() != memberId) ? ("'parent': M" + parent + ", ") : "") + "'collapsed': true, 'innerHTML': \"<div class='card p-1 bg-" + (isActive ? "success" : "danger") + " text-white text-center' title='Referral Id : " + ds.Tables[0].Rows[i]["spoLogId"].ToString() + " &#013;Joinning Date: " + string.Format("{0:dd/MMM/yyyy hh:mm tt}", ds.Tables[0].Rows[i]["reg_date"]) + " &#013;" + (isActive ? ("Upgrade Date : " + string.Format("{0:dd/MMM/yyyy hh:mm tt}", ds.Tables[0].Rows[i]["pos_date"]) + " &#013Upgrade : " + string.Format("{0:0.00}", ds.Tables[0].Rows[i]["package"])) : "Not Upgraded Yet!") + " &#013Total Left Business : " + ds.Tables[0].Rows[i]["lpoints"]?.ToString() + " &#013Total Right Business : " + ds.Tables[0].Rows[i]["rpoints"]?.ToString() + " &#013Total Left Paid Member : " + ds.Tables[0].Rows[i]["plCount"]?.ToString() + " &#013Total Right Paid Member : " + ds.Tables[0].Rows[i]["prCount"]?.ToString() + "' style='margin-bottom:0px;'><div class='card-body pb-3' style='display:grid;text-align:center;padding: 10px 5px;place-items: center;'><img src='/assets/img/logo.png' alt='User-Image' style='width: 100px;height: 100px;border-radius: 50%;object-fit: cover;z-index: 99;;background-color: white;'><a href='Tree-structure?id=" + memLogId + "' style='z-index:99;'><p>" + memLogId + "</p><p>" + ds.Tables[0].Rows[i]["name"].ToString() + "</p></a></div></div>\" },";
                    chart_config2 = chart_config2 + "M" + ds.Tables[0].Rows[i]["memberId"].ToString() + ", ";
                }
            }
            chart_config2 += "]";
            config2 = config2 + chart_config2 + "</script>";
            base.ViewBag.chart_config = config2;
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                base.ViewBag.leftTeam = ds.Tables[1].Rows[0]["TLCount"].ToString();
                base.ViewBag.leftActive = ds.Tables[1].Rows[0]["PLCount"].ToString();
                base.ViewBag.rightTeam = ds.Tables[1].Rows[0]["TRCount"].ToString();
                base.ViewBag.rightActive = ds.Tables[1].Rows[0]["PRCount"].ToString();
            }
            return View();
        }



    }


}
