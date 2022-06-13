using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Payments.Services
{
    public interface IPaymentService
    {
        Task CreateAsync(Payment payment);

        Task DeleteAsync(Guid id);

        Task<List<Payment>> GetApartmentsPayments(Guid apartmentId);

        Task<List<Payment>> GetUsersPayments(string userId);

        Task<Payment> GetAsync(Guid? id);

        Task UpdateAsync(Payment payment);

        Task<Guid> PayPaymentAndReturnApartmentId(Guid id);

        Task<IDictionary<PaymentType, decimal>> GetChartData(DateTime date, Guid apartmentId);
    }
}