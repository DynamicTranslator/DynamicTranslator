using DynamicTranslator.Dependency.Extensions;
using DynamicTranslator.Dependency.Manager;
using DynamicTranslator.Dependency.Markers;
using DynamicTranslator.Optimizers.Runtime.Caching;

namespace DynamicTranslator.Optimizers.Runtime.MemoryCache
{
    public class InMemoryCacheManager : CacheManagerBase, ISingletonDependency
    {
        public InMemoryCacheManager(IIocManager iocManager, ICachingConfiguration configuration) : base(iocManager, configuration)
        {
            IocManager.RegisterIfAbsent<InMemoryCache>(DependencyLifeStyle.Transient);
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Resolve<InMemoryCache>(new {name});
        }
    }
}