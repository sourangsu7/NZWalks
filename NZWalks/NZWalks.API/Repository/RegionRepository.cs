using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Region>> GetAllAsync()
        {
            var allRegions = await nZWalksDBContext.Regions.ToListAsync();
            return allRegions;
        }

        public async Task<Region> AddAsync(Region entity)
        {
            entity.Id = Guid.NewGuid();
            await nZWalksDBContext.Regions.AddAsync(entity);
            await nZWalksDBContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Region> Delete(Guid id)
        {
            var regionToDelete = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionToDelete == null)
                return null;
            nZWalksDBContext.Regions.Remove(regionToDelete);
            await nZWalksDBContext.SaveChangesAsync();
            return regionToDelete;  
        }

        public async Task<Region> UpdateAsync(Guid id, Region entity)
        {
            var regionToUpdate = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionToUpdate == null)
                return null;

            regionToUpdate.Code = entity.Code;
            regionToUpdate.Area = entity.Area;
            regionToUpdate.Longitude = entity.Longitude;
            regionToUpdate.Latitude = entity.Latitude;
            regionToUpdate.Name = entity.Name;


            nZWalksDBContext.Regions.Update(regionToUpdate);
            await nZWalksDBContext.SaveChangesAsync();
            return regionToUpdate;
        }
    }
}
