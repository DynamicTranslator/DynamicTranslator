using System;
using System.Runtime.Caching;

using DynamicTranslator.Exception;
using DynamicTranslator.Optimizers.Runtime.Caching;

namespace DynamicTranslator.Optimizers.Runtime.MemoryCache
{
    #region using

    

    #endregion

    public class InMemoryCache : CacheBase
    {
        private System.Runtime.Caching.MemoryCache _memoryCache;

        public InMemoryCache(string name)
            : base(name)
        {
            _memoryCache = new System.Runtime.Caching.MemoryCache(Name);
        }

        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = new System.Runtime.Caching.MemoryCache(Name);
        }

        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }

        public override object GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
        }

        public override void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null)
        {
            if (value == null)
            {
                throw new BusinessException("Can not insert null values to the cache!");
            }

            //TODO: Optimize by using a default CacheItemPolicy?
            _memoryCache.Set(
                key,
                value,
                new CacheItemPolicy
                {
                    SlidingExpiration = slidingExpireTime ?? DefaultSlidingExpireTime
                });
        }
    }
}