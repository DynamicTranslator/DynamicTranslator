using System;
using System.Reflection;

using Castle.Facilities.TypedFactory;

using DynamicTranslator.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Configuration.UniqueIdentifier;
using DynamicTranslator.Extensions;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator
{
    public class DynamicTranslatorCoreModule : DynamicTranslatorModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.AddFacility<TypedFactoryFacility>();
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register<IUniqueIdentifierProvider, HddBasedIdentifierProvider>();
            IocManager.Register<IUniqueIdentifierProvider, CpuBasedIdentifierProvider>();
            IocManager.Resolve<DynamicTranslatorConfiguration>().Initialize();

            var existingToLanguage = AppConfigManager.Get("ToLanguage");
            var existingFromLanguage = AppConfigManager.Get("FromLanguage");

            Configurations.ApplicationConfiguration.IsLanguageDetectionEnabled = true;
            Configurations.ApplicationConfiguration.IsExtraLoggingEnabled = true;
            Configurations.ApplicationConfiguration.LeftOffset = 500;
            Configurations.ApplicationConfiguration.TopOffset = 15;
            Configurations.ApplicationConfiguration.SearchableCharacterLimit = 200;
            Configurations.ApplicationConfiguration.IsNoSqlDatabaseEnabled = true;
            Configurations.ApplicationConfiguration.MaxNotifications = 4;
            Configurations.ApplicationConfiguration.ToLanguage = new Language(existingToLanguage, LanguageMapping.All[existingToLanguage]);
            Configurations.ApplicationConfiguration.FromLanguage = new Language(existingFromLanguage, LanguageMapping.All[existingFromLanguage]);

            Configurations.ApplicationConfiguration.ClientConfiguration.CreateOrConsolidate(client =>
            {
                client.AppVersion = ApplicationVersion.GetCurrentVersion();
                client.Id = string.IsNullOrEmpty(AppConfigManager.Get("ClientId")) ? GenerateUniqueClientId() : AppConfigManager.Get("ClientId");
                client.MachineName = Environment.MachineName.Normalize();
            });

            Configurations.GoogleAnalyticsConfiguration.Url = "http://www.google-analytics.com/collect";
            Configurations.GoogleAnalyticsConfiguration.TrackingId = "UA-70082243-2";

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        private string GenerateUniqueClientId()
        {
            string uniqueId;
            try
            {
                uniqueId = IocManager.ResolveAll<IUniqueIdentifierProvider>()
                                     .BuildForAll();
            }
            catch (Exception)
            {
                uniqueId = Guid.NewGuid().ToString();
            }

            return uniqueId;
        }
    }
}