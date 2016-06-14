using DynamicTranslator.Config;

namespace DynamicTranslator.Dependency
{
    public class ConventionalRegistrationConfig : DictionayBasedConfig
    {
        public ConventionalRegistrationConfig()
        {
            InstallInstallers = true;
        }

        public bool InstallInstallers { get; set; }
    }
}