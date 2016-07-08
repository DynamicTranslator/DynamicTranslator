namespace DynamicTranslator.Configuration.Startup
{
    public interface IDynamicTranslatorConfiguration : IConfiguration
    {
        IActiveTranslatorConfiguration ActiveTranslatorConfiguration { get; }

        IApplicationConfiguration ApplicationConfiguration { get; }

        IBingTranslatorConfiguration BingTranslatorConfiguration { get; }

        IGoogleAnalyticsConfiguration GoogleAnalyticsConfiguration { get; }

        IGoogleDetectorConfiguration GoogleDetectorConfiguration { get; }

        IGoogleTranslatorConfiguration GoogleTranslatorConfiguration { get; }

        ILocalPersistenceConfiguration LocalConfigurationPersistence { get; }

        ISesliSozlukTranslatorConfiguration SesliSozlukTranslatorConfiguration { get; }

        ITurengTranslatorConfiguration TurengTranslatorConfiguration { get; }

        IYandexDetectorConfiguration YandexDetectorConfiguration { get; }

        IYandexTranslatorConfiguration YandexTranslatorConfiguration { get; }

        IZarganTranslatorConfiguration ZarganTranslatorConfiguration { get; }

        IAppConfigManager AppConfigManager { get; }
    }
}