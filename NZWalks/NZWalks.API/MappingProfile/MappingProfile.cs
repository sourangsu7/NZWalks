using AutoMapper;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.DTO.WalkDifficulty;

namespace NZWalks.API.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Domain.Region, Region>().ReverseMap();
            CreateMap<Models.Domain.Walk, Walk>().ReverseMap();
            CreateMap<Models.Domain.WalkDifficulty, WalkDifficulty>().ReverseMap();
          
            CreateMap<Models.Domain.Walk, CreateWalk>().ReverseMap();
            CreateMap<Models.Domain.Walk, UpdateWalk>().ReverseMap();
            CreateMap<Models.Domain.WalkDifficulty, CreateWalkDifficulty>().ReverseMap();


        }
    }
}
