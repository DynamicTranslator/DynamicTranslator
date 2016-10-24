using System.Reflection;

using Abp.Modules;

using DynamicTranslator.Application.Zargan.Configuration;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Zargan
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorZarganModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseZarganTranslate().WithConfigurations(configuration =>
                          {
                              configuration.Url = "";
                              configuration.SupportedLanguages = LanguageMapping.All.ToLanguages();
                          });
        }
    }
}
