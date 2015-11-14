namespace Dynamic.Translator.Core.Optimizers.Runtime.MemoryCache
{
    #region using

    using System;
    using System.Runtime.Caching;
    using Caching;
    using Exception;

    #endregion

    public class InMemoryCache : CacheBase
    {
        private MemoryCache _memoryCache;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="name">Unique name of the cache</param>
        public InMemoryCache(string name)
            : base(name)
        {
            _memoryCache = new MemoryCache(Name);
        }

        public override object GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
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

        public override void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(Name);
        }

        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}