using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QFX;
using QFX.data;
using QFX.Manager;
using QFX.Manager.Interface;
using QFX.Models;
using QFX.Provider;
using QFX.Provider.Interface;
using QFX.Service;
using QuestPDF.Infrastructure;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x=>{x.LoginPath = "/Admin/Auth/Login"; });
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddNotyf(config=>
    { config.DurationInSeconds = 10;config.IsDismissable = true;config.Position = NotyfPosition.BottomRight; });

builder.Services.AddControllers();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IAuthManager , AuthManager>();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddScoped<ICurrentLocationProvider, CurrentLocationProvider>();
builder.Services.AddScoped<IMailSender, MailSender>();
// builder.Services.AddScoped<AutomateMailService>();
// builder.Services.AddHostedService<AutomateMailService>();
// builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
QuestPDF.Settings.License = LicenseType.Community;

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Public}/{controller=Public}/{action=Index}/{id?}").RequireAuthorization();
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area=Admin}/{controller=Auth}/{action=Login}/{id?}").RequireAuthorization();

// app.MapRazorPages();
app.Run();
