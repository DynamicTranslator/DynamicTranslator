using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Bing.Configuration;
using DynamicTranslator.Application.Google.Configuration;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Prompt.Configuration;
using DynamicTranslator.Application.SesliSozluk.Configuration;
using DynamicTranslator.Application.Tureng.Configuration;
using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

using NSubstitute;

using RestSharp;

namespace DynamicTranslator.TestBase
{
    public class FinderTestBase<TSut, TConfig, TConfigImplementation, TMeanOrganizer> : TestBaseWithLocalIocManager
        where TConfigImplementation : class, TConfig
        where TConfig : class, IMustHaveUrl
        where TMeanOrganizer : AbstractMeanOrganizer, IMeanOrganizer
        where TSut : class
    {
        protected FinderTestBase()
        {
            ApplicationConfiguration = Substitute.For<IApplicationConfiguration, ApplicationConfiguration>();
            ApplicationConfiguration.FromLanguage.Returns(new Language("English", "en"));
            ApplicationConfiguration.ToLanguage.Returns(new Language("Turkish", "tr"));
            Register(ApplicationConfiguration);

            RestClient = Substitute.For<IRestClient>();
            Register(RestClient);

            TranslatorConfiguration = Substitute.For<TConfig, TConfigImplementation>();
            TranslatorConfiguration.Url.Returns("http://www.dummycorrecturl.com/");
            Register(TranslatorConfiguration);

            MeanOrganizer = Substitute.For<IMeanOrganizer, TMeanOrganizer>();
            MeanOrganizer.TranslatorType.Returns(FindTranslatorType());
            MeanOrganizer.OrganizeMean(Arg.Any<string>()).Returns(Task.FromResult(new Maybe<string>("selam")));
            MeanOrganizer.OrganizeMean(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(new Maybe<string>("selam")));
            Register((TMeanOrganizer)MeanOrganizer, DependencyLifeStyle.Transient);

            Register<TSut>();
        }

        protected IApplicationConfiguration ApplicationConfiguration { get; set; }

        protected IMeanOrganizer MeanOrganizer { get; set; }

        protected TConfig TranslatorConfiguration { get; set; }

        protected IRestClient RestClient { get; set; }

        protected TSut ResolveSut()
        {
            return Resolve<TSut>();
        }

        private TranslatorType FindTranslatorType()
        {
            if (typeof(TConfig) == typeof(IGoogleTranslatorConfiguration))
            {
                return TranslatorType.Google;
            }

            if (typeof(TConfig) == typeof(IBingTranslatorConfiguration))
            {
                return TranslatorType.Bing;
            }

            if (typeof(TConfig) == typeof(IYandexTranslatorConfiguration))
            {
                return TranslatorType.Yandex;
            }

            if (typeof(TConfig) == typeof(IPromptTranslatorConfiguration))
            {
                return TranslatorType.Prompt;
            }

            if (typeof(TConfig) == typeof(ISesliSozlukTranslatorConfiguration))
            {
                return TranslatorType.SesliSozluk;
            }

            if (typeof(TConfig) == typeof(ITurengTranslatorConfiguration))
            {
                return TranslatorType.Tureng;
            }

            return default(TranslatorType);
        }
    }
}
