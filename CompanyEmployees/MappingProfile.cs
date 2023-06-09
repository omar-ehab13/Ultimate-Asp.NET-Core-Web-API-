using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForCtorParam(nameof(CompanyDto.FullAddress),
                    options => options.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        }
    }
}
