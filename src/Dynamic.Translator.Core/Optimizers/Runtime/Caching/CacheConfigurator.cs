namespace Dynamic.Translator.Core.Optimizers.Runtime.Caching
{
    using System;

    internal class CacheConfigurator : ICacheConfigurator
    {
        public string CacheName { get; }

        public Action<ICache> InitAction { get; }

        public CacheConfigurator(Action<ICache> initAction)
        {
            InitAction = initAction;
        }

        public CacheConfigurator(string cacheName, Action<ICache> initAction)
        {
            CacheName = cacheName;
            InitAction = initAction;
        }
    }
}