using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NZWalks.API.CacheHelper;
using NZWalks.API.Data;
using NZWalks.API.Helper;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;
        private readonly IMemoryCache _cache;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<WalkDifficultyRepository> _logger;
        private readonly ICacheKeyGenerator _cacheKeyGenerator;
        public static string CurrentEntity { get { return "Region"; } }
        public static string CachingMethod { get { return "AllRegions"; } }

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext, IMemoryCache cache, ICacheManager cacheManager ,ILogger<WalkDifficultyRepository> logger, ICacheKeyGenerator cacheKeyGenerator )
        {
            this.nZWalksDBContext = nZWalksDBContext;
            _cache = cache;
            _cacheManager = cacheManager;
            _logger = logger;
            _cacheKeyGenerator = cacheKeyGenerator;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty entity)
        {
            entity.Id = Guid.NewGuid();
            await nZWalksDBContext.WalkDifficulties.AddAsync(entity);
            await nZWalksDBContext.SaveChangesAsync();
            _cacheManager.RemoveCache(Tuple.Create(CurrentEntity, CachingMethod));
            return entity;
        }

        public async Task<WalkDifficulty> Delete(Guid id)
        {
            var walkDifficultyToDelete = await nZWalksDBContext.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficultyToDelete == null)
                return null;

            nZWalksDBContext.WalkDifficulties.Remove(walkDifficultyToDelete);
            _cacheManager.RemoveCache(Tuple.Create(CurrentEntity, CachingMethod));
            await nZWalksDBContext.SaveChangesAsync();

            return walkDifficultyToDelete;
        }

        public async Task<List<WalkDifficulty>> GetAllAsync()
        {
            _logger.LogInformation("Trying to find all walkdifficulties from Cache");
            if (_cache.TryGetValue(_cacheKeyGenerator.GenerateCacheKey(Tuple.Create(CurrentEntity, CachingMethod)), out List<WalkDifficulty> allWalkDifficulties))
            {
                _logger.LogInformation("found all walkdifficulties in Cache");

                return allWalkDifficulties;
            }

            var walkDifficulties = await nZWalksDBContext.WalkDifficulties.ToListAsync();
            _cache.Set(_cacheKeyGenerator.GenerateCacheKey(Tuple.Create(CurrentEntity, CachingMethod)), walkDifficulties, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(60),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                Priority=CacheItemPriority.Normal
            });
            _logger.LogInformation($"entity {CurrentEntity} added in cache");
            return walkDifficulties;
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
            _cacheManager.RemoveCache(Tuple.Create(CurrentEntity, CachingMethod));
            return walkDifficultyToUpdate;
        }
    }
}
