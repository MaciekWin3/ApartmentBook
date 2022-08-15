using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Payments.Repositories
{
    public interface IPaymentRepository
    {
        Task CreateAsync(Payment payment);

        Task DeleteAsync(Payment payment);

        Task<List<Payment>> GetApartmentPaymentsAsync(Guid apartmentId);

        Task<List<Payment>> GetUsersPaymentsAsync(string userId);

        Task<List<Payment>> GetPaymentsBetweeenDatesAsync(DateTime from, DateTime to);

        Task<Payment> GetAsync(Guid? id);

        Task UpdateAsync(Payment payment);

        Task<IDictionary<PaymentType, decimal>> GetChartData(DateTime date, Guid apartmentId);
    }
}