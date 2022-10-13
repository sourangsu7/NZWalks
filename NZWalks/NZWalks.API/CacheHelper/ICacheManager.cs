namespace NZWalks.API.CacheHelper
{
    public interface ICacheManager
    {
        void RemoveCache(Tuple<string, string> cacheIndentifier);
    }
}
