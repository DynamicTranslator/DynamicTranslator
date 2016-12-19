using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Application.Bing.Configuration;
using DynamicTranslator.Application.Bing.Orchestration;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;
using DynamicTranslator.TestBase;

using NSubstitute;

using RestSharp;

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
            applicationConfiguration.FromLanguage.Returns(new Language("English", "en"));
            applicationConfiguration.ToLanguage.Returns(new Language("Turkish", "tr"));

            IBingTranslatorConfiguration bingTranslatorConfiguration = Substitute.For<IBingTranslatorConfiguration, BingTranslatorConfiguration>();
            bingTranslatorConfiguration.Url.Returns("bingUri");
            bingTranslatorConfiguration.CanSupport().Returns(true);
            bingTranslatorConfiguration.IsActive().Returns(true);

            var bingMeanOrganizer = Substitute.For<BingTranslatorMeanOrganizer>();
            bingMeanOrganizer.TranslatorType.Returns(TranslatorType.Bing);
            bingMeanOrganizer.OrganizeMean(Arg.Any<string>()).Returns(Task.FromResult(new Maybe<string>("selam")));

            var meanOrganizerFactory = Substitute.For<IMeanOrganizerFactory>();
            meanOrganizerFactory.GetMeanOrganizers().Returns(new List<IMeanOrganizer> { bingMeanOrganizer });
            LocalIocManager.IocContainer.Register(
                Component.For<IMeanOrganizerFactory>().Instance(meanOrganizerFactory).AsFactory()
            );

            var restClient = Substitute.For<IRestClient>();
            restClient.ExecutePostTaskAsync(Arg.Any<RestRequest>()).Returns(Task.FromResult<IRestResponse>(new RestResponse { StatusCode = HttpStatusCode.OK }));

            Register(restClient);
            Register(applicationConfiguration);
            Register(bingTranslatorConfiguration);
            Register<BingTranslatorMeanFinder>();

            var translateRequest = new TranslateRequest(currentText: "hi", fromLanguageExtension: "en");
            var sut = Resolve<BingTranslatorMeanFinder>();
            (await sut.Find(translateRequest)).IsSuccess.ShouldBe(true);
            (await sut.Find(translateRequest)).ResultMessage.ShouldBe(new Maybe<string>("selam"));
        }
    }
}
