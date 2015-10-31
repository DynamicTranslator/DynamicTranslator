namespace Dynamic.Translator.Core.Dependency.Installer
{
    #region using

    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Config;
    using Orchestrators;

    #endregion

    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.AddFacility<TextGuardConvention>();

            container.Register(
                Component.For<IStartupConfiguration>().ImplementedBy<StartupConfiguration>().LifeStyle.Singleton,
                Component.For<IMeanFinderFactory>().AsFactory(),
                Component.For<IMeanOrganizerFactory>().AsFactory()
                );
        }
    }
}