using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using Bigprofits.Repository;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Bigprofits.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuth")]
    public class WalletController(ContextClass context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, CommonMethods commonMethods, SqlConnectionClass dataAccess, HomeRepository homeRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private readonly IConfiguration _configuration = configuration;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly HomeRepository homeRepository = homeRepository;
        private string mango = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            mango = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            ViewBag.MemName = User.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        [HttpGet("account/add-wallet-fund")]
        public async Task<IActionResult> AddFund()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "ADD FUND TO WALLET"));
            ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpGet("account/Swap")]
        public async Task<IActionResult> SwapingWallet()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "ADD FUND TO WALLET Bigprofits"));
            ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpPost("account/stake-hold")]
        public async Task<IActionResult> SwapingWallet(string type, int sid, decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(type) || amount <= 0)
                {
                    TempData["msg"] = "All the fields are required!";
                    return View();
                }

                if (!type.Equals("HOLD", StringComparison.CurrentCultureIgnoreCase) && !type.Equals("STAKE", StringComparison.CurrentCultureIgnoreCase))
                {
                    TempData["msg"] = "Invalid data! Please enter a valid data.";
                    return View();
                }

                if (amount < 10)
                {
                    TempData["msg"] = "Miminum stake amount is 10 USD.";
                    return View();
                }

                var liveRate = await context.UserMasters.Select(x => x.TokenPrice).FirstOrDefaultAsync();
                if (type.Equals("STAKE", StringComparison.CurrentCultureIgnoreCase))
                {
                    var stakeInfo = await context.StakeInfos.FirstOrDefaultAsync(x => x.Id == sid);
                    if (stakeInfo == null)
                    {
                        TempData["msg"] = "Please enter a valid stake plan.";
                        return View();
                    }

                    var stakeAmount = new StakeAmount()
                    {
                        MemberId = mango,
                        StakeId = stakeInfo.Id,
                        Amount = amount,
                        TokenAmount = amount / liveRate,
                        Sstatus = 1,
                        LiveRate = liveRate,
                        Duration = stakeInfo.Duration,
                        Sdate = DateTime.Now,
                        Mdate = DateTime.Now.AddMonths((int)stakeInfo.Duration!),
                        Rate = stakeInfo.Rate,
                        ReturnAmount = (amount / liveRate) * stakeInfo.Rate
                    };
                    await context.StakeAmounts.AddAsync(stakeAmount);
                    await context.SaveChangesAsync();

                    TempData["success"] = "Big Profits token has been staked successfully!";
                    return Redirect("/account/stake-hold");
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync($"{_configuration.GetValue<string>("NodeApiUrl")}/withdrawal");
                        response.EnsureSuccessStatusCode();
                        //Console.WriteLine(await response.Content.ReadAsStringAsync());
                        var statusList = JObject.Parse(await response.Content.ReadAsStringAsync());
                        if (statusList == null)
                        {
                            TempData["msg"] = "Opps! Blockchain API is currently not responding! Please try after sometime.";
                            return Redirect("/account/stake-hold");
                        }
                    }

                    using (var client = new HttpClient())
                    {
                        var payload = new
                        {
                            tokenName = "OCSK"
                        };
                        var response = await client.PostAsJsonAsync($"{_configuration.GetValue<string>("NodeApiUrl")}/fetch-balance", payload);
                        response.EnsureSuccessStatusCode();
                        //Console.WriteLine(await response.Content.ReadAsStringAsync());
                        var statusList = JObject.Parse(await response.Content.ReadAsStringAsync());
                        if (statusList == null)
                        {
                            TempData["msg"] = "Opps! Blockchain API is currently not responding! Please try after sometime.";
                            return Redirect("/account/stake-hold");
                        }

                        if (statusList["balance"] == null)
                        {
                            TempData["msg"] = "Opps! Blockchain API is currently not responding!! Please try after sometime.";
                            return Redirect("/account/stake-hold");
                        }

                        double balance = 0, tokenAmount = ((double)amount / (double)liveRate);
                        balance = Convert.ToDouble(statusList["balance"]);
                        if (balance < tokenAmount)
                        {
                            TempData["msg"] = "Right now, server is not available to process your withdrawal! Please try after sometime.";
                            return Redirect("/account/stake-hold");
                        }

                        var buyToken = new BuyToken()
                        {
                            MemberId = mango,
                            Amount = amount,
                            TokenAmount = amount / liveRate,
                            TokenName = "Big Profits",
                            CRate = liveRate,
                            RDate = DateTime.Now,
                            AStatus = 0
                        };
                        await context.BuyTokens.AddAsync(buyToken);
                        await context.SaveChangesAsync();
                    }

                    var sendToken = await context.BuyTokens.Where(x => x.MemberId == mango && x.AStatus == 0).OrderByDescending(x => x.Id).ToListAsync();
                    using (var client = new HttpClient())
                    {
                        var payload = new
                        {
                            userId = sendToken[0].MemberId,
                            orderId = sendToken[0].Id
                        };
                        var response = await client.PostAsJsonAsync($"{_configuration.GetValue<string>("NodeApiUrl")}/send-token", payload);
                        response.EnsureSuccessStatusCode();
                        //Console.WriteLine(await response.Content.ReadAsStringAsync());
                        var statusList = JObject.Parse(await response.Content.ReadAsStringAsync());
                    }

                    TempData["success"] = "Big Profits token sent to your wallet successfully!";
                    return Redirect("/account/stake-hold");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpPost("account/add-wallet-fund")]
        public async Task<IActionResult> AddFundPost()
        {
            try
            {
                decimal available = 0;
                string amount = Request.Form["amount"].ToString().Trim();
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    available = Convert.ToDecimal(ds.Tables[0].Rows[0]["available"]);
                }

                if (decimal.TryParse(amount.ToString(), out decimal result) == false)
                {
                    TempData["msg"] = "Please enter valid amount..!";
                    return RedirectToAction("AddFund");
                }

                par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@amount", amount));
                par.Add(new SqlParameter("@available", available.ToString()));
                par.Add(new SqlParameter("@actionType", "INSERT"));
                par.Add(new SqlParameter("@type", "ADD FUND"));
                ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);
                ViewBag.data = ds;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    return RedirectToAction("AddFund");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpGet("account/manage-request/{id}")]
        public async Task<IActionResult> ReqApproveOrReject(string id)
        {
            var data = await context.CmitRequests.Where(x => x.CmitId == id && x.CmitStatus == 1 && x.IsActive == 1).FirstOrDefaultAsync();
            if (data == null)
            {
                TempData["success"] = $"Failed! We could not find the topup.";
                return Redirect("/account/topup-history");
            }

            List<SqlParameter> par = [];
            par = [];
            par.Add(new SqlParameter("@memberId", data.CmitId));
            par.Add(new SqlParameter("@typeInt", "1"));
            par.Add(new SqlParameter("@usdPrice", _configuration.GetValue<string>("usdRate")));

            var ds = await _dataAccess.FnRetriveByPro("[Amountinsert]", par);
            TempData["success"] = ds.Tables[0].Rows[0]["msg"].ToString();

            return Redirect("/account/topup-history");
        }

        [HttpGet("account/topup-history")]
        public async Task<IActionResult> MytopUpHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@atype", "MEMBER"));
            par.Add(new SqlParameter("@rtype", "TOPUP HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpGet("account/topup-wallet-add-Drontoken")]
        public async Task<IActionResult> DrontokenRequestFund()
        {
            ViewBag.ds = Global.Dp20address;
            ViewBag.memberid = mango;
            try
            {
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@rtype", "ADD FUND TO WALLET USDT"));
                var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
                ViewBag.data = ds;
                return View(ds);
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpGet("account/topup-wallet-add")]
        public async Task<IActionResult> RequestFund()
        {
            ViewBag.ds = Global.Bp20Address;
            ViewBag.memberid = mango;
            try
            {
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@rtype", "ADD FUND TO WALLET USDT"));
                var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
                ViewBag.data = ds;
                return View(ds);
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpPost("account/topup-wallet-add")]
        public async Task<IActionResult> RequestFundPost()
        {
            string amount = Request.Form["amount"].ToString().Trim();

            if (amount == null)
            {
                TempData["msg"] = "Please enter  amount..!";
                return RedirectToAction("RequestFund");
            }

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@actionType", "CHECK"));
            par.Add(new SqlParameter("@amount", amount));
            par.Add(new SqlParameter("@type", "FUND REQUEST"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);

            if (ds.Tables[0].Rows[0]["chk"].ToString() == "1")
            {
                return Redirect("/account/wallet-add-fund-bep/" + amount);
            }
            else
            {
                TempData["error"] = ds.Tables[0].Rows[0]["msg"].ToString();
            }
            return Redirect("/account/topup-wallet-add");
        }

        [HttpGet]
        [Route("account/wallet-add-fund-bep/{amount}")]
        [Route("account/wallet-add-fund-bep")]
        public async Task<IActionResult> AddfundBp30(string amount)
        {
            ViewBag.amount = amount;
            ViewBag.bp20address = Global.Bp20Address;
            // var data = await context.AdminAddressInfos.Where(t => t.DepositAddress == Global.Bp20Address).FirstOrDefaultAsync();
            var data = await context.AdminAddressInfos.Where(a => a.Ctype == "USDT-BEP-20").FirstOrDefaultAsync();

            var address = commonMethods.Decrypt(data!.DepositAddress!);
            if (address != Global.Bp20Address)
            {
                return RedirectToAction("RequestFund");
            }
            return View();
        }

        [HttpPost("account/wallet-add-fund-bep/{amount}/{hashId}")]
        public async Task<IActionResult> AddfundBp30(double amount, string hashId)
        {
            try
            {
                string cType = "USDT-BEP-20";
                double weiValue = 1000000000000000000;
                string depositAddress = "", tokenAddress = "";

                if (string.IsNullOrEmpty(hashId))
                {
                    TempData["msg"] = "Please enter hash ID/txn ID first!";
                    return RedirectToAction("RequestFund");
                }

                string url = $"https://api.bscscan.com/api?module=proxy&action=eth_getTransactionReceipt&txhash={hashId}&apikey=8V5JN3EH1C7AS1R816K5MCDJEN1B8UCV4F";
                var StatusList = await commonMethods.GetApiData(url);

                var data = await context.AdminAddressInfos.FirstOrDefaultAsync(t => t.Ctype == cType);
                if (data != null)
                {
                    depositAddress = commonMethods.Decrypt(data.DepositAddress!).ToUpper();
                    tokenAddress = data.TokenAddress!.ToUpper();
                }

                // Check for duplicate transactions
                bool isDuplicate = await context.CmitRequests.AnyAsync(t => t.HashId == hashId) || await context.FundAmounts.AnyAsync(t => t.HashId == hashId);

                if (isDuplicate)
                {
                    TempData["msg"] = "Duplicate Transaction Found! We could not complete your request due to transaction duplicacy.";
                    return RedirectToAction("RequestFund");
                }

                if (StatusList["result"] == null || StatusList["result"]!["status"] == null)
                {
                    TempData["msg"] = "Invalid Transaction! We could not find transaction on the blockchain.";
                    return RedirectToAction("RequestFund");
                }

                if (StatusList["result"]!["status"]!.ToString() != "0x1")
                {
                    TempData["msg"] = "Transaction failed! Please try again.";
                    return RedirectToAction("RequestFund");
                }

                var logs = StatusList["result"]!["logs"];
                if (logs == null || !logs.Any() || logs[0]?["topics"]?[2] == null || logs[0]?["data"] == null || logs[0]?["address"] == null)
                {
                    TempData["msg"] = "Invalid Transaction! Log data is missing.";
                    return RedirectToAction("RequestFund");
                }

                string tokenAmount = logs[0]!["data"]!.ToString()[2..];
                tokenAmount = System.Numerics.BigInteger.Parse(tokenAmount, NumberStyles.AllowHexSpecifier).ToString();

                bool isToAddressMatch = logs[0]!["topics"]![2]!.ToString().Contains(depositAddress[2..], StringComparison.CurrentCultureIgnoreCase);

                if (!isToAddressMatch || Convert.ToDouble(tokenAmount) < ((amount - 0.5) * weiValue) || logs[0]!["transactionHash"]!.ToString() != hashId || !logs[0]!["address"]!.ToString().Equals(tokenAddress, StringComparison.CurrentCultureIgnoreCase) || !depositAddress.Equals(Global.Bp20Address, StringComparison.CurrentCultureIgnoreCase))
                {
                    TempData["msg"] = "Invalid Transaction! We could not verify your transaction.";
                    return RedirectToAction("RequestFund");
                }

                var par = new List<SqlParameter>
                {
                    new("@memberId", mango),
                    new("@amount", amount),
                    new("@tokenAmount", tokenAmount),
                    new("@walletType", "USDT (BEP-20)"),
                    new("@fromAddress", StatusList["result"]!["from"]!.ToString()),
                    new("@toAddress", StatusList["result"]!["to"]!.ToString()),
                    new("@hashId", hashId),
                    new("@actionType", "INSERT"),
                    new("@type", "FUND REQUEST")
                };

                var ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string chk = ds.Tables[0].Rows[0]["chk"].ToString()!;
                    if (chk == "1") TempData["success"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    else TempData["error"] = ds.Tables[0].Rows[0]["msg"].ToString();

                    return RedirectToAction("RequestFund");
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error occurred: " + ex.Message;
            }
            return View();
        }

        [HttpPost("account/wallet-add-fund-Dptoken/{amount}/{hashId}")]
        public async Task<IActionResult> AddfundDp20(double amount, string hashId)
        {
            try
            {
                string cType = "USDT-Bigprofits-20";
                double weiValue = 1000000000000000000;
                string depositAddress = "", tokenAddress = "";

                if (string.IsNullOrEmpty(hashId))
                {
                    TempData["msg"] = "Please enter hash ID/txn ID first!";
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }

                // Get Transaction Status from BSC
                string url = $"https://api.bscscan.com/api?module=proxy&action=eth_getTransactionReceipt&txhash={hashId}&apikey=8V5JN3EH1C7AS1R816K5MCDJEN1B8UCV4F";
                var StatusList = await commonMethods.GetApiData(url);

                var data = await context.AdminAddressInfos.FirstOrDefaultAsync(t => t.Ctype == cType);
                if (data != null)
                {
                    depositAddress = commonMethods.Decrypt(data.DepositAddress!).ToUpper();
                    tokenAddress = data.TokenAddress!.ToUpper();
                }

                bool isDuplicate = await context.CmitRequests.AnyAsync(t => t.HashId == hashId) || await context.FundAmounts.AnyAsync(t => t.HashId == hashId);

                if (isDuplicate)
                {
                    TempData["msg"] = "Duplicate Transaction Found! We could not complete your request due to transaction duplicacy.";
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }

                if (StatusList["result"] == null || StatusList["result"]!["status"] == null)
                {
                    TempData["msg"] = "Invalid Transaction! We could not find transaction on the blockchain.";
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }

                if (StatusList["result"]!["status"]!.ToString() != "0x1")
                {
                    TempData["msg"] = "Transaction failed! Please try again.";
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }

                var logs = StatusList["result"]!["logs"];
                if (logs == null || !logs.Any() || logs[0]?["topics"]?[2] == null || logs[0]?["data"] == null || logs[0]?["address"] == null)
                {
                    TempData["msg"] = "Invalid Transaction! Log data is missing.";
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }

                string tokenAmount = logs[0]!["data"]!.ToString()[2..];
                tokenAmount = System.Numerics.BigInteger.Parse(tokenAmount, NumberStyles.AllowHexSpecifier).ToString();
                bool isToAddressMatch = logs[0]!["topics"]![2]!.ToString().Contains(depositAddress[2..], StringComparison.CurrentCultureIgnoreCase);

                if (!isToAddressMatch || Convert.ToDouble(tokenAmount) < ((amount - 0.5) * weiValue) || logs[0]!["transactionHash"]!.ToString() != hashId || !logs[0]!["address"]!.ToString().Equals(tokenAddress, StringComparison.CurrentCultureIgnoreCase) || !depositAddress.Equals(Global.Dp20address, StringComparison.CurrentCultureIgnoreCase))
                {
                    TempData["msg"] = "Invalid Transaction! We could not verify your transaction.";
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }

                var par = new List<SqlParameter>
                {
                    new("@memberId", mango),
                    new("@amount", amount),
                    new("@tokenAmount", tokenAmount),
                    new("@walletType", "USDT (DP-20)"),
                    new("@fromAddress", StatusList["result"]!["from"]!.ToString()),
                    new("@toAddress", StatusList["result"]!["to"]!.ToString()),
                    new("@hashId", hashId),
                    new("@actionType", "INSERT"),
                    new("@type", "FUND REQUEST")
                };

                var ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string chk = ds.Tables[0].Rows[0]["chk"].ToString()!;
                    if (chk == "1") TempData["success"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    else TempData["error"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    return Redirect("/account/topup-wallet-add-Drontoken");
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error occurred: " + ex.Message;
            }
            return View();
        }

        [HttpPost("account/wallet-add-fund-hshid")]
        public async Task<IActionResult> AddfundBp30hshid(CmitRequest cmit)
        {

            try
            {
                string cType = "USDT-BEP-20";
                double weiValue = 1000000000000000000;
                string depositAddress = "", /*memberAddress = "",*/ tokenAddress = "", hashId = cmit.HashId!;
                var amount = cmit.CmitReqAmount;

                if (hashId == "")
                {
                    TempData["msg"] = "Please enter hash ID/txn ID first!";
                    return View();
                }

                string url = "https://api.bscscan.com/api?module=proxy&action=eth_getTransactionReceipt&txhash=" + hashId + "&apikey=8V5JN3EH1C7AS1R816K5MCDJEN1B8UCV4F";
                var StatusList = await commonMethods.GetApiData(url);

                var data = await context.AdminAddressInfos.Where(t => t.Ctype == cType).FirstOrDefaultAsync();
                if (data != null)
                {
                    depositAddress = commonMethods.Decrypt(data.DepositAddress!).ToUpper();
                    tokenAddress = data.TokenAddress!.ToUpper();
                }

                var dataCmit = await context.CmitRequests.Where(t => t.HashId == hashId).ToListAsync();
                var dataAf = await context.FundAmounts.Where(t => t.HashId == hashId).ToListAsync();

                if (StatusList["result"] == null || StatusList["result"]!["status"] == null)
                {
                    TempData["msg"] = "Invalid Transaction! We could not find transaction on the blockchain. Please submit a valid transaction.";
                    return RedirectToAction("AddfundBp30");
                }

                if (StatusList["result"]!["status"]!.ToString() != "0x1")
                {
                    TempData["msg"] = "Transaction failed! Please try again";
                    return RedirectToAction("AddfundBp30");
                }

                if (StatusList["result"]!["logs"] == null)
                {
                    TempData["msg"] = "Sorry! We could not verify your transaction. Please submit a valid transaction.";
                    return RedirectToAction("AddfundBp30");
                }

                if (StatusList["result"]!["from"] == null || StatusList["result"]!["logs"]![0]!["topics"]![2] == null || StatusList["result"]!["logs"]![0]!["data"] == null || StatusList["result"]!["logs"]![0]!["address"] == null || StatusList["result"]!["logs"]![0]!["transactionHash"] == null)
                {
                    TempData["msg"] = "Invalid Transaction!! We could not verify your transaction. Please submit a valid transaction.";
                    return RedirectToAction("AddfundBp30");
                }

                string tokenAmount = StatusList["result"]!["logs"]![0]!["data"]!.ToString();
                tokenAmount = tokenAmount[2..];
                tokenAmount = System.Numerics.BigInteger.Parse(tokenAmount, NumberStyles.AllowHexSpecifier).ToString();

                bool isToAddress = StatusList["result"]!["logs"]![0]!["topics"]![2]!.ToString().Contains(depositAddress[2..], StringComparison.CurrentCultureIgnoreCase);

                double margin = 0.5;
                if (!isToAddress || /*StatusList["result"]!["from"]!.ToString().ToUpper() != memberAddress ||*/ Convert.ToDouble(tokenAmount) < ((Convert.ToDouble(amount) - margin) * weiValue) || StatusList["result"]!["logs"]![0]!["transactionHash"]!.ToString() != hashId || !StatusList["result"]!["logs"]![0]!["address"]!.ToString().Equals(tokenAddress, StringComparison.CurrentCultureIgnoreCase))
                {
                    TempData["msg"] = "Invalid Transaction!!! We could not verify your transaction. Please submit a valid transaction.";
                    return RedirectToAction("AddfundBp30");
                }

                if (dataCmit.Count > 0 || dataAf.Count > 0)
                {
                    TempData["msg"] = "Duplicate Transaction Found! We could not complete your request due to transaction duplicacy.";
                    return RedirectToAction("AddfundBp30");
                }

                if (!depositAddress.Equals(Global.Bp20Address, StringComparison.CurrentCultureIgnoreCase))
                {
                    TempData["msg"] = "Invalid Transaction! We could not verify your transaction. Please submit a valid transaction.";
                    return RedirectToAction("AddfundBp30");
                }

                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@amount", amount));
                par.Add(new SqlParameter("@tokenAmount", tokenAmount));
                par.Add(new SqlParameter("@walletType", "USDT (BEP-20)"));
                par.Add(new SqlParameter("@fromAddress", StatusList["result"]!["from"]!.ToString()));
                par.Add(new SqlParameter("@toAddress", StatusList["result"]!["to"]!.ToString()));
                par.Add(new SqlParameter("@hashId", StatusList["result"]!["logs"]![0]!["transactionHash"]!.ToString()));
                par.Add(new SqlParameter("@actionType", "INSERT"));
                par.Add(new SqlParameter("@type", "FUND REQUEST"));
                var ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string chk = ds.Tables[0].Rows[0]["chk"].ToString()!;
                    if (chk == "1") TempData["success"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    else if (chk == "0") TempData["error"] = ds.Tables[0].Rows[0]["msg"].ToString();

                    return RedirectToAction("RequestFund");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpGet("account/wallet-summary")]
        public async Task<IActionResult> RequestFundHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }

            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "WALLET HISTORY"));
            ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("account/wallet-send-fund")]
        public async Task<IActionResult> Transfer()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "WALLET TRANSFER"));
            ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpGet("account/wallet-add-inr/{amount}")]
        public IActionResult InrAmmount(string amount)
        {
            string usdRate = _configuration.GetValue<string>("usdRate")!;
            ViewBag.usdt = usdRate;
            ViewBag.paybleamount = Convert.ToDecimal(amount) * Convert.ToDecimal(usdRate);
            ViewBag.amount = amount;

            return View();
        }

        [HttpPost("account/wallet-add-inr/{amount}")]
        public async Task<IActionResult> InrAmmount(CmitRequest cmit, string amount)
        {
            string usdRate = _configuration.GetValue<string>("usdRate")!;
            ViewBag.paybleamount = Convert.ToDecimal(amount) * Convert.ToDecimal(usdRate);

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@amount", amount));
            par.Add(new SqlParameter("@usdtAmount", usdRate));
            par.Add(new SqlParameter("@hashId", cmit.HashId));
            par.Add(new SqlParameter("@tokenAmount", cmit.CmitReqAmount));
            par.Add(new SqlParameter("@type", "WALLET REQUEST"));
            par.Add(new SqlParameter("@actionType", "INSERT"));

            var ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);

            TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();

            return View();
        }

        [HttpPost("account/wallet-send-fund")]
        public async Task<IActionResult> Transfer(FundAmount fund)
        {
            try
            {
                string amount = Request.Form["amount"].ToString().Trim();
                string Memberid = Request.Form["Memberid"].ToString().Trim();
                if (fund.MemberId!.Trim() == "")
                {
                    TempData["msg"] = "Invalid Member ID or member is inactive! You can not transfer fund to an inactive member.";
                    return RedirectToAction("Transfer");
                }

                if (!decimal.TryParse(amount.ToString(), out decimal result))
                {
                    TempData["msg"] = "Please enter valid amount..!";
                    return RedirectToAction("Transfer");
                }

                decimal balance = 0;
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    balance = Convert.ToDecimal(ds.Tables[0].Rows[0]["AW_Available"]);
                }

                par = [];
                par.Add(new SqlParameter("@memberId", Memberid));
                par.Add(new SqlParameter("@byMemberId", mango));
                par.Add(new SqlParameter("@amount", amount.ToString()));
                par.Add(new SqlParameter("@available", balance.ToString()));
                par.Add(new SqlParameter("@actionType", "INSERT"));
                par.Add(new SqlParameter("@type", "WALLET TRANSFER"));
                ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);
                ViewBag.data = ds;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                    return RedirectToAction("Transfer");
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return RedirectToAction("Transfer");
        }

        [HttpGet("account/wallet-received-fun")]
        public async Task<IActionResult> Recivedfund()
        {
            try
            {
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                par.Add(new SqlParameter("@rtype", "RECEIVED FUND"));
                var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

                ViewBag.data = ds;
                return View(ds);
            }
            catch (Exception)
            {
                TempData["msg"] = "Exception message, please contact to admin";
            }
            return View();
        }

        [HttpGet("account/activate-from-topup-wallet")]
        public async Task<IActionResult> TopUp()
        {
            ViewBag.memberid=mango;
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }

            TempData["walletType"] = "TOPUP WALLET";

            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@atype", TempData["walletType"]));
            par.Add(new SqlParameter("@rtype", "USERTOPUP HISTORY"));
            ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpGet("account/activate-from-topup-wallet1")]
        public async Task<IActionResult> TopUpNew()
        {
            ViewBag.memberid = mango;
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }

            TempData["walletType"] = "EXTRA WALLET";

            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@atype", TempData["walletType"]));
            par.Add(new SqlParameter("@rtype", "USERTOPUP HISTORY"));
            ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
            return View(ds);
        }

        [HttpPost("SponsorName/{memberId}")]
        public string GetMemberName(String memberId)
        {
            string? result = "";
            try
            {
                var data = context.MemberInfos.Where(x => x.MemberId == memberId).FirstOrDefault();
                result = data == null ? "No such member exist" : data.MemName;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result!;
        }

        [HttpPost("account/activate-from-topup-wallet")]
        public async Task<IActionResult> TopUpPost()
        {           
            string redirectPage = Request.Form["BymemberId"].ToString().Trim() == "EXTRA WALLET" ? "TopUpNew" : "TopUp";
            try
            {
                string amount = Request.Form["amount"].ToString().Trim();
                string Memberid = Request.Form["Memberid"].ToString().Trim();
                string walletType = Request.Form["BymemberId"].ToString().Trim();

                if (walletType == "EXTRA WALLET") Memberid = mango;
                
                if (Memberid.Trim() == "")
                {
                    TempData["msg"] = "Please enter member ID first..!";
                    return RedirectToAction(redirectPage);
                }

                if (!decimal.TryParse(amount.ToString(), out decimal result))
                {
                    TempData["msg"] = "Please enter valid amount..!";
                    return RedirectToAction(redirectPage);
                }

                //using (var client = new HttpClient())
                //{
                //    var response = await client.GetAsync($"{_configuration.GetValue<string>("NodeApiUrl")}/withdrawal");
                //    response.EnsureSuccessStatusCode();
                //    //Console.WriteLine(await response.Content.ReadAsStringAsync());
                //    var statusList = JObject.Parse(await response.Content.ReadAsStringAsync());
                //    if (statusList == null)
                //    {
                //        TempData["msg"] = "Opps! Blockchain API is currently not responding! Please try after sometime.";
                //        return RedirectToAction(redirectPage);
                //    }
                //}

                string memberId = Memberid.ToString().Trim();
                var datam = await context.MemberInfos.Where(t => t.MemLogId == memberId || t.MemberId == memberId).FirstOrDefaultAsync();

                decimal balance = 0;
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    balance = Convert.ToDecimal(ds.Tables[0].Rows[0][(walletType == "EXTRA WALLET" ? "EW_Available" : "AW_Available")]);
                }

                ds = new DataSet();
                par = [];
                par.Add(new SqlParameter("@memberId", Memberid));
                par.Add(new SqlParameter("@amount", amount));
                par.Add(new SqlParameter("@byMemberId", mango));
                par.Add(new SqlParameter("@activationAmount", amount));
                par.Add(new SqlParameter("@available", balance.ToString()));
                par.Add(new SqlParameter("@walletType", walletType));
                par.Add(new SqlParameter("@actionType", "INSERT"));
                par.Add(new SqlParameter("@type", "TOPUP FROM TOPUP WALLET"));
                ds = await _dataAccess.FnRetriveByPro("[SP_SendFund]", par);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["msg"].ToString();

                    if (ds.Tables[0].Rows[0]["Chk"].ToString() == "1")
                    {
                        var client = new HttpClient();
                        var payload = new
                        {
                            userId = Memberid,
                            orderId = ds.Tables[0].Rows[0]["cmitId"].ToString()
                        };
                        var response = await client.PostAsJsonAsync($"{_configuration.GetValue<string>("NodeApiUrl")}/send-token", payload);
                        response.EnsureSuccessStatusCode();
                        //Console.WriteLine(await response.Content.ReadAsStringAsync());
                        var statusList = JObject.Parse(await response.Content.ReadAsStringAsync());
                    }

                    return RedirectToAction(redirectPage);
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message.Contains("No connection could be made") ? "Opps! Blockchain API is currently not responding! Please try after sometime." : "Exception message, please contact to admin";
            }
            return RedirectToAction(redirectPage);
        }

        //[HttpGet("account/Electcity-recharge")]
        //public async Task<IActionResult> Electcity()
        //{
        //    var memid = HttpContext.Session.GetString(Global.userid);
        //    ViewBag.Op = new SelectList(await insPayRepository.ElectricityOperators(), "Id", "OpName");
        //    ViewBag.fatchbill = new SelectList(await insPayRepository.FatchElectricityOperators(), "OpCode", "OpName");
        //    var result = await customContext.aaSpMemberAccount.FromSqlRaw($"Exec SpMemberAccount1 '{memid}'").ToListAsync();
        //    ViewBag.wallet = result[0].AvailableWallet;
        //    ViewBag.totalrec = result[0].FundSpent;
        //    return View();
        //}

        [HttpGet("account/mobile-recharge")]
        public async Task<IActionResult> MobileRecharge()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            return View(ds);
        }

        [HttpGet("account/dth-recharge")]
        public async Task<IActionResult> Dth()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.SpMember = System.Text.Json.JsonSerializer.Serialize(_dataAccess.ConvertDataSetToJson(ds.Tables[0]));
            }
            return View(ds);       
        }

        [HttpGet("Order-Details")]
        public async Task<IActionResult> Orderdetails(string Our_txn_no)
        {
            try
            {
                var data = await context.OnlineServices.FirstOrDefaultAsync(t => t.OurTxnNo == Our_txn_no || t.UserTxnNo == Our_txn_no);
                if (data == null)
                {
                    TempData["msg"] = "Sorry! We could not find your transaction. Please try again.";
                    return RedirectToAction("OrderHistory");
                }

                if (data.OprtnTime != "INSTANTPAY")
                {
                    TempData["msg"] = "Operation time is not INSTANTPAY.";
                    return RedirectToAction("OrderHistory");
                }

                //var client = new RestClient(new RestClientOptions("https://api.instantpay.in"));
                //var request = new RestRequest("/reports/txnStatus", Method.Post)
                //    .AddHeader("X-Ipay-Auth-Code", "1")
                //    .AddHeader("X-Ipay-Client-Id", _configuration["INSP:client_id"]!)
                //    .AddHeader("X-Ipay-Client-Secret", _configuration["INSP:client_secret"]!)
                //    .AddHeader("X-Ipay-Endpoint-Ip", _configuration["INSP:ip"]!)
                //    .AddHeader("Content-Type", "application/json");

                //var body = new
                //{
                //    transactionDate = string.Format("{0:yyyy-MM-dd}", data.Odate),
                //    externalRef = Our_txn_no
                //};
                //request.AddStringBody(JsonConvert.SerializeObject(body), DataFormat.Json);

                //var response = await client.ExecuteAsync(request);
                //var statusList = JObject.Parse(response.Content!);

                //if (statusList?["statuscode"] != null)
                //{
                //    string status;
                //    string bg;
                //    string icon;

                //    if (statusList["statuscode"]!.ToString() == "TXN")
                //    {
                //        status = "SUCCESS";
                //        bg = "success";
                //        icon = "check";
                //    }
                //    else if (statusList["statuscode"]!.ToString() == "TUP")
                //    {
                //        status = "PENDING";
                //        bg = "warning";
                //        icon = "refresh";
                //    }
                //    else
                //    {
                //        status = "FAILED";
                //        bg = "danger";
                //        icon = "times";
                //    }

                //    ViewData["OrderId"] = statusList["data"]!["order"]!["externalRef"]!.ToString();
                //    ViewData["Status"] = status;
                //    ViewData["StatusClass"] = bg;
                //    ViewData["StatusIcon"] = icon;
                //    ViewData["Service"] = statusList["data"]!["order"]!["spName"]!.ToString();
                //    ViewData["Amount"] = statusList["data"]!["transactionAmount"]!.ToString();
                //    ViewData["Number"] = statusList["data"]!["order"]!["account"]!.ToString();
                //    ViewData["TransactionStatus"] = statusList["data"]!["transactionStatus"]!.ToString();
                //    ViewData["TransactionDateTime"] = statusList["data"]!["transactionDateTime"]!.ToString();

                //    if (data.Status == "PENDING" && status != "PENDING")
                //    {
                //        var isPaid = status == "SUCCESS" ? "1" : "0";
                //        List<SqlParameter> par = new List<SqlParameter>
                //        {
                //            new SqlParameter("@memberId", Our_txn_no),
                //            new SqlParameter("@status", isPaid),
                //            new SqlParameter("@type", "UPDATE TXN")
                //        };

                //        await _dataAccess.FnRetriveByPro("[SP_Action]", par);

                //        if (status == "FAILED")
                //        {
                //            TempData["msg"] = "Transaction failed. Please try again.";
                //            return RedirectToAction("OrderHistory");
                //        }
                //    }
                //}
                //else
                //{
                //    TempData["msg"] = "Sorry! We could not find your transaction. Please try again.";
                //    return RedirectToAction("OrderHistory");
                //}
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Exception occurred, please contact admin. " + ex.Message;
                return RedirectToAction("OrderHistory");
            }
            return View();
        }

        [HttpGet("account/order-history")]
        public async Task<IActionResult> OrderHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@memberId", mango));
            par.Add(new SqlParameter("@rtype", "OrderHistory"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);

            ViewBag.data = ds;
            return View(ds);
        }

        [HttpPost("account/INSP_Utility_Payment")]
        public async Task<ActionResult> INSPPayment(string number, string amount, string billerId)
        {
            CommonRepository.Swal obj = new();
            try
            {
                var biller = await context.InspBillers.Where(x => x.BillerId == billerId && x.IsAvailable == 1).FirstOrDefaultAsync();
                if (biller == null)
                {
                    obj.Error = 0;
                    obj.Type = "error";
                    obj.Title = "Invalid Data!";
                    obj.Text = "Invalid Data! Biller ID is not valid.";
                    return Json(obj);
                }

                decimal balanceInr = 0;
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                var ds = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    balanceInr = Convert.ToDecimal(ds.Tables[0].Rows[0]["available"]);
                }

                if (balanceInr < Convert.ToDecimal(amount))
                {
                    obj.Error = 0;
                    obj.Type = "error";
                    obj.Title = "Insufficient Balance!";
                    obj.Text = "Insufficient Balance! You don't have enough balance to recharge";
                    return Json(obj);
                }

                par = [];
                ds = await _dataAccess.FnRetriveByPro("[SpApiFundDetail]", par);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    balanceInr = Convert.ToDecimal(ds.Tables[0].Rows[0]["available"]);
                }

                if (balanceInr < Convert.ToDecimal(amount))
                {
                    obj.Error = 0;
                    obj.Type = "error";
                    obj.Title = "Server Unavailable!";
                    obj.Text = "Server Unavailable! We are unable to complete your request right now. Please try after sometime.";
                    return Json(obj);
                }

                using var client = new HttpClient();
                string SERVICETYPE = biller.CategoryName!;
                int paybale = Convert.ToInt32(amount);
                string USERTXN = await NewTxnId("INSTANTPAY");

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.instantpay.in/marketplace/utilityPayments/payment");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("X-Ipay-Auth-Code", "1");
                request.Headers.Add("X-Ipay-Client-Id", _configuration["INSP:client_id"]);
                request.Headers.Add("X-Ipay-Client-Secret", _configuration["INSP:client_secret"]);
                request.Headers.Add("X-Ipay-Endpoint-Ip", _configuration["INSP:ip"]);
                request.Headers.Add("X-Ipay-Outlet-Id", _configuration["INSP:outlet_id"]);
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    billerId = biller.BillerId,
                    externalRef = USERTXN,
                    enquiryReferenceId = "",
                    inputParameters = new { param1 = number },
                    initChannel = "AGT",
                    deviceInfo = new
                    {
                        terminalId = "12813923",
                        mobile = number,
                        postalCode = "462041",
                        geoCode = "28.6326,77.2175"
                    },
                    paymentMode = "Wallet",
                    paymentInfo = new { Remarks = "Divendu Sir Instant Pay Wallet" },
                    remarks = new { param1 = number },
                    transactionAmount = paybale,
                    customerPan = ""
                }), Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var StatusList = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());

                if (StatusList == null)
                {
                    obj.Error = 0;
                    obj.Type = "error";
                    obj.Title = "Something Went Wrong!";
                    obj.Text = "Something Went Wrong! Please try again sometime.";
                    return Json(obj);
                }

                if (StatusList["statuscode"] == null)
                {
                    obj.Error = 0;
                    obj.Type = "error";
                    obj.Title = "Request Failed!";
                    obj.Text = "Request Failed! Please try again sometime.";
                    return Json(obj);
                }

                string ResStatus = StatusList["statuscode"]!.ToString();
                string Transtatus = ResStatus == "TXN" ? "SUCCESS" : ResStatus == "TUP" ? "PENDING" : "FAILED";
                string status = ResStatus == "TXN" ? "1" : ResStatus == "TUP" ? "2" : "0";

                par =
                [
                    new("@memberid", mango),
                    new("@ServiceType", SERVICETYPE),
                    new("@Status", Transtatus),
                    new("@Our_txn_no", USERTXN),
                    new("@time", "INSTANTPAY"),
                    new("@isPaid", status)
                ];

                if (status == "0")
                {
                    par.Add(new SqlParameter("@message", StatusList["status"]!.ToString()));

                    obj.Error = 0;
                    obj.Type = "error";
                    obj.Title = "Transaction Failed!";
                    obj.Text = "Transaction Failed! " + StatusList["status"]!.ToString();
                }
                else
                {
                    par.Add(new SqlParameter("@operatorcode", biller.BillerId));
                    par.Add(new SqlParameter("@account", StatusList["data"]!["billerDetails"]!["account"]!.ToString()));
                    par.Add(new SqlParameter("@Operator_ref_no", StatusList["data"]!["txnReferenceId"]!.ToString()));
                    par.Add(new SqlParameter("@amount", StatusList["data"]!["txnValue"]!.ToString()));
                    par.Add(new SqlParameter("@message", StatusList["status"]!.ToString()));
                    par.Add(new SqlParameter("@User_txn_no", StatusList["data"]!["externalRef"]!.ToString()));

                    obj.Error = 0;
                    obj.Type = Transtatus == "SUCCESS" ? "success" : "warning";
                    obj.Title = "Transaction Submitted Successfully!";
                    obj.Text = "Transaction Submitted Successfully! Status : " + Transtatus;
                }
                ds = await _dataAccess.FnRetriveByPro("OnlineData", par);

                return Json(obj);
            }
            catch (Exception)
            {
                obj.Error = 1;
                obj.Type = "error";
                obj.Title = "Something Went Wrong!!";
                obj.Text = "Something Went Wrong! Please try again sometime.";
                return Json(obj);
            }
        }

        private async Task<string> NewTxnId(string type)
        {
            try
            {
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@type", type));
                var orderId = await _dataAccess.FnRetriveByPro("[NewTrnsctn]", par);
                return orderId.Tables[0].Rows[0]["orderId"].ToString()!;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("account/products")]
        public async Task<IActionResult> Products(int? subCategoryId)
        {
            var categoryList = await context.ProductCats.ToListAsync();
            ViewBag.CategoryList = categoryList;

            var subCategoryList = await context.ProductSubCats.ToListAsync();
            ViewBag.SubCategoryList = subCategoryList;

            //var categorySubCategoryMap = categoryList.ToDictionary(category => category.CatName,
            //    category => subCategoryList.Where(sc => sc.CatId == category.Id).ToList()
            //);
            //ViewBag.CategorySubCategoryMap = categorySubCategoryMap;

            List<SqlParameter> par = [];
            if (subCategoryId == null)
            {
                par.Add(new SqlParameter("@atype", "SELECT_PRODUCT"));
                var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
                ViewBag.data = data.Tables[0];
            }
            else
            {
                par.Add(new SqlParameter("@atype", "SELECT_PRODUCT BY ID"));
                par.Add(new SqlParameter("@subCatId", subCategoryId));
                var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
                ViewBag.data = data.Tables[0];
            }

            return View();
        }

        [HttpGet("account/member-products-history")]
        public async Task<IActionResult> MemberProductsHistory()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@userId", mango));
            par.Add(new SqlParameter("@atype", "PRODUCT HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
            return View(ds);
        }

        [HttpGet("account/add-cart")]
        public async Task<IActionResult> AddToCart(string ProductId)
        {
            if (ModelState.IsValid)
            {
                var addres = await context.MemberInfos.Where(x => x.MemberId == mango).FirstOrDefaultAsync();
                if (addres!.MemAddress == "" || addres.MemAddress == null)
                {
                    TempData["error"] = "Please Fill Your Address First.";
                    return RedirectToAction("profile", "Account");
                }

                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@atype", "ADD_TO_CART"));
                par.Add(new SqlParameter("@userId", mango));
                par.Add(new SqlParameter("@pId", ProductId));
                try
                {
                    var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
                    TempData["msg"] = data.Tables[0].Rows[0]["msg"].ToString();
                    return RedirectToAction("products");
                }
                catch
                {
                    TempData["error"] = "SomeThing Went Wrong";
                    return RedirectToAction("products");
                }
            }
            return RedirectToAction("products");
        }

        [HttpGet("account/cartlist")]
        public async Task<IActionResult> CartList()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@atype", "GET_CART"));
            par.Add(new SqlParameter("@userId", mango));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
            ViewBag.data = String.Format("{0:0.00}", ds.Tables[1].Rows[0]["totalCartPrice"]);

            par = [];
            par.Add(new SqlParameter("@memberId", mango));
            var dsa = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
            if (dsa.Tables[0].Rows.Count > 0)
            {
                ViewBag.availableWallet = string.Format("{0:0.00}", dsa.Tables[0].Rows[0]["AW_Available"]);
            }
            return View(ds);
        }

        [HttpPost("account/cart/remove/{Id}")]
        public async Task<IActionResult> Remove(int Id)
        {
            var cartcount = await context.TableCarts.FindAsync(Id);
            if (cartcount != null)
            {
                context.TableCarts.Remove(cartcount);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("CartList");
        }

        [HttpPost("account/UpdateQty/{Id}")]
        public async Task<IActionResult> UpdateProdQty(int Id, int quant)
        {
            var cartcount = await context.TableCarts.FindAsync(Id);
            if (cartcount != null)
            {
                if (quant < 0 && cartcount.Quantity > 1)
                {
                    cartcount.Quantity += quant;
                }
                if (quant > 0)
                {
                    cartcount.Quantity += quant;
                }
                await context.SaveChangesAsync();
            }
            return RedirectToAction("CartList");
        }

        [HttpPost("account/place-ordered-standalone")]
        public async Task<IActionResult> PlaceOrder()
        {
            var memmob = Request.Form["memberId"].ToString().Trim();
            try
            {
                if (memmob == "")
                {
                    TempData["error"] = "Failed! Please Enter Member ID";
                    return RedirectToAction("CartList");
                }

                var data1 = await context.MemberInfos.Where(x => x.MemberId == memmob).FirstOrDefaultAsync();
                if (data1 == null)
                {
                    TempData["error"] = "Failed! Invalid member.";
                    return RedirectToAction("CartList");
                }
                var card = await context.TableCarts.FirstOrDefaultAsync();
                if (card == null)
                {
                    TempData["error"] = "Failed! Your Cart Empty Please Select Product.";
                    return RedirectToAction("CartList");
                }
                decimal available = 0;
                List<SqlParameter> par = [];
                par.Add(new SqlParameter("@memberId", mango));
                var dsa = await _dataAccess.FnRetriveByPro("[SpMemberAccount]", par);
                if (dsa.Tables[0].Rows.Count > 0)
                {
                    available = Convert.ToDecimal(dsa.Tables[0].Rows[0]["AW_Available"]);
                }

                par = [];
                par.Add(new SqlParameter("@atype", "Place Order"));
                par.Add(new SqlParameter("@userId", data1.MemberId));
                par.Add(new SqlParameter("@byUserId", mango));
                par.Add(new SqlParameter("@walletBal", available));
                var data = await _dataAccess.FnRetriveByPro("[SP_PlaceOrder]", par);

                TempData["msg"] = data.Tables[0].Rows[0]["msg"].ToString();
            }
            catch
            {
                TempData["error"] = "SomeThing Went Wrong";
                return RedirectToAction("CartList");
            }
            return RedirectToAction("CartList");
        }

        [HttpPost("account/GetName")]
        public string GetMemberNames(string memberId)
        {
            string? result = "";
            try
            {
                var data = context.MemberInfos.FirstOrDefault(x => x.MemberId == memberId);
                result = data == null ? "No such member exist" : data.MemName;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result!;
        }

    }
}
