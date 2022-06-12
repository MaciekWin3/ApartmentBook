using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Apartments.Repositories;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Payments.Repositories;
using ApartmentBook.MVC.Features.Payments.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(ApartmentBook.RecurringJobs.Startup))]

namespace ApartmentBook.RecurringJobs
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseLazyLoadingProxies(false);
                options.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=aspnet-ApartmentBook.MVC-2ABDDE8A-66F2-459E-AF64-94B6C4C29FD1;Trusted_Connection=True;MultipleActiveResultSets=true",
                builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    builder.CommandTimeout(10);
                }
                );
            });

            builder.Services.AddTransient<IApartmentRepository, ApartmentRepository>();
            builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();

            builder.Services.AddTransient<IApartmentService, ApartmentService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
        }
    }
}