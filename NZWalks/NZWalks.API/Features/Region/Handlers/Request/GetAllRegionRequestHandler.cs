using AutoMapper;
using MediatR;
using NZWalks.API.Features.Region.Queries.Request;
using NZWalks.API.Repository;

namespace NZWalks.API.Features.Region.Handlers.Request
{
    public class GetAllRegionRequestHandler : IRequestHandler<GetAllRegionRequest, List<Models.DTO.Region>>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public GetAllRegionRequestHandler(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        public async Task<List<Models.DTO.Region>> Handle(GetAllRegionRequest request, CancellationToken cancellationToken)
        {
            var allRegions = await regionRepository.GetAllAsync();

            var regionList = mapper.Map<List<Models.DTO.Region>>(allRegions);

            return regionList;

        }
    }
}
