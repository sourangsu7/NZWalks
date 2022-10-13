using Microsoft.Extensions.Caching.Memory;
using NZWalks.API.Helper;

namespace NZWalks.API.CacheHelper
{
    public class ManageCache:ICacheManager
    {
        private readonly ILogger<ManageCache> _logger;
        private readonly ICacheKeyGenerator _cacheKeyGenerator;
        private readonly IMemoryCache _cache;

        public ManageCache(ILogger<ManageCache> logger,ICacheKeyGenerator cacheKeyGenerator,IMemoryCache cache)
        {
            _logger = logger;
            _cacheKeyGenerator = cacheKeyGenerator;
            _cache = cache;
        }
        public void RemoveCache(Tuple<string,string> cacheIndentifier)
        {
            _logger.LogInformation("Trying to access information from in memory cache");
            var cacheKey = _cacheKeyGenerator.GenerateCacheKey(Tuple.Create(cacheIndentifier.Item1, cacheIndentifier.Item2));

            if (!string.IsNullOrEmpty(cacheKey) && _cache.TryGetValue(cacheKey, out var nzRegions))
            {
                _logger.LogInformation("Found list of all regions in Cache");
                _cache.Remove(cacheKey);
                _logger.LogInformation("Cleared Cache from memory to reload on next call");
            }
        }
    }
}
