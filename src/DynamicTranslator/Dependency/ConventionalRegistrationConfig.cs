using DynamicTranslator.Config;

namespace DynamicTranslator.Dependency
{
    public class ConventionalRegistrationConfig : DynamicTranslatorDictionayBasedConfig
    {
        public ConventionalRegistrationConfig()
        {
            InstallInstallers = true;
        }

        public bool InstallInstallers { get; set; }
    }
}