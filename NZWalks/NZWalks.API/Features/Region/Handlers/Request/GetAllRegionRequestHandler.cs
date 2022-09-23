using AutoMapper;
using MediatR;
using NZWalks.API.Features.Region.Queries.Request;
using NZWalks.API.Models.DTO.Region;
using NZWalks.API.Repository;

namespace NZWalks.API.Features.Region.Handlers.Request
{
    public class GetAllRegionRequestHandler : IRequestHandler<GetAllRegionRequest, List<RegionListDTO>>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public GetAllRegionRequestHandler(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        public async Task<List<RegionListDTO>> Handle(GetAllRegionRequest request, CancellationToken cancellationToken)
        {
            var allRegions = await regionRepository.GetAllRegionsAsync();

            var regionList = mapper.Map<List<RegionListDTO>>(allRegions);

            return regionList;

        }
    }
}
