using AutoMapper;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>().ReverseMap();
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>().ReverseMap();
            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
            //CreateMap<Models.Domain.Walk, CreateWalk>()
            //    .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Region.Id))
            //    .ForMember(dest => dest.WalkDifficultyId, opt => opt.MapFrom(src => src.WalkDifficulty.Id))
            //    .ReverseMap();
            CreateMap<Models.Domain.Walk, CreateWalk>().ReverseMap();
            CreateMap<Models.Domain.Walk, UpdateWalk>().ReverseMap();

        }
    }
}
