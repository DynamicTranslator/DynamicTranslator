using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public class ClientConfiguration : IClientConfiguration
    {
        public ClientConfiguration(IDynamicTranslatorConfiguration configurations)
        {
            Configurations = configurations;
        }

        public string AppVersion { get; set; }

        public IDynamicTranslatorConfiguration Configurations { get; }

        public string Id { get; set; }

        public string MachineName { get; set; }
    }
}
