using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Yandex
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorYandexModule : DynamicTranslatorModule
    {
        private const string Url = "https://translate.yandex.net/api/v1.5/tr/translate?";
        private const string AnonymousUrl = "https://translate.yandex.net/api/v1/tr.json/translate?";

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IMeanOrganizer, YandexMeanOrganizer>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanFinder, YandexFinder>(DependencyLifeStyle.Transient);

            Configurations.ModuleConfigurations.UseYandexDetector().WithConfigurations(configuration => { configuration.Url = Url; });

            Configurations.ModuleConfigurations.UseYandexTranslate().WithConfigurations(configuration =>
                           {
                               configuration.ShouldBeAnonymous = false;
                               configuration.Url = configuration.ShouldBeAnonymous ? AnonymousUrl : Url;
                               configuration.ApiKey = AppConfigManager.Get("YandexApiKey");
                               configuration.SupportedLanguages = LanguageMapping.Yandex.ToLanguages();
                           });
        }
    }
}