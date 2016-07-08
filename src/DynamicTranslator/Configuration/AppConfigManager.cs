using System.Configuration;
using System.Linq;

using Abp.Dependency;

namespace DynamicTranslator.Configuration
{
    public class AppConfigManager : IAppConfigManager, ITransientDependency
    {
        public string Get(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return ConfigurationManager.AppSettings[key];
            }

            return string.Empty;
        }

        public IAppConfigManager SaveOrUpdate(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
            return this;
        }
    }
}