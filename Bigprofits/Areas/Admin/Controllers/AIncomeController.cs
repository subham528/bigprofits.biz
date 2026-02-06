using Bigprofits.Data;
using Bigprofits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;
using Bigprofits.Common;
using Bigprofits.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bigprofits.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("britglbl253adpnl")]
    [Route("admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AIncomeController(ContextClass context, CommonMethods commonMethods, SqlConnectionClass dataAccess, IWebHostEnvironment webHostEnvironment, HomeRepository homeRepository) : Controller
    {
        private readonly ContextClass context = context;
        private readonly HomeRepository homeRepository = homeRepository;
        private readonly CommonMethods commonMethods = commonMethods;
        private readonly SqlConnectionClass _dataAccess = dataAccess;
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private string admingo = "";
        public override void OnActionExecuting(ActionExecutingContext _context)
        {
            admingo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var msg = context.TableSupports.Where(x => x.MyStatus == 0 && x.ToBy == "ADMIN").ToList();
            ViewBag.msgCount = msg.Count;
        }

        [HttpGet("reward-Income")]
        public async Task<IActionResult> LevelIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "MATRIX INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Roi-Income")]
        public async Task<IActionResult> ADailyROI()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "ROI INCOME"));    
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;
        
            return View(ds);
        }

        [HttpGet("Sponsor-Income")]
        public async Task<IActionResult> SponsorIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@hashId", "LEVEL"));
            par.Add(new SqlParameter("@rtype", "LEVEL INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Achiver-Income")]
        public async Task<IActionResult> AchiverIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "ACHIEVER INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("DropReward-Income")]
        public async Task<IActionResult>ReffrealIncome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@atype", "REWARD"));
            par.Add(new SqlParameter("@rtype", "REWARD HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("SallaryD-Income")]
        public async Task<IActionResult> Salary()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "REWARD RETURN"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Matching-Income")]
        public async Task<IActionResult> Bonusincome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "BINARY INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("Amatrix-Income")]
        public async Task<IActionResult> Matrixincome()
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@rtype", "MATRIX INCOME"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ShowHistory]", par);
            ViewBag.data = ds;

            return View(ds);
        }

        [HttpGet("add-product")]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.CList = new SelectList(await homeRepository.ProductCat(), "Id", "CatName");
            return View();
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct(AddProduct model)
        {
            try
            {
                var file = model.Product_Image;//IFormFile to Get Image from Form
                string imgPath = "Uploads/ProductsImages/";//Path where profile Image will be saved
                if (file == null)
                {
                    TempData["error"] = "Please select an image first!";
                    return Redirect("/britglbl253adpnl/add-product");
                }
                string extnesion = Path.GetExtension(file.FileName);//for getting File Extension
                string randomId = GenerateRandomString(10);
                if (extnesion != ".jpg" && extnesion != ".jpeg" && extnesion != ".png")
                {
                    TempData["error"] = "Only jpeg,jpg and png supported";
                    return Redirect("/britglbl253adpnl/add-product");
                }
                model.ProductImage = CommonRepository.UploadImage(imgPath, file, webHostEnvironment.WebRootPath, randomId);
                if (ModelState.IsValid)
                {
                    List<SqlParameter> par = [];

                    par.Add(new SqlParameter("@atype", "ADD_PRODUCTS"));
                    par.Add(new SqlParameter("@Product_Name", model.ProductName));
                    par.Add(new SqlParameter("@Product_Description", model.ProductDescription));
                    par.Add(new SqlParameter("@dp", model.Dpprise));
                    par.Add(new SqlParameter("@Price", model.Price));
                    par.Add(new SqlParameter("@GST", model.Gst));
                    par.Add(new SqlParameter("@PriceWithGST", model.PriceWithGst));
                    par.Add(new SqlParameter("@Final_Price", model.FinalPrice));
                    par.Add(new SqlParameter("@Product_Image", model.ProductImage));
                    par.Add(new SqlParameter("@Weight", model.Weight));
                    par.Add(new SqlParameter("@Product_Bv", model.ProductBv));
                    par.Add(new SqlParameter("@catId", model.CatId));
                    par.Add(new SqlParameter("@subCatId", model.SubCatId));

                    try
                    {
                        var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
                        TempData["msg"] = data.Tables[0].Rows[0]["msg"].ToString();

                        return Redirect("/britglbl253adpnl/add-product");
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync(ex.ToString());
                    }
                }
            }
            catch
            {
                TempData["ExceptionError"] = "Exception error";
                return View();
            }
            return View();
        }

        [HttpGet("product-list")]
        public async Task<IActionResult> ProductsList()
        {
            var data = await context.AddProducts.OrderBy(x => x.ProductName).ToListAsync();
            ViewBag.Model = JsonConvert.SerializeObject(data);
            return View(data);
        }

        [HttpGet("edit-product/{id}")]
        public async Task<IActionResult> EditProduct(int id)
        {
            if (id < 1)
            {
                return Redirect("/britglbl253adpnl/product-list");
            }
            try
            {
                var data = await context.AddProducts.Where(x => x.Id == id).FirstOrDefaultAsync();
                ViewBag.CList = new SelectList(await homeRepository.ProductCat(), "Id", "CatName");
                ViewBag.SCList = new SelectList(await homeRepository.ProductSubCat(), "Id", "SubCatName");

                return View(data);
            }
            catch
            {
                TempData["err"] = "Something went wrong";
                return Redirect("/britglbl253adpnl/product-list");
            }
        }

        [HttpPost("update-products/{id}")]
        public async Task<IActionResult> UpdateProducts(AddProduct model, int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await context.AddProducts.Where(x => x.Id == Id).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        return Redirect("/britglbl253adpnl/product-list");
                    }

                    var file = model.Product_Image;//IFormFile to Get Image from Form
                    if (file != null)
                    {
                        string imgPath = "Uploads/ProductsImages/";//Path where profile Image will be saved

                        string extnesion = Path.GetExtension(file.FileName);//for getting File Extension
                        if (extnesion != ".jpg" && extnesion != ".jpeg" && extnesion != ".png")
                        {
                            TempData["error"] = "Only jpeg,jpg and png supported";
                            return Redirect("/britglbl253adpnl/product-list");
                        }

                        var randomId = GenerateRandomString(10);
                        model.ProductImage = CommonRepository.UploadImage(imgPath, file, webHostEnvironment.WebRootPath, randomId);
                    }

                    if (data != null)
                    {
                        data.ProductName = model.ProductName;
                        data.ProductDescription = model.ProductDescription;
                        data.Dpprise = model.Dpprise;
                        data.Price = model.Price;
                        data.Gst = model.Gst;
                        data.PriceWithGst = model.PriceWithGst;
                        data.FinalPrice = model.FinalPrice;
                        if (file != null) data.ProductImage = model.ProductImage;
                        data.Weight = model.Weight;
                        data.ProductBv = model.ProductBv;
                        data.CatId = model.CatId;
                        data.SubCatId = model.SubCatId;
                    }
                    await context.SaveChangesAsync();
                    TempData["msg"] = "Product Updated Successfully";
                    return Redirect("/britglbl253adpnl/product-list");
                }
            }
            catch
            {
                TempData["err"] = "Something went wrong.Product Not Updated";
                return Redirect("/britglbl253adpnl/product-list");
            }
            return View();
        }

        [HttpGet("Product-Request")]
        public async Task<IActionResult> ProductRequest(int status)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@OrderStatus", status));
            par.Add(new SqlParameter("@atype", "PRODUCT HISTORY"));
            var ds = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);

            return View(ds);
        }

        [HttpGet("Order_Detail")]
        public async Task<IActionResult> Order_Details(string? OrderId)
        {
            try
            {
                // Check if OrderId is null or empty
                if (string.IsNullOrEmpty(OrderId))
                {
                    TempData["error"] = "Invalid Order Id.";
                    return RedirectToAction("Product-Request");
                }

                // Query the database to retrieve order details based on OrderId
                var data = await context.OrderDetails
                    .FromSqlRaw("SELECT Order_Details.Id, memberid, OrderId, Order_Details.Product_Id, Order_Details.Quantity, Order_Details.Price, Order_Details.GST, Order_Details.GST_Amount, Order_Details.Amount_With_GST, Order_Details.Final_Price, User_Type, Add_Product.Product_Image, Add_Product.Product_Name AS Product_Name, Add_Product.Product_BV FROM Order_Details INNER JOIN Add_Product ON Add_Product.Product_Id = Order_Details.Product_Id WHERE OrderId = @OrderId",
                    new SqlParameter("@OrderId", OrderId))
                    .ToListAsync();

                // If data is retrieved, pass it to the view
                if (data != null && data.Count > 0)
                {
                    ViewBag.model = data;
                    return View(data);
                }
                else
                {
                    TempData["error"] = "No details found for this order.";
                    return RedirectToAction("Product-Request");
                }
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while fetching the order details.";
                return RedirectToAction("Product-Request");
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet("update-product-request")]
        public async Task<IActionResult> Updateorder(string? OrderId)
        {
            List<SqlParameter> para = [];
            para.Add(new SqlParameter("@atype", "ACCEPT ORDER"));
            para.Add(new SqlParameter("@orderId", OrderId));
            await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", para);
            TempData["msg"] = "Success";
            return RedirectToAction("ProductRequest", new { status = 0 });
        }

        [HttpGet("reject-product-request")]
        public async Task<IActionResult> Rejectorder(string? OrderId)
        {
            List<SqlParameter> para = [];
            para.Add(new SqlParameter("@atype", "REJECT ORDER"));
            para.Add(new SqlParameter("@orderId", OrderId));
            var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", para);
            TempData["rej"] = data.Tables[0].Rows[0]["msg"].ToString();
            return RedirectToAction("ProductRequest", new { status = 0 });
        }

        [HttpGet("product-history")]
        public async Task<IActionResult> ProductHistory()
        {
            var data = await context.ProductRequests.ToListAsync();
            return View(data);
        }

        [HttpGet("add-category")]
        public async Task<IActionResult> AddCategory(int? catId)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@atype", "SELECT_CATEGORY"));
            if (catId > 0) par.Add(new SqlParameter("@catId", catId));
            var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
            ViewBag.data = data.Tables[0];
            ViewBag.btnTitle = "Add Category";

            if (catId > 0)
            {
                ProductCat td = new()
                {
                    Id = (int)catId,
                    CatName = data.Tables[0].Rows[0]["CatName"].ToString()
                };

                ViewBag.btnTitle = "Update Category";
                return View(td);
            }

            return View();
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(ProductCat model)
        {
            if (model.CatName == null)
            {
                TempData["error"] = "Please fill out all the fields!";
                return Redirect("/britglbl253adpnl/add-category");
            }

            if (model.CatName == "")
            {
                TempData["error"] = "Please fill out all the fields!";
                return Redirect("/britglbl253adpnl/add-category");
            }

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@atype", model.Id > 0 ? "UPDATE_CATEGORY" : "ADD_CATEGORY"));
            par.Add(new SqlParameter("@Product_Name", model.CatName));
            if (model.Id > 0) par.Add(new SqlParameter("@catId", model.Id));
            try
            {
                var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
                if (data.Tables[0].Rows[0]["Id"].ToString() == "1") TempData["success"] = data.Tables[0].Rows[0]["msg"].ToString();
                else TempData["error"] = data.Tables[0].Rows[0]["msg"].ToString();

                return Redirect("/britglbl253adpnl/add-category");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return Redirect("/britglbl253adpnl/add-category");
            }
        }

        [HttpGet("add-sub-category")]
        public async Task<IActionResult> AddSubCategory(int? subCatId)
        {
            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@atype", "SELECT_SUB_CATEGORY"));
            if (subCatId > 0) par.Add(new SqlParameter("@subCatId", subCatId));
            var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
            ViewBag.data = data.Tables[0];
            ViewBag.btnTitle = "Add Sub Category";

            ViewBag.PList = new SelectList(await homeRepository.ProductCat(), "Id", "CatName");

            if (subCatId > 0)
            {
                ProductSubCat td = new()
                {
                    Id = (int)subCatId,
                    SubCatName = data.Tables[0].Rows[0]["SubCatName"].ToString(),
                    CatId = Convert.ToInt32(data.Tables[0].Rows[0]["CatId"])
                };

                ViewBag.btnTitle = "Update Sub Category";
                return View(td);
            }

            return View();
        }

        [HttpPost("add-sub-category")]
        public async Task<IActionResult> AddSubCategory(ProductSubCat model)
        {
            if (model.SubCatName == null || model.CatId == null)
            {
                TempData["error"] = "Please fill out all the fields!";
                return Redirect("/britglbl253adpnl/add-sub-category");
            }

            if (model.SubCatName == "" || model.CatId <= 0)
            {
                TempData["error"] = "Please fill out all the fields!";
                return Redirect("/britglbl253adpnl/add-sub-category");
            }

            List<SqlParameter> par = [];
            par.Add(new SqlParameter("@atype", model.Id > 0 ? "UPDATE_SUB_CATEGORY" : "ADD_SUB_CATEGORY"));
            par.Add(new SqlParameter("@Product_Name", model.SubCatName));
            par.Add(new SqlParameter("@catId", model.CatId));
            if (model.Id > 0) par.Add(new SqlParameter("@subCatId", model.Id));
            try
            {
                var data = await _dataAccess.FnRetriveByPro("[SP_ManageProducts]", par);
                if (data.Tables[0].Rows[0]["Id"].ToString() == "1") TempData["success"] = data.Tables[0].Rows[0]["msg"].ToString();
                else TempData["error"] = data.Tables[0].Rows[0]["msg"].ToString();

                return Redirect("/britglbl253adpnl/add-sub-category");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return Redirect("/britglbl253adpnl/add-sub-category");
            }
        }

        [HttpGet("select-sub-category")]
        public async Task<JsonResult> ProductSubCatByCat(int catId)
        {
            var data = await context.ProductSubCats.Where(x => x.CatId == catId).OrderBy(x => x.SubCatName).ToListAsync();
            if (data.Count > 0)
            {
                var result = new List<object>();
                foreach (var item in data)
                {
                    result.Add(new
                    {
                        SubCatId = item.Id,
                        Name = item.SubCatName
                    });
                }
                return Json(result);
            }

            return Json(null);
        }






    }
}
