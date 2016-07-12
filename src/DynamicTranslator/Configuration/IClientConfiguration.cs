using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public interface IClientConfiguration : IConfiguration
    {
        string AppVersion { get; set; }

        IDynamicTranslatorConfiguration Configurations { get; }

        string Id { get; set; }

        string MachineName { get; set; }
    }
}