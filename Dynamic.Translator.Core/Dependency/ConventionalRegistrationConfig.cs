namespace Dynamic.Translator.Core.Dependency
{
    #region using

    using Config;

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