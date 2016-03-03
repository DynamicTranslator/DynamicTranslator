#region using

using System.Collections.Generic;
using DynamicTranslator.Core.Config;
using DynamicTranslator.Core.Orchestrators.Finder;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.ViewModel.Constants;
using DynamicTranslator.Test.Helper;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;

#endregion

namespace DynamicTranslator.Test.Finder
{
    public class GoogleTranslateFinderTest
    {
        [Theory, AutoDataMoq]
        public void Find_And_ThrowNothing(
            [Frozen] Mock<IMeanFinderFactory> finderFactoryMock,
            [Frozen] Mock<IMeanFinder> googleTranslateFinderMock,
            [Frozen] Mock<IStartupConfiguration> startupConfigurationMock,
            [Frozen] Mock<TranslateRequest> requestMock)
        {
            googleTranslateFinderMock.Setup(x => x.TranslatorType).Returns(TranslatorType.Google);
            finderFactoryMock.Setup(x => x.GetFinders())
                             .Returns(new List<IMeanFinder> {googleTranslateFinderMock.Object});

            requestMock.Setup(x => x.CurrentText).Returns("Hi");
            requestMock.Setup(x => x.FromLanguageExtension).Returns("en");

            startupConfigurationMock.Setup(x => x.ToLanguageExtension).Returns("tr");

            var sut = googleTranslateFinderMock.Object;

            sut.Invoking(async x => await x.Find(requestMock.Object))
               .Should()
               .Be(new TranslateResult(true, new Maybe<string>("selam")));
        }
    }
}
