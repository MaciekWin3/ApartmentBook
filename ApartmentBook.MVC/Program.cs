using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Apartments.Repositories;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Repositories;
using ApartmentBook.MVC.Features.Payments.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
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

// Logger
builder.Logging.ClearProviders();
ILogger logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(logger);

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Repositories
builder.Services.AddTransient<IApartmentRepository, ApartmentRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();

// Services
builder.Services.AddTransient<IApartmentService, ApartmentService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

var app = builder.Build();

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