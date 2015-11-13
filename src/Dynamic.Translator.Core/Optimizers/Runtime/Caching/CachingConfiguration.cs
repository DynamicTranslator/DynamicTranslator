namespace Dynamic.Translator.Core.Optimizers.Runtime.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Dependency.Markers;

    internal class CachingConfiguration : ICachingConfiguration, ISingletonDependency
    {
        public IReadOnlyList<ICacheConfigurator> Configurators => _configurators.ToImmutableList();

        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration()
        {
            _configurators = new List<ICacheConfigurator>();
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}