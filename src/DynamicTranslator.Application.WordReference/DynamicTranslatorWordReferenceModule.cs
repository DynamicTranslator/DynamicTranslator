using System.Reflection;

using Abp.Modules;

using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.WordReference
{
    [DependsOn(
        typeof(DynamicTranslatorApplicationModule)
    )]
    public class DynamicTranslatorWordReferenceModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseWordReferenceTranslator().WithConfigurations(configuration =>
            {
                configuration.Url = "http://www.wordreference.com/redirect/translation.aspx?w={0}&dict={1}";
                configuration.SupportedLanguages = LanguageMapping.WordReference.ToLanguages();
            });
        }
    }
}
