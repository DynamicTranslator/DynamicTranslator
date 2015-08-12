namespace Dynamic.Translator.Core.Dependency.Installer
{
    #region using

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Config;

    #endregion

    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IStartupConfiguration>().ImplementedBy<StartupConfiguration>().LifeStyle.Singleton
                //Component.For<IDictionaryBasedConfig>().ImplementedBy<DictionayBasedConfig>().LifeStyle.Singleton
                );
        }
    }
}