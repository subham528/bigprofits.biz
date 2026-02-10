using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("admin/[controller]/[action]")]
    public class AdminController(ContextClass context, CommonMethods commonMethods, SqlConnectionClass dataAccess, HomeRepository homeRepository, IAuditRepository auditRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly IAuditRepository _auditRepository = auditRepository;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            TempData["ProjectName"] = Global.projectName;
        }

        [HttpGet("sign-in")]
        public IActionResult AdLog(string? ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> AdLog(UserMaster model)
        {
            var data = await context.UserMasters.Where(x => x.UserId == commonMethods.Encrypt(model.UserId!.Trim()) && x.UserPass == commonMethods.Encrypt(model.UserPass!.Trim())).FirstOrDefaultAsync();
            if (data != null)
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, commonMethods.Decrypt(data.UserId!)),
                    new (ClaimTypes.NameIdentifier, commonMethods.Decrypt(data.UserId!))
                };

                var identity = new ClaimsIdentity(claims, "AdminAuth");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };
                await HttpContext.SignInAsync("AdminAuth", principal, authProperties);

                await _auditRepository.LogActionAsync($"ADMIN LOGIN SUCCESSFULL", 0, "Admin", $"Admin login successfull, enter login ID is {model.UserId!.Trim()} and login password is {model.UserPass!.Trim()}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                return Redirect(TempData["ReturnUrl"] == null ? "/britglbl253adpnl/home" : TempData["ReturnUrl"]!.ToString()!);
            }

            await _auditRepository.LogActionAsync($"ADMIN LOGIN FAILED", 0, "Admin", $"Admin login failed, enter login ID is {model.UserId!.Trim()} and login password is {model.UserPass!.Trim()}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

            TempData["error"] = "Invalid id Or Password";
            return Redirect("/britglbl253adpnl/sign-in");
        }

        [HttpGet("admin-signout")]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync("AdminAuth");

            await _auditRepository.LogActionAsync($"ADMIN LOGOUT", 0, "Admin", $"Admin successfully logout.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

            return Redirect("/britglbl253adpnl/sign-in");
        }

        [HttpGet("reward-closing")]
        public IActionResult Rewardclosing()
        {
            return View();
        }

        [HttpPost("encrypt-data")]
        public async Task<IActionResult> Encrypt(string Edata)
        {
            try
            {
                TempData["msg"] = "";
                Edata ??= string.Empty;

                var edata = commonMethods.Encrypt(Edata);

                await _auditRepository.LogActionAsync($"TEXT ENCRYPTED", 0, "Admin", $"Some text encrypted by developer/admin/someone.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                TempData["EncryptedData"] = edata;
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return Redirect("/britglbl253adpnl/reward-closing");
        }

        [HttpPost("decrypt-data")]
        public async Task<IActionResult> Decrypt(string EncryptedData)
        {
            try
            {
                TempData["msg"] = "";
                var ddata = commonMethods.Decrypt(EncryptedData.Trim());

                await _auditRepository.LogActionAsync($"TEXT DECRYPTED", 0, "Admin", $"Some text decrypted by developer/admin/someone.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                TempData["DecryptedData"] = ddata;
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return Redirect("/britglbl253adpnl/reward-closing");
        }

        [HttpGet("topup/system-topup")]
        public async Task<IActionResult> TopupDetails(string? date)
        {
            ViewBag.date = date;
            List<SqlParameter> par = [];
            if (date != null) par.Add(new SqlParameter("@date", date));
            var ds = await _dataAccess.FnRetriveByPro("[SP_AdminInfo]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            return View();
        }


    }
}
