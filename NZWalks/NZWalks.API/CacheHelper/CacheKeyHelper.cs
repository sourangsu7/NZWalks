namespace NZWalks.API.Helper
{
    public class CacheKeyHelper : ICacheKeyGenerator
    {
        public string GenerateCacheKey(Tuple<string, string> KeyInput)
        {
            return $"{KeyInput.Item1}#{KeyInput.Item2}";
        }
    }
}
