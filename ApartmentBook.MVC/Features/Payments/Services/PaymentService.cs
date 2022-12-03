using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Payments.Repositories;
using System.Text;

namespace ApartmentBook.MVC.Features.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IApartmentService apartmentService;

        public PaymentService(IPaymentRepository paymentRepository, IApartmentService apartmentService)
        {
            this.paymentRepository = paymentRepository;
            this.apartmentService = apartmentService;
        }

        public async Task<Payment> GetAsync(Guid? id)
        {
            return await paymentRepository.GetAsync(id);
        }

        public async Task<List<Payment>> GetApartmentsPayments(Guid apartmentId)
        {
            return await paymentRepository.GetApartmentPaymentsAsync(apartmentId);
        }

        public async Task<List<Payment>> GetUsersPayments(string userId)
        {
            return await paymentRepository.GetUsersPaymentsAsync(userId);
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await paymentRepository.GetAsync(id);
            if (payment is not null)
            {
                await paymentRepository.DeleteAsync(payment);
            }
        }

        public async Task UpdateAsync(Payment payment)
        {
            await paymentRepository.UpdateAsync(payment);
        }

        public async Task CreateAsync(Payment payment)
        {
            await paymentRepository.CreateAsync(payment);
        }

        public async Task<Guid> PayPaymentAndReturnApartmentId(Guid id)
        {
            var payment = await paymentRepository.GetAsync(id);
            var apartment = await apartmentService.GetAsync(payment.Apartment.Id);
            if (payment.IsPaid)
            {
                return apartment.Id;
            }
            payment.AmountPaid = payment.Amount;
            await paymentRepository.UpdateAsync(payment);
            return apartment.Id;
        }

        public IDictionary<PaymentType, decimal> GetChartData(DateTime date, Guid apartmentId)
        {
            return paymentRepository.GetChartData(date, apartmentId);
        }

        public async Task<string> GenerateCsvPaymentSummary(CsvPaymetsType type, string userId = null, Guid? apartmentId = null)
        {
            List<Payment> payments = new();
            switch (type)
            {
                case CsvPaymetsType.All:
                    payments = await paymentRepository.GetPaymentsBetweeenDatesAsync(DateTime.MinValue, DateTime.Now);
                    break;

                case CsvPaymetsType.User when userId is not null:
                    payments = await paymentRepository.GetUsersPaymentsAsync(userId);
                    break;

                case CsvPaymetsType.Apartment when apartmentId is not null:
                    payments = await paymentRepository.GetApartmentPaymentsAsync((Guid)apartmentId);
                    break;

                default:
                    throw new Exception();
            }
            var sb = new StringBuilder();
            sb.AppendLine("");
            foreach (var payment in payments)
            {
                sb.AppendLine($"");
            }
            return string.Empty;
        }
    }
}