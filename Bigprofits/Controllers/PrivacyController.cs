using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class PrivacyController(ContextClass context, HomeRepository homeRepository, IWebHostEnvironment webHostEnvironment, CommonMethods commonMethods, IAuditRepository auditRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private string mango = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            mango = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            ViewBag.MemName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        [HttpGet("account/profile")]
        public async Task<IActionResult> Profile()
        {
            var memberInfo = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
            TempData["MemId"] = memberInfo!.MemId;

            if (memberInfo == null)
            {
                return NotFound();
            }

            memberInfo.MemEmail = commonMethods.Decrypt(memberInfo.MemEmail!);
            memberInfo.MemMobile = commonMethods.Decrypt(memberInfo.MemMobile!);
            memberInfo.MemAddress = commonMethods.Decrypt(memberInfo.MemAddress!);
            ViewBag.CList = new SelectList(await homeRepository.CountryList(), "Cid", "Cname");
            ViewBag.SList = new SelectList(await homeRepository.StateList(), "Sid", "Sname");
            return View(memberInfo);
        }

        [HttpPost("account/profile")]
        public async Task<IActionResult> Profile(int id)
        {
            id = Convert.ToInt32(TempData["MemId"]);
            var memberInfo = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
            try
            {
                string memName = Request.Form["MemName"].ToString().Trim();
                string memMobile = Request.Form["MemMobile"].ToString().Trim();
                string memEmail = Request.Form["MemEmail"].ToString().Trim();
                string memAddress = Request.Form["MemAddress"].ToString().Trim().ToLower();

                memberInfo!.MemName = memName;
                memberInfo.MemMobile = commonMethods.Encrypt(memMobile);
                memberInfo.MemEmail = commonMethods.Encrypt(memEmail);

                if (string.IsNullOrEmpty(memAddress))
                {
                    memberInfo.MemAddress = commonMethods.Encrypt(memAddress);
                }

                context.Update(memberInfo);
                await context.SaveChangesAsync();

                await _auditRepository.LogActionAsync($"MEMBER PROFILE UPDATE", 0, mango, $"Member profile updated, member ID : {mango}, updated details - memName : {memName}, memMobile : {memMobile}, memEmail : {memEmail}, memAddress : {memAddress}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                TempData["msg"] = "Your Profile Details Updated Successfully...!";
                return Redirect("/account/profile");
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("account/bank-details")]
        public async Task<IActionResult> BankDetails()
        {
            var memberInfo = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
            memberInfo!.MemAcNo = commonMethods.Decrypt(memberInfo.MemAcNo!);
            return View(memberInfo);
        }

        [HttpPost("account/bank-details")]
        public async Task<IActionResult> BankDetailsPost()
        {
            var memberInfo = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();

            string accountNo = Request.Form["MemAcNo"].ToString().Trim();
            string ifsc = Request.Form["MemAcIfsc"].ToString().Trim();
            string accountName = Request.Form["MemAcName"].ToString().Trim();

            memberInfo!.MemAcNo = commonMethods.Encrypt(accountNo);
            memberInfo.MemAcIfsc = ifsc;
            memberInfo.MemAcName = accountName;

            context.Update(memberInfo);
            await context.SaveChangesAsync();

            await _auditRepository.LogActionAsync($"MEMBER BANK DETAILS UPDATE", 0, mango, $"Member bank details updated, member ID : {mango}, updated details - MemAcNo : {accountNo}, MemAcIfsc : {ifsc}, MemAcName : {accountName}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

            return View(memberInfo);
        }

        [HttpGet("account/Image")]
        public async Task<IActionResult> ProfileImageUpload()
        {
            var memberInfo = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
            return View(memberInfo);
        }

        [HttpGet("account/reset-password")]
        public async Task<IActionResult> ChangeLoginPassword()
        {
            var memberInfo = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
            return View(memberInfo);
        }

        [HttpPost("account/reset-password")]
        public async Task<JsonResult> IsPasswordChanged(string op, string np, string cp)
        {
            var res = "Old Password Does't Match";
            if (np != cp)
            {
                res = "New And Confirm password doesn't match";
                return Json(res);
            }

            var data = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
            if (data!.MemLogPass == commonMethods.Encrypt(op))
            {
                data.MemLogPass = commonMethods.Encrypt(np);

                context.Update(data);
                context.SaveChanges();

                res = "Password Changed Successfull..!";

                await _auditRepository.LogActionAsync($"MEMBER RESET PASSWORD SUCCESS", 0, mango, $"Member reset password success, member ID : {mango}, old password : {op}, new password {np}, confirm password : {cp}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                return Json(res);
            }

            await _auditRepository.LogActionAsync($"MEMBER RESET PASSWORD FAILED", 0, mango, $"Member reset password failed, member ID : {mango}, current password : {commonMethods.Decrypt(data.MemLogPass!)}, entered current password : {op}, new password {np}, confirm password : {cp}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);
            return Json(res);
        }

        [HttpPost("account/MemberIdExist/{idMember}")]
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

        [HttpPost("account/Image/{imgPath}/{IFormFile}")]
        private async Task<string> ChangeImage(string imgPath, IFormFile file)
        {
            string? Mdata = "";
            if (TempData["memberid"] != null)
            {
                Mdata = TempData["memberid"] as string;

                TempData.Keep();
            }

            imgPath += Mdata + "_" + file.FileName;
            string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, imgPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + imgPath;
        }



    }
}
