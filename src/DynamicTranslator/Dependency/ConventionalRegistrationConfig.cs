using DynamicTranslator.Config;

namespace DynamicTranslator.Dependency
{
    #region using

    

    #endregion

    public class ConventionalRegistrationConfig : DictionayBasedConfig
    {
        public ConventionalRegistrationConfig()
        {
            InstallInstallers = true;
        }

        public bool InstallInstallers { get; set; }
    }
}