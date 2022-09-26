using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Walk> AddAsync(Walk entity)
        {
            entity.Id = Guid.NewGuid();
            await nZWalksDBContext.Walks.AddAsync(entity);
            await nZWalksDBContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Walk> Delete(Guid id)
        {
            var walkToRemove = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkToRemove == null)
                return null;

            nZWalksDBContext.Walks.Remove(walkToRemove);
            await nZWalksDBContext.SaveChangesAsync();
            return walkToRemove;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await nZWalksDBContext.Walks.Include(x=>x.Region).Include(x=>x.WalkDifficulty).ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Walks.Include(x=>x.Region).Include(x=>x.WalkDifficulty).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk entity)
        {
            var walkToUpdate = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkToUpdate == null)
                return null;

            walkToUpdate.Name=entity.Name;
            walkToUpdate.Length=entity.Length;
            walkToUpdate.WalkDifficultyId=entity.WalkDifficultyId;
            walkToUpdate.RegionId = entity.RegionId;

            nZWalksDBContext.Walks.Update(walkToUpdate);
            await nZWalksDBContext.SaveChangesAsync();
            return walkToUpdate;
        }
    }
}
