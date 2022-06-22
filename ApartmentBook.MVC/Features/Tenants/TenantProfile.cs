using ApartmentBook.MVC.Features.Tenants.DTOs;
using ApartmentBook.MVC.Features.Tenants.Models;
using AutoMapper;

namespace ApartmentBook.MVC.Features.Tenants
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateEntityToDTOMappings();
            CreateDTOToEntityMappings();
        }

        private void CreateEntityToDTOMappings()
        {
            CreateMap<Tenant, TenantDTO>();
            CreateMap<Tenant, TenantForCreateDTO>();
            CreateMap<Tenant, TenantForUpdateDTO>();
        }

        private void CreateDTOToEntityMappings()
        {
            CreateMap<TenantDTO, Tenant>();
            CreateMap<TenantForCreateDTO, Tenant>();
            CreateMap<TenantForUpdateDTO, Tenant>();
        }
    }
}