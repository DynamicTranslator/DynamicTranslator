using Abp.Configuration;
using Abp.Dependency;

namespace DynamicTranslator.Configuration.Startup
{
    public class DynamicTranslatorConfiguration : DictionaryBasedConfig, IDynamicTranslatorConfiguration
    {
        public IIocManager IocManager { get; }

        public DynamicTranslatorConfiguration(IIocManager iocManager, IAppConfigManager appConfigManager)
        {
            IocManager = iocManager;
            AppConfigManager = appConfigManager;
        }

        public IActiveTranslatorConfiguration ActiveTranslatorConfiguration { get; private set; }

        public IAppConfigManager AppConfigManager { get; }

        public IApplicationConfiguration ApplicationConfiguration { get; private set; }

        public IBingTranslatorConfiguration BingTranslatorConfiguration { get; private set; }

        public IGoogleAnalyticsConfiguration GoogleAnalyticsConfiguration { get; private set; }

        public IGoogleDetectorConfiguration GoogleDetectorConfiguration { get; private set; }

        public IGoogleTranslatorConfiguration GoogleTranslatorConfiguration { get; private set; }

        public ILocalPersistenceConfiguration LocalConfigurationPersistence { get; private set; }

        public ISesliSozlukTranslatorConfiguration SesliSozlukTranslatorConfiguration { get; private set; }

        public ITurengTranslatorConfiguration TurengTranslatorConfiguration { get; private set; }

        public IYandexDetectorConfiguration YandexDetectorConfiguration { get; private set; }

        public IYandexTranslatorConfiguration YandexTranslatorConfiguration { get; private set; }

        public IZarganTranslatorConfiguration ZarganTranslatorConfiguration { get; private set; }

        public void Initialize()
        {
            ApplicationConfiguration = IocManager.Resolve<IApplicationConfiguration>();
            LocalConfigurationPersistence = IocManager.Resolve<ILocalPersistenceConfiguration>();
            ActiveTranslatorConfiguration = IocManager.Resolve<IActiveTranslatorConfiguration>();
            SesliSozlukTranslatorConfiguration = IocManager.Resolve<ISesliSozlukTranslatorConfiguration>();
            YandexTranslatorConfiguration = IocManager.Resolve<IYandexTranslatorConfiguration>();
            TurengTranslatorConfiguration = IocManager.Resolve<ITurengTranslatorConfiguration>();
            BingTranslatorConfiguration = IocManager.Resolve<IBingTranslatorConfiguration>();
            GoogleTranslatorConfiguration = IocManager.Resolve<IGoogleTranslatorConfiguration>();
            GoogleDetectorConfiguration = IocManager.Resolve<IGoogleDetectorConfiguration>();
            GoogleAnalyticsConfiguration = IocManager.Resolve<IGoogleAnalyticsConfiguration>();
            ZarganTranslatorConfiguration = IocManager.Resolve<IZarganTranslatorConfiguration>();
            YandexDetectorConfiguration = IocManager.Resolve<IYandexDetectorConfiguration>();
        }
    }
}