using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using DynamicTranslator.Bing;
using DynamicTranslator.Extensions;
using DynamicTranslator.Google;
using DynamicTranslator.SesliSozluk;
using DynamicTranslator.Tureng;
using DynamicTranslator.Yandex;

using Octokit;

namespace DynamicTranslator.Wpf
{
    [DependsOn(
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
            IocManager.Register<GitHubClient>(new GitHubClient(new ProductHeaderValue(Configurations.ApplicationConfiguration.GitHubRepositoryName)),
                DependencyLifeStyle.Transient);
        }
    }
}