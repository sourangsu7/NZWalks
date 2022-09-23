using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }


        public int Add(Region entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Region Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Region> GetAll()
        {
            return nZWalksDBContext.Regions.ToList();
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            
            return await nZWalksDBContext.Regions.ToListAsync();
        }

        public int Update(Region entity)
        {
            throw new NotImplementedException();
        }
    }
}
