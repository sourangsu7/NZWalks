using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NZWalks.API.CacheHelper;
using NZWalks.API.Data;
using NZWalks.API.Helper;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WalkRepository> _logger;
        private readonly ICacheManager _cacheManager;
        private readonly ICacheKeyGenerator _cacheKeyGenerator;

        public static string CurrentEntity { get { return "Walk"; } }
        public static string CachingMethod { get { return "AllWalks"; } }

        public WalkRepository(NZWalksDBContext nZWalksDBContext,IMemoryCache cache, ILogger<WalkRepository> logger,ICacheManager cacheManager,ICacheKeyGenerator cacheKeyGenerator)
        {
            this.nZWalksDBContext = nZWalksDBContext;
            _cache = cache;
            _logger = logger;
            _cacheManager = cacheManager;
            _cacheKeyGenerator = cacheKeyGenerator;
        }
        public async Task<Walk> AddAsync(Walk entity)
        {
            entity.Id = Guid.NewGuid();
            await nZWalksDBContext.Walks.AddAsync(entity);
            await nZWalksDBContext.SaveChangesAsync();
            _cacheManager.RemoveCache(Tuple.Create(CurrentEntity, CachingMethod));
            return entity;
        }

        public async Task<Walk> Delete(Guid id)
        {
            var walkToRemove = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkToRemove == null)
                return null;

            nZWalksDBContext.Walks.Remove(walkToRemove);
            await nZWalksDBContext.SaveChangesAsync();
            _cacheManager.RemoveCache(Tuple.Create(CurrentEntity, CachingMethod));
            return walkToRemove;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            _logger.LogInformation("Trying to access all walks from cache");
            if(_cache.TryGetValue(_cacheKeyGenerator.GenerateCacheKey(Tuple.Create(CurrentEntity,CachingMethod)), out List<Walk> allnzWalks))
            {
                _logger.LogInformation("all walks found in cache");
                return allnzWalks;
            }

            var allWalks = await nZWalksDBContext.Walks.Include(x=>x.Region).Include(x=>x.WalkDifficulty).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(60),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                Priority = CacheItemPriority.Normal,
            };
            _cache.Set(_cacheKeyGenerator.GenerateCacheKey(Tuple.Create(CurrentEntity, CachingMethod)),allWalks,cacheEntryOptions);
            _logger.LogInformation($"entity {CurrentEntity} added in cache");
            return allWalks;
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            _logger.LogInformation("Trying to access all walks from cache");
            if (_cache.TryGetValue(_cacheKeyGenerator.GenerateCacheKey(Tuple.Create(CurrentEntity, CachingMethod)), out List<Walk> allnzWalks))
            {
                _logger.LogInformation("all walks found in cache");
                var currentWalk = allnzWalks.FirstOrDefault(x => x.Id == id);
                if(currentWalk != null)
                    return currentWalk;
            }
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
            _cacheManager.RemoveCache(Tuple.Create(CurrentEntity, CachingMethod));
            await nZWalksDBContext.SaveChangesAsync();
            return walkToUpdate;
        }
    }
}
