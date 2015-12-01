namespace DynamicTranslator.Core.Dependency
{
    #region using

    using System.Reflection;
    using Manager;

    #endregion

    internal class ConventionalRegistrationContext : IConventionalRegistrationContext
    {
        internal ConventionalRegistrationContext(Assembly assembly, IIocManager iocManager, ConventionalRegistrationConfig config)
        {
            Assembly = assembly;
            IocManager = iocManager;
            Config = config;
        }

        public Assembly Assembly { get; }

        public ConventionalRegistrationConfig Config { get; }

        public IIocManager IocManager { get; }
    }
}