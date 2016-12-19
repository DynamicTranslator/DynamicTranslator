using System.Collections.Generic;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Application.Bing.Configuration;
using DynamicTranslator.Application.Bing.Orchestration;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.LanguageManagement;
using DynamicTranslator.TestBase;

using NSubstitute;

using Shouldly;

using Xunit;

namespace DynamicTranslator.Application.Tests.BingTests
{
    public class BingTranslatorMeanFinderTests : TestBaseWithLocalIocManager
    {
        [Fact]
        public async void Finder_Should_Work()
        {
            IApplicationConfiguration applicationConfiguration = Substitute.For<IApplicationConfiguration, ApplicationConfiguration>();
            IBingTranslatorConfiguration bingTranslatorConfiguration = Substitute.For<IBingTranslatorConfiguration, BingTranslatorConfiguration>();
            var translateRequest = new TranslateRequest("currentText", "en");

            applicationConfiguration.FromLanguage.Returns(new Language("English", "en"));
            applicationConfiguration.ToLanguage.Returns(new Language("Turkish", "tr"));

            bingTranslatorConfiguration.Url.Returns("bingUri");
            bingTranslatorConfiguration.SupportedLanguages.Returns(
                new List<Language>
                {
                    new Language("English", "en"),
                    new Language("Turkish", "tr")
                });

            LocalIocManager.IocContainer.Register(
                Component.For<IMeanOrganizerFactory>().AsFactory()
            );

            Register(applicationConfiguration);
            Register(bingTranslatorConfiguration);
            Register<BingTranslatorMeanFinder>();

            var sut = Resolve<BingTranslatorMeanFinder>();
            (await sut.Find(translateRequest)).IsSuccess.ShouldBe(true);
        }
    }
}
