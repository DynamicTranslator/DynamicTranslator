using System;

namespace DynamicTranslator.Core.Optimizers.Runtime.Caching
{
    #region using

    

    #endregion

    internal class CacheConfigurator : ICacheConfigurator
    {
        public CacheConfigurator(Action<ICache> initAction)
        {
            InitAction = initAction;
        }

        public CacheConfigurator(string cacheName, Action<ICache> initAction)
        {
            CacheName = cacheName;
            InitAction = initAction;
        }

        public string CacheName { get; }

        public Action<ICache> InitAction { get; }
    }
}