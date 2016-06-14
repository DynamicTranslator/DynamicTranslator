using System.IO;
using System.Reflection;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using DBreeze;

using DynamicTranslator.Config;
using DynamicTranslator.Orchestrators.Detector;
using DynamicTranslator.Orchestrators.Finder;
using DynamicTranslator.Orchestrators.Organizer;

namespace DynamicTranslator.Dependency.Installer
{
    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.AddFacility<InterceptorFacility>();

            var noSqlDBPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DynamicTranslatorDb");

            container.Register(
                Component.For<IDynamicTranslatorStartupConfiguration>().ImplementedBy<DynamicTranslatorStartupConfiguration>().LifeStyle.Singleton,
                Component.For<IMeanFinderFactory>().AsFactory().LifeStyle.Transient,
                Component.For<IMeanOrganizerFactory>().AsFactory().LifeStyle.Transient,
                Component.For<ILanguageDetectorFactory>().AsFactory().LifeStyle.Transient,
                Component.For(typeof(DBreezeEngine)).Instance(new DBreezeEngine(noSqlDBPath))
                );
        }
    }
}