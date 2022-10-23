using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Apartments.Repositories;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Emails.Services;
using ApartmentBook.MVC.Features.Payments.Repositories;
using ApartmentBook.MVC.Features.Payments.Services;
using ApartmentBook.MVC.Features.Tenants.Repositories;
using ApartmentBook.MVC.Features.Tenants.Services;
using FluentEmail.Core;
using FluentEmail.Razor;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Globalization;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

/*builder.WebHost
    .UseUrls(builder.Configuration["IpAddress:Address"])
    .ConfigureKestrel(options =>
    {
        //options.Listen(System.Net.IPAddress.Parse("192.168.68.103"), 7262);
        options.Listen(System.Net.IPAddress
            .Parse(builder.Configuration["IpAddress:Address"]), int.Parse(builder.Configuration["IpAddress:Port"]));
    });*/

// Logger
builder.Logging.ClearProviders();
ILogger logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(logger);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console());

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Repositories
builder.Services.AddTransient<IApartmentRepository, ApartmentRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<ITenantRepository, TenantRepository>();

// Services
builder.Services.AddTransient<IApartmentService, ApartmentService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<ITenantService, TenantService>();

// SendGrid
builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.AddTransient<IEmailService, EmailService>();

var sendGridKey = builder.Configuration["SendGrid:Key"];

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = sendGridKey;
});

builder.Services
    .AddFluentEmail(builder.Configuration["SendGrid:Email"])
    .AddRazorRenderer()
    .AddSendGridSender(sendGridKey);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
};

Email.DefaultRenderer = new RazorRenderer();

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("en")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();