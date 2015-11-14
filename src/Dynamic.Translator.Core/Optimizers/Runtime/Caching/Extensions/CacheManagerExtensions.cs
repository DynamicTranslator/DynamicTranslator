namespace Dynamic.Translator.Core.Optimizers.Runtime.Caching.Extensions
{
    public static class CacheManagerExtensions
    {
        public static ITypedCache<TKey, TValue> GetCacheEnvironment<TKey, TValue>(this ICacheManager cacheManager, string name)
        {
            return cacheManager.GetCacheEnvironment(name).AsTyped<TKey, TValue>();
        }
    }
}