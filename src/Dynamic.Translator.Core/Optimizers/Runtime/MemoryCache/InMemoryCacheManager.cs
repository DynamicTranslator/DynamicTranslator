namespace Dynamic.Translator.Core.Optimizers.Runtime.MemoryCache
{
    #region using

    using Caching;
    using Dependency.Extensions;
    using Dependency.Manager;
    using Dependency.Markers;

    #endregion

    public class InMemoryCacheManager : CacheManagerBase, ISingletonDependency
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public InMemoryCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {
            IocManager.RegisterIfNot<InMemoryCache>(DependencyLifeStyle.Transient);
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Resolve<InMemoryCache>(new {name});
        }
    }
}