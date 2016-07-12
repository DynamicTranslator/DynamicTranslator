using System.Reflection;

using Abp.Modules;

using DynamicTranslator.Application;
using DynamicTranslator.Bing;
using DynamicTranslator.Google;
using DynamicTranslator.SesliSozluk;
using DynamicTranslator.Tureng;
using DynamicTranslator.Yandex;

namespace DynamicTranslator.Wpf
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule),
        typeof(DynamicTranslatorGoogleModule),
        typeof(DynamicTranslatorYandexModule),
        typeof(DynamicTranslatorBingModule),
        typeof(DynamicTranslatorTurengModule),
        typeof(DynamicTranslatorSesliSozlukModule)
        )]
    public class DynamicTranslatorWpfModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}