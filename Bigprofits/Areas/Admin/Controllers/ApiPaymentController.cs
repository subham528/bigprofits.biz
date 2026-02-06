using Bigprofits.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class ApiPaymentController(ContextClass context, IConfiguration configuration, CommonMethods commonMethods, SqlConnectionClass dataAccess) : Controller
    {
        private readonly ContextClass context = context;
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

        [HttpGet("AdminFund-Request")]
        public async Task<IActionResult> AdminFundRequest()
        {
            List<SqlParameter> par = [];
            var test = await _dataAccess.FnRetriveByPro("[SpApiFundDetail]", par);
            ViewBag.totalFundReq = test.Tables[0].Rows[0]["totalFundReq"];
            ViewBag.approvedFund = test.Tables[0].Rows[0]["approvedFund"];
            ViewBag.pendingFund = test.Tables[0].Rows[0]["pendingFund"];
            ViewBag.withdrawalAmount = test.Tables[0].Rows[0]["withdrawalAmount"];
            ViewBag.apiCharge = test.Tables[0].Rows[0]["apiCharge"];
            ViewBag.discount = test.Tables[0].Rows[0]["discount"];
            ViewBag.available = test.Tables[0].Rows[0]["available"];

            if (TempData["code"] != null) ViewBag.msg = TempData["code"];
            if (TempData["msg"] != null) ViewBag.msg = TempData["msg"];
            return View();
        }

        [HttpPost("AdminFund-Request")]
        public async Task<IActionResult> AdminFundRequest(TblApiPaymentReq payReq)
        {
            try
            {
                if (payReq.ReqAmount < 10)
                {
                    TempData["code"] = 0;
                    TempData["msg"] = "Amount should be minimum 10";
                }
                else
                {
                    List<SqlParameter> par = [];

                    par.Add(new SqlParameter("@reqAmount", payReq.ReqAmount));
                    par.Add(new SqlParameter("@aType", "INSERT"));
                    par.Add(new SqlParameter("@description", payReq.Admindescription ?? ""));
                    par.Add(new SqlParameter("@memberid", admingo));
                    var test = await _dataAccess.FnRetriveByPro("[SpApiPaymentReq]", par);

                    string result = test.Tables[0].Rows[0]["Msg"].ToString()!;

                    TempData["code"] = 1;
                    TempData["msg"] = "Fund Requested of " + payReq.ReqAmount + " is Successful. ";
                }
                return RedirectToAction("AdminFundRequest", "ApiPayment");
            }
            catch (Exception ex)
            {
                TempData["code"] = 0;
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("AdminFundRequest", "ApiPayment");
        }

        [HttpGet("AFund-History")]
        public async Task<IActionResult> AdminFundHistory()
        {
            List<SqlParameter> par = [];
            var adminfund = await _dataAccess.FnRetriveByPro("[SpApiFundDetail]", par);
            ViewBag.adminfund = adminfund.Tables[0];

            var data = await context.TblApiPaymentReqs.ToListAsync();
            return View(data);
        }

        [HttpGet("Approvepayreq")]
        public async Task<IActionResult> Approvepayreq()
        {

            List<SqlParameter> par = [];
            var adminfund = await _dataAccess.FnRetriveByPro("[SpApiFundDetail]", par);

            ViewBag.adminfund = adminfund.Tables[0];
            ViewBag.totalFundReq = adminfund.Tables[0].Rows[0]["totalFundReq"];
            ViewBag.approvedFund = adminfund.Tables[0].Rows[0]["approvedFund"];
            ViewBag.pendingFund = adminfund.Tables[0].Rows[0]["pendingFund"];
            ViewBag.withdrawalAmount = adminfund.Tables[0].Rows[0]["withdrawalAmount"];
            ViewBag.apiCharge = adminfund.Tables[0].Rows[0]["apiCharge"];
            ViewBag.discount = adminfund.Tables[0].Rows[0]["discount"];
            ViewBag.available = adminfund.Tables[0].Rows[0]["available"];

            var data = await context.TblApiPaymentReqs.ToListAsync();

            if (TempData["code"] != null)
            {
                ViewBag.code = TempData["code"];
                ViewBag.msg = TempData["msg"];
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ApiFundRequestApprove(int? fundid, int? id)
        {
            var data = await context.TblApiPaymentReqs.Where(x => x.Id == fundid).FirstOrDefaultAsync();
            if (id == 1)
            {
                TempData["code"] = 1;
                TempData["msg"] = "Request Approved";
                data!.ReqStatus = 1;
                data.ApproveDate = DateTime.Now;
                context.Update(data);
                await context.SaveChangesAsync();
            }
            else if (id == 0)
            {
                TempData["code"] = 1;
                TempData["msg"] = "Request Rejected";
                data!.ReqStatus = 2;
                data.ApproveDate = DateTime.Now;
                context.Update(data);
                await context.SaveChangesAsync();
            }
            else
            {
                TempData["code"] = 1;
                TempData["msg"] = "Something Went Wrong";
            }
            return RedirectToAction("Approvepayreq");
        }

        [HttpGet("Order-History")]
        public async Task<IActionResult> OrderHistory()
        {
            var data = await context.OnlineServices.OrderByDescending(t => t.Memid).ToListAsync();

            if (TempData["code"] != null)
            {
                ViewBag.code = TempData["code"];
                ViewBag.msg = TempData["msg"];
            }

            return View(data);
        }

        [HttpGet("Order-Details")]
        public async Task<IActionResult> OrderDetails(string txnId)
        {
            try
            {
                string orderId = txnId;
                ViewBag.orderId = orderId;

                var data = await context.OnlineServices.Where(x => x.OurTxnNo == txnId).FirstOrDefaultAsync();
                if (data != null)
                {
                    if (data.OprtnTime == "INSTANTPAY")
                    {
                        //var options = new RestClientOptions("https://api.instantpay.in");
                        //var client = new RestClient(options);
                        //var request = new RestRequest("/reports/txnStatus", Method.Post);
                        //request.AddHeader("X-Ipay-Auth-Code", "1");
                        //request.AddHeader("X-Ipay-Client-Id", _configuration["INSP:client_id"]!);
                        //request.AddHeader("X-Ipay-Client-Secret", _configuration["INSP:client_secret"]!);
                        //request.AddHeader("X-Ipay-Endpoint-Ip", _configuration["INSP:ip"]!);
                        //request.AddHeader("Content-Type", "application/json");
                        //var body = new
                        //{
                        //    transactionDate = string.Format("{0:yyyy-MM-dd}", data.Odate),
                        //    externalRef = orderId
                        //};
                        //string jsonBody = JsonConvert.SerializeObject(body);
                        //request.AddStringBody(jsonBody, DataFormat.Json);
                        //RestResponse response = client.Execute(request);
                        ////Console.WriteLine(response.Content);

                        //var StatusList = JsonConvert.DeserializeObject(response.Content!) as JObject;
                        //if (StatusList != null && StatusList["statuscode"] != null)
                        //{
                        //    string ResStatus = StatusList["statuscode"]!.ToString();
                        //    string status = ResStatus == "TXN" ? "SUCCESS" : ResStatus == "TUP" ? "PENDING" : "FAILED";

                        //    string bg = status == "PENDING" ? "warning" : status == "SUCCESS" ? "success" : "danger";
                        //    string icon = status == "PENDING" ? "refresh" : status == "SUCCESS" ? "check" : "times";

                        //    string orderInfo = "<button type='button' class='btn btn-" + bg + "'><span class='badge badge-light text-dark'><i class='fa fa-" + icon + "'></i></span> " + status + "</button>";
                        //    orderInfo += "<table class='table table-borderless mt-3 text-left'>" +
                        //                           "<tr><td>Service : </td><td>" + StatusList["data"]!["order"]!["spName"]!.ToString() + "</td></tr>" +
                        //                           "<tr><td>Amount : </td><td>" + StatusList["data"]!["transactionAmount"]!.ToString() + "</td></tr>" +
                        //                           "<tr><td>Number : </td><td>" + StatusList["data"]!["order"]!["account"]!.ToString() + "</td></tr>" +
                        //                           "<tr><td>Status : </td><td>" + StatusList["data"]!["transactionStatus"]!.ToString() + "</td></tr>" +
                        //                           "<tr><td>Date : </td><td>" + StatusList["data"]!["transactionDateTime"]!.ToString() + "</td></tr></table>";
                        //    ViewBag.orderInfo = orderInfo;

                        //    if (data.Status == "PENDING" && status != "PENDING")
                        //    {
                        //        if (status == "SUCCESS" || status == "FAILED")
                        //        {
                        //            int isPaid = status == "SUCCESS" ? 1 : 0;
                        //            data.Status = status;
                        //            if (isPaid == 1)
                        //            {
                        //                data.IsPaid = 1;
                        //            }
                        //            else
                        //            {
                        //                data.IsPaid = 0;
                        //            }
                        //            await context.SaveChangesAsync();
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception)
            {
            }
            return View();
        }

        public Dictionary<string, object> SignatureGeneration()
        {
            try
            {
                string key = _configuration["Eko:AuthKey"]!;

                string encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
                long secretKeyTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                byte[] encodedKeyBytes = Encoding.UTF8.GetBytes(encodedKey);
                byte[] timestampBytes = Encoding.UTF8.GetBytes(secretKeyTimestamp.ToString());

                byte[] signatureBytes;
                using (var hmac = new HMACSHA256(encodedKeyBytes))
                {
                    signatureBytes = hmac.ComputeHash(timestampBytes);
                }
                string secretKey = Convert.ToBase64String(signatureBytes);

                return new Dictionary<string, object>
                {
                    { "SecretKey", secretKey },
                    { "SecretKeyTimestamp", secretKeyTimestamp }
                };
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "error", ex.Message }
                };
            }
        }



    }
}
