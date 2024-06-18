using AttendanceManagement.Data;
using AttendanceManagement.Filters;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
    
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.RegisterDataTables();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMvc();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews(options => {
    options.Filters.Add(typeof(UserPageVisitedFilter));
} );

builder.Services.AddResponseCaching(
    Options =>
    {
        Options.MaximumBodySize = 10240;
        Options.UseCaseSensitivePaths = true;
    });
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    options.ExcludedHosts.Add("AttendanceManagement.Web");
    options.ExcludedHosts.Add("AttendanceManagement.Web");
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "AttendanceManagement.Web";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Identity/Account/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.AccessDeniedPath = new PathString("/Error/AccessDenied");
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromDays(60);
});

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(7012, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});


var app = builder.Build();


/*handle 404 error*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/404");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/404");

app.Use((context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");

    context.Response.Headers.AltSvc = "h3=\":443\"";
    return next(context);
});

app.UseResponseCaching();
app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse =
        r =>
        {
            var path = r.File.PhysicalPath;
            if (
                path.EndsWith(".gif") ||
                path.EndsWith(".jpg") ||
                path.EndsWith(".png") ||
                path.EndsWith(".svg") ||
                path.Contains("Roboto") ||
                path.Contains("roboto") ||
                path.Contains("plugins") ||
                path.Contains("ico") ||
                path.Contains("swup")
            )
            {
                var maxAge = new TimeSpan(0, 0, 100, 0);
                r.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("60"));
            }
        }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.MapRazorPages();

app.Run();