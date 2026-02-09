using Bigprofits.Data;
using Bigprofits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Bigprofits.Repository;
using Microsoft.CodeAnalysis;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("britglbl253adpnl/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AMemberController(ContextClass context, CommonMethods commonMethods, SqlConnectionClass dataAccess, HomeRepository homeRepository, IAuditRepository auditRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly IAuditRepository _auditRepository = auditRepository;

        private string admingo = "";
        public override void OnActionExecuting(ActionExecutingContext _context)
        {
            admingo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = context.TableSupports.Where(x => x.MyStatus == 0 && x.ToBy == "ADMIN").ToList();
            ViewBag.msgCount = msg.Count;
        }

        private static readonly string[] encryptedFields = ["address", "mobile", "email"];

        [HttpGet]
        [Route("member-list")]
        public async Task<IActionResult> MemberSearch(int? page, string? active, string? type, string? value)
        {
            ViewBag.type = type;
            ViewBag.value = value;
            ViewBag.active = active;

            List<SqlParameter> par = [];
            if (page != null && page > 0) par.Add(new SqlParameter("@index", page > 0 ? page : 0));
            if (type != null && value != null)
            {
                if (encryptedFields.Contains(type)) par.Add(new SqlParameter($"@{type}", commonMethods.Encrypt(value)));
                else par.Add(new SqlParameter($"@{type}", value));
            }
            if (active != null) par.Add(new SqlParameter("@activeStatus", active));

            par.Add(new SqlParameter("@rank", "1"));
            var ds = await _dataAccess.FnRetriveByPro("[memberInfoSelect]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string url = $"/britglbl253adpnl/member-list?";
                if (active != null) url += $"active={active}&";
                if (type != null) url += $"type={type}&";
                if (value != null) url += $"value={value}&";
                url += $"page=";

                ViewBag.pgnHtml = homeRepository.GetPaginationBtn(url, Convert.ToInt32(ds.Tables[1].Rows[0]["size"]), Convert.ToInt32(ds.Tables[1].Rows[0]["rCount"]), (int)(page == null ? 0 : page));
            }
            return View(ds);
        }

        [HttpPost]
        [Route("member-block")]
        public async Task<JsonResult> Payall(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                List<SqlParameter> par =
                [
                    new SqlParameter("@memberId", id),
                    new SqlParameter("@type", "BLOCK")
                ];
                await _dataAccess.FnRetriveByPro("[SP_Action]", par);

                await _auditRepository.LogActionAsync($"USER BLOCKED BY ADMIN", 0, "Admin", $"User blocked by admin, memberid is {id}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                TempData["msg"] = "Member Blocked";
                return Json("");
            }
            else
            {
                TempData["msg"] = "Please select a member to block.";
                return Json("");
            }
        }

        [HttpPost]
        [Route("member-Unblock")]
        public async Task<JsonResult> Payalll(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                List<SqlParameter> par =
                [
                    new SqlParameter("@memberId", id),
                    new SqlParameter("@type", "UNBLOCK")
                ];
                await _dataAccess.FnRetriveByPro("[SP_Action]", par);

                await _auditRepository.LogActionAsync($"USER UNBLOCKED BY ADMIN", 0, "Admin", $"User unblocked by admin, memberid is {id}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                TempData["msg"] = "Member Blocked";
                return Json("");
            }
            else
            {
                TempData["msg"] = "Please select a member to block.";
                return Json("");
            }
        }

        [HttpGet("Member-Income/{MemId}")]
        public async Task<IActionResult> MemberDashboard(int MemId)
        {
            var data = await context.MemberInfos.Where(x => x.MemId == MemId).FirstOrDefaultAsync();
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
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                TempData["ermsg"] = "Something went wrong";
                return RedirectToAction("AMember", "MemberSearch");
            }
        }

        [HttpGet("member-update/{id}")]
        public async Task<IActionResult> MemberUpdate(string id)
        {
            var data = await context.MemberInfos.Where(x => x.MemberId == id || x.MemId == Convert.ToInt32(id)).FirstOrDefaultAsync();
            if (data != null)
            {
                data.MemMobile = commonMethods.Decrypt(data.MemMobile!);
                data.MemAddress = commonMethods.Decrypt(data.MemAddress!);
                data.MemEmail = commonMethods.Decrypt(data.MemEmail!);
                data.MemLogPass = commonMethods.Decrypt(data.MemLogPass!);
              
                return View(data);
            }
            return View(data);
        }

        [HttpPost("member-update")]
        public async Task<IActionResult> MemberUpdate(MemberInfo info)
        {
            try
            {
                string preMsg = "";
                string memberId = info.MemberId!.ToString();

                var memberInfo = context.MemberInfos.Where(t => t.MemberId == memberId || t.MemLogId == memberId).SingleOrDefault();
                if (memberInfo != null)
                {
                    List<SqlParameter> par = [];

                    par.Add(new SqlParameter("@memberId", memberInfo.MemberId));
                    var ds = await _dataAccess.FnRetriveByPro("[SP_MemberInfoUpdate]", par);

                    //memberInfo.MemName = info.MemName?.Trim();
                    //memberInfo.MemMobile = commonMethods.Encrypt(info.MemMobile?.Trim() ?? "");
                    //memberInfo.MemEmail = commonMethods.Encrypt(info.MemEmail?.Trim() ?? "");
                    //memberInfo.MemState = info.MemState;
                    memberInfo.MemAddress = commonMethods.Encrypt(info.MemAddress?.Trim().ToLower() ?? "");
                    
                    //memberInfo.MemLogPass = commonMethods.Encrypt(info.MemLogPass?.Trim() ?? "");

                    context.Update(memberInfo);
                    await context.SaveChangesAsync();

                    await _auditRepository.LogActionAsync($"USER INFO UPDATE BY ADMIN", 0, "Admin", $"User update by admin, memberid is {memberId}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                    TempData["msg"] = preMsg + "Detail successfully updated";
                    return Redirect($"/britglbl253adpnl/Member-Update/{info.MemberId}");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
                return Redirect($"/britglbl253adpnl/Member-Update/{info.MemberId}");
            }
            return Redirect($"/britglbl253adpnl/Member-Update/{info.MemberId}");
        }

        [HttpGet]
        [Route("member-list-block")]
        public async Task<IActionResult> BlockMember(int? page, string? active, string? type, string? value)
        {
            ViewBag.type = type;
            ViewBag.value = value;
            ViewBag.active = active;

            List<SqlParameter> par = [];
            if (page != null && page > 0) par.Add(new SqlParameter("@index", page > 0 ? page : 0));
            if (type != null && value != null) par.Add(new SqlParameter($"@{type}", value));
            if (active != null) par.Add(new SqlParameter("@activeStatus", active));

            par.Add(new SqlParameter("@rank", "2"));
            var ds = await _dataAccess.FnRetriveByPro("[memberInfoSelect]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string url = $"/britglbl253adpnl/member-list-block?";
                if (active != null) url += $"active={active}&";
                if (type != null) url += $"type={type}&";
                if (value != null) url += $"value={value}&";
                url += $"page=";

                ViewBag.pgnHtml = homeRepository.GetPaginationBtn(url, Convert.ToInt32(ds.Tables[1].Rows[0]["size"]), Convert.ToInt32(ds.Tables[1].Rows[0]["rCount"]), (int)(page == null ? 0 : page));
            }
            return View(ds);
        }

        [HttpGet("list-Update")]
        public async Task<IActionResult> UpdateHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "MEMBER UPDATE HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("update-token")]
        public async Task<IActionResult> UpdateToken()
        {
            var user = await context.UserMasters.FirstOrDefaultAsync();
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost("update-token")] 
        public async Task<IActionResult> UpdateToken(UserMaster model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await context.UserMasters.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return View(model);
            }

            user.TokenPrice = model.TokenPrice;
            await context.SaveChangesAsync();

            TempData["Success"] = "Token price updated successfully!";
            return Redirect("/britglbl253adpnl/update-token");
        }



    }
}
