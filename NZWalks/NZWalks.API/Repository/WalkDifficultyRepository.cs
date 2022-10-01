using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty entity)
        {
            entity.Id = Guid.NewGuid();
            await nZWalksDBContext.WalkDifficulties.AddAsync(entity);
            await nZWalksDBContext.SaveChangesAsync();
            return entity;
        }

        public async Task<WalkDifficulty> Delete(Guid id)
        {
            var walkDifficultyToDelete = await nZWalksDBContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficultyToDelete == null)
                return null;

            nZWalksDBContext.WalkDifficulties.Remove(walkDifficultyToDelete);
            await nZWalksDBContext.SaveChangesAsync();

            return walkDifficultyToDelete;
        }

        public Task<List<WalkDifficulty>> GetAllAsync()
        {
            return nZWalksDBContext.WalkDifficulties.ToListAsync();
        }

        public Task<WalkDifficulty> GetAsync(Guid id)
        {
            return nZWalksDBContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty entity)
        {
            var walkDifficultyToUpdate = await nZWalksDBContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficultyToUpdate == null)
                return null;

            walkDifficultyToUpdate.Code = entity.Code;
            nZWalksDBContext.Update(walkDifficultyToUpdate);
            await nZWalksDBContext.SaveChangesAsync();

            return walkDifficultyToUpdate;
        }
    }
}
