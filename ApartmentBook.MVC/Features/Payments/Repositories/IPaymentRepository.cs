using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Payments.Repositories
{
    public interface IPaymentRepository
    {
        Task CreateAsync(Payment payment);
        Task DeleteAsync(Payment payment);
        Task<List<Payment>> GetApartmentPayments(Guid apartmentId);
        Task<List<Payment>> GetUsersPayments(string userId);
        Task<Payment> GetAsync(Guid? id);
        Task UpdateAsync(Payment payment);
    }
}