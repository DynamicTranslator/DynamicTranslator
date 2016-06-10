using DynamicTranslator.Core.Config;

namespace DynamicTranslator.Core.Dependency
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