using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicTranslator.Core.Optimizers.Runtime.Caching
{
    #region using

    

    #endregion

    internal class CachingConfiguration : ICachingConfiguration
    {
        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration()
        {
            _configurators = new List<ICacheConfigurator>();
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public IReadOnlyList<ICacheConfigurator> Configurators => _configurators.ToImmutableList();
    }
}