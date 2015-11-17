namespace DynamicTranslator.Core.Dependency.Installer
{
    #region using

    using System.IO;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Config;
    using DBreeze;
    using Orchestrators;

    #endregion

    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.AddFacility<TextGuardConvention>();
            container.AddFacility<UnitOfWorkConvention>();

            var noSqlDBPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            container.Register(
                Component.For<IStartupConfiguration>().ImplementedBy<StartupConfiguration>().LifeStyle.Singleton,
                Component.For<IMeanFinderFactory>().AsFactory().LifeStyle.Transient,
                Component.For<IMeanOrganizerFactory>().AsFactory().LifeStyle.Transient,
                Component.For(typeof (DBreezeEngine)).Instance(new DBreezeEngine(noSqlDBPath))
                );
        }
    }
}