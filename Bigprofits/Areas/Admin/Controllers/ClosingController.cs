using Bigprofits.Data;
using Bigprofits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using Bigprofits.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class ClosingController(ContextClass context, CommonMethods commonMethods, SqlConnectionClass dataAccess, IWebHostEnvironment webHostEnvironment, IAuditRepository auditRepository, HomeRepository homeRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private readonly HomeRepository homeRepository = homeRepository;
        private string admingo = "";
        public override void OnActionExecuting(ActionExecutingContext _context)
        {
            admingo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = context.TableSupports.Where(x => x.MyStatus == 0 && x.ToBy == "ADMIN").ToList();
            ViewBag.msgCount = msg.Count;
        }

        [HttpGet("daily-closing/{type}")]
        public async Task<IActionResult> DailyClosing(string type)
        {
            ViewBag.type = type;
            var lastclo = await context.UserMasters.Take(1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            ViewBag.clo = lastclo == null ? "No One Get This Income Till Now" : lastclo.DailyClosing.ToString();

            var rate = await context.UserMasters.FirstOrDefaultAsync();
            ViewBag.rate = string.Format("{0:0.00}", rate!.TokenPrice);

            return View();
        }

        [HttpPost("daily-closing/{type}")]
        public async Task<IActionResult> DailyClosingPost(string type)
        {
            try
            {
                decimal rate = Convert.ToDecimal(Request.Form["rate"].ToString().Trim());
                if (rate <= 0)
                {
                    TempData["msg"] = "Please enter valid ROI percentage.";
                    return Redirect($"/britglbl253adpnl/Daily-Closing/{type}");
                }

                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@type", type));
                par.Add(new SqlParameter("@rate", rate));
                var ds = await _dataAccess.FnRetriveByPro("[SP_Closing]", par);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    await _auditRepository.LogActionAsync($"{type} CLOSING BY ADMIN", 0, "Admin", $"{type} closing by admin", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                    TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    return Redirect($"/britglbl253adpnl/Daily-Closing/{type}");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return Redirect($"/britglbl253adpnl/Daily-Closing/{type}");
        }

        [HttpGet("add-news")]
        public IActionResult AddNews()
        {
            return View();
        }

        [HttpPost("add-news")]
        public async Task<IActionResult> AddNews(TblNews model)
        {
            await context.AddAsync(model);
            await context.SaveChangesAsync();
            TempData["nwadd"] = "News Added Successfully..!";

            await _auditRepository.LogActionAsync($"NEWS ADD BY ADMIN", 0, "Admin", $"A new news added by admin.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

            return RedirectToAction("AddNews");
        }

        [HttpGet("news-list")]
        public async Task<IActionResult> NewsList()
        {
            var data = await context.TblNews.ToListAsync();
            return View(data);
        }

        [HttpGet("news-update")]
        public async Task<IActionResult> NewsUpdate(int newsid, int deleteid)
        {
            if (deleteid > 0)
            {
                var result = await context.TblNews.Where(x => x.NewsId == deleteid).FirstOrDefaultAsync();
                if (result != null)
                {
                    await _auditRepository.LogActionAsync($"NEWS DELETE BY ADMIN", 0, "Admin", $"A news is deleted by admin, news ID is {result.NewsId}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                    context.Remove(result);
                    await context.SaveChangesAsync();
                    TempData["nwupdate"] = "News Deleted Successfully..!";

                    return RedirectToAction("NewsList");
                }
            }
            else
            {
                TempData["nw"] = newsid;
                var data = await context.TblNews.Where(x => x.NewsId == newsid).FirstOrDefaultAsync();

                return View(data);
            }
            return View();
        }

        [HttpPost("news-update")]
        public async Task<IActionResult> NewsUpdate(TblNews model)
        {
            var nid = Convert.ToInt32(TempData["nw"]!.ToString());
            var data = await context.TblNews.Where(x => x.NewsId == nid).FirstOrDefaultAsync();
            if (data != null)
            {
                data.News = model.News;
                data.NewsDate = model.NewsDate;

                context.Update(data);
                await context.SaveChangesAsync();

                await _auditRepository.LogActionAsync($"NEWS UPDATE BY ADMIN", 0, "Admin", $"A news is updated by admin, news ID is {data.NewsId}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                ViewBag.nwmsg = "News updated Successfully..!";
                return View();
            }
            ViewBag.nwmsg = "Something went wrong..!";
            return View();
        }

        [HttpGet]
        [Route("support-system")]
        public async Task<IActionResult> Support(int? page, string? userId, string? date, string? date1)
        {
            ViewBag.userId = userId;
            ViewBag.date = date;
            ViewBag.date1 = date1;

            List<SqlParameter> par = [];
            if (page != null && page > 0) par.Add(new SqlParameter("@index", page > 0 ? page : 0));
            if (userId != null) par.Add(new SqlParameter("@memberId", userId));
            if (date != null) par.Add(new SqlParameter("@fromDate", date));
            if (date1 != null) par.Add(new SqlParameter("@toDate", date1));

            par.Add(new SqlParameter("@status", "0"));
            par.Add(new SqlParameter("@rtype", "SUPPORT REQUEST"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                string url = $"/britglbl253adpnl/support-system?";
                if (userId != null) url += $"userId={userId}&";
                if (date != null) url += $"date={date}&";
                if (date1 != null) url += $"date={date1}&";
                url += $"page=";

                ViewBag.pgnHtml = homeRepository.GetPaginationBtn(url, Convert.ToInt32(ds.Tables[1].Rows[0]["size"]), Convert.ToInt32(ds.Tables[1].Rows[0]["rCount"]), (int)(page == null ? 0 : page));
            }
            return View(ds);
        }

        [HttpGet]
        [Route("view-message/{id}")]
        public async Task<IActionResult> SupportView(int id)
        {
            ViewBag.id = id;

            var data = await context.TableSupports.Where(x => x.Srno == id && x.MyStatus == 0).FirstOrDefaultAsync();
            if (data != null)
            {
                data.MyStatus = 1;
                await context.SaveChangesAsync();

                await _auditRepository.LogActionAsync($"MESSAGE SEEN BY ADMIN", 0, "Admin", $"A support message seen by admin, message ID is {data.Srno} and memberId is {data.FromBy}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);
            }

            List<SqlParameter> par = [new SqlParameter("@memberId", id), new SqlParameter("@rtype", "INBOX VIEW")];
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            return View(ds);
        }

        [HttpPost]
        [Route("send-reply/{id}")]
        public async Task<IActionResult> SaveReply(int id)
        {
            string msg = Request.Form["txtReply"].ToString();
            if (msg == "")
            {
                TempData["error"] = "Please enter a reply first.";
                return Redirect($"/britglbl253adpnl/view-message/{id}");
            }

            var data = await context.TableSupports.Where(x => x.Srno == id).FirstOrDefaultAsync();
            if (data != null)
            {
                var reply = new TableSupport
                {
                    MySubject = data.MySubject,
                    MyDetails = msg,
                    FromBy = "ADIMIN",
                    ToBy = data.FromBy,
                    Sdate = DateTime.Now,
                    MyStatus = 0,
                    Reply = data.MyDetails
                };

                await context.TableSupports.AddAsync(reply);
                await context.SaveChangesAsync();

                await _auditRepository.LogActionAsync($"MESSAGE REPLIED BY ADMIN", 0, "Admin", $"A support message is replied by admin, message ID is {data.Srno}, member ID is {data.FromBy} and reply ID is {reply.Srno}.", HttpContext.Connection.RemoteIpAddress?.ToString()!);

                TempData["success"] = "Success! Reply has been sent successfullY.";
                return Redirect($"/britglbl253adpnl/view-message/{id}");
            }

            TempData["error"] = "Failed! Could not find the message. Please try again.";
            return Redirect($"/britglbl253adpnl/view-message/{id}");
        }

        [HttpGet]
        [Route("outbox")]
        public async Task<IActionResult> Oubox(int? page, string? userId, string? date, string? date1)
        {
            ViewBag.userId = userId;
            ViewBag.date = date;
            ViewBag.date1 = date1;

            List<SqlParameter> par = [];
            if (page != null && page > 0) par.Add(new SqlParameter("@index", page > 0 ? page : 0));
            if (userId != null) par.Add(new SqlParameter("@memberId", userId));
            if (date != null) par.Add(new SqlParameter("@fromDate", date));
            if (date1 != null) par.Add(new SqlParameter("@toDate", date1));

            par.Add(new SqlParameter("@rtype", "OUTBOX"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                string url = $"/britglbl253adpnl/outbox?";
                if (userId != null) url += $"userId={userId}&";
                if (date != null) url += $"date={date}&";
                if (date1 != null) url += $"date={date1}&";
                url += $"page=";

                ViewBag.pgnHtml = homeRepository.GetPaginationBtn(url, Convert.ToInt32(ds.Tables[1].Rows[0]["size"]), Convert.ToInt32(ds.Tables[1].Rows[0]["rCount"]), (int)(page == null ? 0 : page));
            }
            return View(ds);
        }

        [HttpGet]
        [Route("inbox-history")]
        public async Task<IActionResult> InboxHistory(int? page, string? userId, string? date, string? date1)
        {
            try
            {
                ViewBag.userId = userId;
                ViewBag.date = date;
                ViewBag.date1 = date1;

                List<SqlParameter> par = [];
                if (page != null && page > 0) par.Add(new SqlParameter("@index", page > 0 ? page : 0));
                if (userId != null) par.Add(new SqlParameter("@memberId", userId));
                if (date != null) par.Add(new SqlParameter("@fromDate", date));
                if (date1 != null) par.Add(new SqlParameter("@toDate", date1));

                par.Add(new SqlParameter("@status", "1"));
                par.Add(new SqlParameter("@rtype", "SUPPORT REQUEST"));
                var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
                ViewBag.data = ds;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string url = $"/britglbl253adpnl/inbox-history?";
                    if (userId != null) url += $"userId={userId}&";
                    if (date != null) url += $"date={date}&";
                    if (date1 != null) url += $"date={date1}&";
                    url += $"page=";

                    ViewBag.pgnHtml = homeRepository.GetPaginationBtn(url, Convert.ToInt32(ds.Tables[1].Rows[0]["size"]), Convert.ToInt32(ds.Tables[1].Rows[0]["rCount"]), (int)(page == null ? 0 : page));
                }
                return View(ds);
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return RedirectToAction("InboxHistory");
        }

        [HttpGet("create-poster")]
        public IActionResult Poster()
        {
            return View();
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("create-poster")]
        public async Task<IActionResult> Poster(Poster model)
        {
            try
            {
                IFormFile file = model.PosteImage!;
                string imgPath = "Uploads/ProductsImages/";
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string randomId = GenerateRandomString(10);
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        base.TempData["error"] = "Only jpeg, jpg, and png supported";
                        return Redirect("create-poster");
                    }
                    model.PosterImage = CommonRepository.UploadImage(imgPath, file, webHostEnvironment.WebRootPath, randomId);
                }

                model.Posterdate = DateTime.Now;
                model.PosterStatus = 1;
                context.Posters.Add(model);
                await context.SaveChangesAsync();
                base.TempData["success"] = "QR added successfully";
            }
            catch (Exception ex)
            {
                base.TempData["ExceptionError"] = "An error occurred: " + ex.Message;
                return View();
            }
            return Redirect("create-poster");
        }

        [HttpGet("poster-history")]
        public async Task<IActionResult> PosterList()
        {
            List<Poster> data = await context.Posters.OrderBy((Poster x) => x.Id).ToListAsync();
            base.ViewBag.Model = JsonConvert.SerializeObject(data);
            return View(data);
        }

        [HttpPost("poster-delete")]
        public async Task<IActionResult> PosterDelete(int id)
        {
            var poster = await context.Posters.FindAsync(id);
            if (poster == null)
            {
                return Json(new
                {
                    success = false,
                    message = "QR not found."
                });
            }
            context.Posters.Remove(poster);
            await context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = "QR deleted successfully!"
            });
        }

        [HttpGet("poster-update/{id}")]
        public async Task<IActionResult> PosterUpdate(int id)
        {
            var data = await context.Posters.FindAsync(id);
            if (data == null)
            {
                base.TempData["err"] = "Poster not found.";
                return RedirectToAction("poster-history");
            }
            return View(data);
        }

        [HttpPost("poster-update")]
        public async Task<IActionResult> PosterUpdate(Poster model)
        {
            var data = await context.Posters.FindAsync(model.Id);
            IFormFile file = model.PosteImage!;
            string imgPath = "Uploads/ProductsImages/";
            if (file == null)
            {
                return Redirect("create-poster");
            }
            string extension = Path.GetExtension(file.FileName);
            string randomId = GenerateRandomString(10);
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                base.TempData["error"] = "Only jpeg, jpg, and png supported";
                return Redirect("create-poster");
            }
            model.PosterImage = CommonRepository.UploadImage(imgPath, file, webHostEnvironment.WebRootPath, randomId);
            data!.Postername = model.Postername;
            data.PosterContent = model.PosterContent;
            context.Posters.Update(data);
            await context.SaveChangesAsync();
            base.TempData["success"] = "Poster updated successfully!";
            return RedirectToAction("poster-update", new
            {
                id = model.Id
            });
        }



    }
}
