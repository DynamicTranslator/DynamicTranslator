using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Application.Yandex.Configuration
{
    public interface IYandexTranslatorConfiguration : ITranslatorConfiguration, IConfiguration
    {
        string ApiKey { get; set; }

        string BaseUrl { get; set; }

        bool ShouldBeAnonymous { get; set; }

        string SId { get; set; }
    }
}