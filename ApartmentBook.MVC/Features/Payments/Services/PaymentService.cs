using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Payments.Repositories;

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
            return await paymentRepository.GetApartmentPayments(apartmentId);
        }

        public async Task<List<Payment>> GetUsersPayments(string userId)
        {
            return await paymentRepository.GetUsersPayments(userId);
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

        public async Task CreateReccuringPayments()
        {
            var apartametsIds = 2;
            //await paymentRepository.
        }
    }
}
