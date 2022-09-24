using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Region, RegionListDTO>().ReverseMap();
        }
    }
}
