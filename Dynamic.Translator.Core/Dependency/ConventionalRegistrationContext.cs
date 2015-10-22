namespace Dynamic.Translator.Core.Dependency
{
    #region using

    using System.Reflection;
    using Manager;

    #endregion

    internal class ConventionalRegistrationContext : IConventionalRegistrationContext
    {
        internal ConventionalRegistrationContext(Assembly assembly, IIocManager iocManager, ConventionalRegistrationConfig config)
        {
            this.Assembly = assembly;
            this.IocManager = iocManager;
            this.Config = config;
        }

        public Assembly Assembly { get; }
        public IIocManager IocManager { get; }
        public ConventionalRegistrationConfig Config { get; }
    }
}