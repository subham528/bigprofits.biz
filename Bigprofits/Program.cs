using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Bigprofits.Common;
using Bigprofits.Data;
using Bigprofits.Repository;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Persist Data Protection keys so cookies survive IIS app pool recycle
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys");
Directory.CreateDirectory(keysFolder);
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(keysFolder)).SetApplicationName("Bigprofits");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContextClass>(options => options.UseSqlServer(connectionString!));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "UserAuth";
    options.DefaultSignInScheme = "UserAuth";
    options.DefaultChallengeScheme = "UserAuth";
})
.AddCookie("UserAuth", options =>
{
    options.LoginPath = "/account/sign-in";
    options.AccessDeniedPath = "/";
    options.Cookie.Name = "UserAuth.Auth";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
})
.AddCookie("AdminAuth", options =>
{
    options.LoginPath = "/britglbl253adpnl/sign-in";
    options.AccessDeniedPath = "/";
    options.Cookie.Name = "AdminAuth.Auth";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<HomeRepository, HomeRepository>();
builder.Services.AddScoped<MailRepository, MailRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<CommonMethods, CommonMethods>();
builder.Services.AddScoped<SqlConnectionClass, SqlConnectionClass>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(

    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
