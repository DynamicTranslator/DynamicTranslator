namespace Dynamic.Translator.Core.Dependency
{
    #region using

    using System.Reflection;

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
        public IIocManager IocManager { get; }
        public ConventionalRegistrationConfig Config { get; }
    }
}