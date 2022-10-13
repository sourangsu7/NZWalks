using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NZWalks.API.CacheHelper;
using NZWalks.API.Data;
using NZWalks.API.Helper;
using NZWalks.API.Models.Domain;
using System.ComponentModel;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RegionRepository> _logger;
        private readonly ICacheKeyGenerator _keyGenerator;
        private readonly ICacheManager _cacheManager;

        public static string currentEntity { get { return "Region"; } }
        public static string cachingMethod { get { return "AllRegions"; } }

        public RegionRepository(NZWalksDBContext nZWalksDBContext,IMemoryCache cache,ILogger<RegionRepository> logger, ICacheKeyGenerator keyGenerator, ICacheManager cacheManager)
        {
            this.nZWalksDBContext = nZWalksDBContext;
            _cache = cache;
            _logger = logger;
            _keyGenerator = keyGenerator;
            _cacheManager = cacheManager;
        }

        public async Task<Region> GetAsync(Guid id)
        {
            _logger.LogInformation("Trying to access information from in memory cache");
            var cacheKey = _keyGenerator.GenerateCacheKey(Tuple.Create(currentEntity,cachingMethod));

            var region = new Region();

            if (!string.IsNullOrEmpty(cacheKey) && _cache.TryGetValue(cacheKey, out List<Region> nzRegions))
            {
                _logger.LogInformation("Found all Regions in Cache");
                region = nzRegions.FirstOrDefault(x => x.Id == id);
            }
            if(region != null)
            {
                _logger.LogInformation($"Found region by {id} in Cache");
                return region;
            }

            return await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Region>> GetAllAsync()
        {
            _logger.LogInformation("Trying to access information from in memory cache");
            var cacheKey = _keyGenerator.GenerateCacheKey(Tuple.Create(currentEntity, cachingMethod));

            if (!string.IsNullOrEmpty(cacheKey) && _cache.TryGetValue(cacheKey, out List<Region> nzRegions))
            {
                _logger.LogInformation("Found list of all regions in Cache");
                return nzRegions;
            }
            var allRegions = await nZWalksDBContext.Regions.ToListAsync();
            var cacheEntryOption = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(60),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                Priority = CacheItemPriority.Normal
            };
            _cache.Set(cacheKey, allRegions, cacheEntryOption);
            _logger.LogInformation($"entity {currentEntity} added in cache");
            return allRegions;
        }

        public async Task<Region> AddAsync(Region entity)
        {
            entity.Id = Guid.NewGuid();
            await nZWalksDBContext.Regions.AddAsync(entity);
            await nZWalksDBContext.SaveChangesAsync();
            _cacheManager.RemoveCache(Tuple.Create(currentEntity, cachingMethod));
            return entity;
        }

        public async Task<Region> Delete(Guid id)
        {
            var regionToDelete = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionToDelete == null)
                return null;
            nZWalksDBContext.Regions.Remove(regionToDelete);
            await nZWalksDBContext.SaveChangesAsync();
            _cacheManager.RemoveCache(Tuple.Create(currentEntity, cachingMethod));
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
            _cacheManager.RemoveCache(Tuple.Create(currentEntity, cachingMethod));
            return regionToUpdate;
        }
    }
}
