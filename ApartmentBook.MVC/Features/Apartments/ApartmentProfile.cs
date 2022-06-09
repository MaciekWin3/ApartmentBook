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
            CreateMap<Apartment, ApartamentForCreateDTO>();
            CreateMap<Apartment, ApartamentForUpdateDTO>();
        }

        private void CreateDTOToEntityMappings()
        {
            CreateMap<ApartamentForCreateDTO, Apartment>();
            CreateMap<ApartamentForUpdateDTO, Apartment>();
        }
    }
}