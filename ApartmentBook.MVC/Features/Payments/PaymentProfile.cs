using ApartmentBook.MVC.Features.Payments.DTOs;
using ApartmentBook.MVC.Features.Payments.Models;
using AutoMapper;

namespace ApartmentBook.MVC.Features.Payments
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateEntityToDTOMappings();
            CreateDTOToEntityMappings();
        }

        private void CreateEntityToDTOMappings()
        {
            CreateMap<Payment, PaymentForCreateDTO>();
            CreateMap<Payment, PaymentForUpdateDTO>();
        }

        private void CreateDTOToEntityMappings()
        {
            CreateMap<PaymentForCreateDTO, Payment>();
            CreateMap<PaymentForUpdateDTO, Payment>();
        }
    }
}