using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Yandex.Configuration
{
    public interface IYandexTranslatorConfiguration : ITranslatorConfiguration, IConfiguration
    {
        string ApiKey { get; set; }
    }
}