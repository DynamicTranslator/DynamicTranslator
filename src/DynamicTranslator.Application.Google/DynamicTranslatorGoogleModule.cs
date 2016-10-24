using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using DynamicTranslator.Application.Google.Configuration;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Google
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorGoogleModule : DynamicTranslatorModule
    {
        private const string GoogleTranslateUrl = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={0}&hl={1}&dt=t&dt=bd&dj=1&source=bubble&q={2}";

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseGoogleTranslate().WithConfigurations(configuration =>
                          {
                              configuration.Url = GoogleTranslateUrl;
                              configuration.SupportedLanguages = LanguageMapping.All.ToLanguages();
                          });

            Configurations.ModuleConfigurations.UseGoogleDetector().WithConfigurations(configuration =>
                          {
                              configuration.Url = GoogleTranslateUrl;
                          });
        }
    }
}
