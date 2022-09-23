using MediatR;
using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Features.Region.Queries.Request
{
    public class GetAllRegionRequest:IRequest<List<RegionListDTO>>
    {
    }
}
