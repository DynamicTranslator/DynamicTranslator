using System.Reflection;

using Abp.Modules;

using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Yandex
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorYandexModule : DynamicTranslatorModule
    {
        private const string AnonymousUrl = "https://translate.yandex.net/api/v1/tr.json/translate?";
        private const string BaseUrl = "https://translate.yandex.com/";
        private const string InternalSId = "id=93bdaee7.57bb46e3.e787b736-0-0";
        private const string Url = "https://translate.yandex.net/api/v1.5/tr/translate?";

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseYandexDetector().WithConfigurations(configuration => { configuration.Url = Url; });

            Configurations.ModuleConfigurations.UseYandexTranslate().WithConfigurations(configuration =>
                          {
                              configuration.ShouldBeAnonymous = false;
                              configuration.BaseUrl = BaseUrl;
                              configuration.SId = InternalSId;
                              configuration.Url = configuration.ShouldBeAnonymous ? AnonymousUrl : Url;
                              configuration.ApiKey = AppConfigManager.Get("YandexApiKey");
                              configuration.SupportedLanguages = LanguageMapping.Yandex.ToLanguages();
                          });
        }
    }
}
