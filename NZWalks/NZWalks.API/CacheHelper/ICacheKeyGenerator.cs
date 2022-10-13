namespace NZWalks.API.Helper
{
    public interface ICacheKeyGenerator
    {
        string GenerateCacheKey(Tuple<string,string> KeyInput);
    }
}
