namespace Dynamic.Translator.Core.Optimizers.Runtime.Caching
{
    #region using

    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Dependency.Manager;

    #endregion

    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly IIocManager IocManager;

        protected readonly ICachingConfiguration Configuration;

        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase(IIocManager iocManager, ICachingConfiguration configuration)
        {
            IocManager = iocManager;
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCacheEnvironments()
        {
            return Caches.Values.ToImmutableList();
        }

        public virtual ICache GetCacheEnvironment(string name)
        {
            return Caches.GetOrAdd(name, cacheName =>
            {
                var cache = CreateCacheImplementation(cacheName);

                var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);

                foreach (var configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }

                return cache;
            });
        }

        public virtual void Dispose()
        {
            foreach (var cache in Caches)
            {
                IocManager.Release(cache.Value);
            }

            Caches.Clear();
        }

        /// <summary>
        ///     Used to create actual cache implementation.
        /// </summary>
        /// <param name="name">Name of the cache</param>
        /// <returns>Cache object</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}