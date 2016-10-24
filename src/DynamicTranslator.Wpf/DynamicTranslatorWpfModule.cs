using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using DynamicTranslator.Application.Bing;
using DynamicTranslator.Application.Google;
using DynamicTranslator.Application.SesliSozluk;
using DynamicTranslator.Application.Tureng;
using DynamicTranslator.Application.Yandex;
using DynamicTranslator.Extensions;

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
            IocManager.Register<GitHubClient>(new GitHubClient(new ProductHeaderValue(Configurations.ApplicationConfiguration.GitHubRepositoryName)), DependencyLifeStyle.Transient);
        }
    }
}