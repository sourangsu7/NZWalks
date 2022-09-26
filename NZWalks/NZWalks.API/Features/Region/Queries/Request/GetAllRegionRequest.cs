using MediatR;

namespace NZWalks.API.Features.Region.Queries.Request
{
    public class GetAllRegionRequest:IRequest<List<Models.DTO.Region>>
    {
    }
}
