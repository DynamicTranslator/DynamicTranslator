using DynamicTranslator.Core.Dependency.Extensions;
using DynamicTranslator.Core.Dependency.Manager;
using DynamicTranslator.Core.Dependency.Markers;
using DynamicTranslator.Core.Optimizers.Runtime.Caching;

namespace DynamicTranslator.Core.Optimizers.Runtime.MemoryCache
{
    #region using

    

    #endregion

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