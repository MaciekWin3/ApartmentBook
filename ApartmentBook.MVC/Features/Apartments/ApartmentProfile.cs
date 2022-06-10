using ApartmentBook.MVC.Features.Apartments.DTOs;
using ApartmentBook.MVC.Features.Apartments.Models;
using AutoMapper;

namespace ApartmentBook.MVC.Features.Apartments
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateEntityToDTOMappings();
            CreateDTOToEntityMappings();
        }

        private void CreateEntityToDTOMappings()
        {
            CreateMap<Apartment, ApartmentForCreateDTO>();
            CreateMap<Apartment, ApartmentForUpdateDTO>();
        }

        private void CreateDTOToEntityMappings()
        {
            CreateMap<ApartmentForCreateDTO, Apartment>();
            CreateMap<ApartmentForUpdateDTO, Apartment>();
        }
    }
}