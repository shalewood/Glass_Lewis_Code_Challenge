using CompanyApi.Api.Models.DTO;
using CompanyApi.Api.Models;
using AutoMapper;

namespace CompanyApi.Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Company, CompanyDTO>().ReverseMap();
        }
    }
}
