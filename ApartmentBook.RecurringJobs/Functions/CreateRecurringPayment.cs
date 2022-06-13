using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Payments.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApartmentBook.RecurringJobs.Functions
{
    public class CreateRecurringPayment
    {
        private readonly IPaymentService paymentService;
        private readonly IApartmentService apartmentService;

        public CreateRecurringPayment(IPaymentService paymentService, IApartmentService apartmentService)
        {
            this.paymentService = paymentService;
            this.apartmentService = apartmentService;
        }

        [FunctionName("CreateRecurringPayment")]
        public async Task Run([TimerTrigger("0 0 1 * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"CreateRecurringPayment function started at: {DateTime.Now}");
            var apartments = await apartmentService.GetAllAsync();
            int amountOfPaymentsCreated = 0;
            foreach (var apartment in apartments)
            {
                var payment = new Payment()
                {
                    Type = PaymentType.Rent,
                    Amount = apartment.Rent,
                    AmountPaid = 0,
                    Apartment = apartment,
                    User = apartment.User
                };
                await paymentService.CreateAsync(payment);
                amountOfPaymentsCreated++;
            }
            log.LogInformation($"CreateRecurringPayment function finised at: {DateTime.Now}, creating {amountOfPaymentsCreated} payments");
        }
    }
}