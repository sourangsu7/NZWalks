using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<List<Region>> GetAllRegionsAsync();
    }
}
