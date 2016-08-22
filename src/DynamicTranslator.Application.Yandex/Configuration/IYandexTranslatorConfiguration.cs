using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Application.Yandex.Configuration
{
    public interface IYandexTranslatorConfiguration : ITranslatorConfiguration, IConfiguration
    {
        string ApiKey { get; set; }

        bool ShouldBeAnonymous { get; set; }
    }
}