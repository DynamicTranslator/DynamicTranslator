namespace DynamicTranslator.Configuration.Startup
{
    public interface IYandexTranslatorConfiguration : ITranslatorConfiguration, IConfiguration
    {
        string ApiKey { get; set; }
    }
}