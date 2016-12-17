using DynamicTranslator.Configuration;
using DynamicTranslator.TestBase;

using NSubstitute;

using Xunit;

namespace DynamicTranslator.Tests.Configuration
{
    public class AppConfigManagerTests
    {
        [Theory]
        [AutoSubstituteData]
        public void Get_Should_Work(IAppConfigManager fakeAppConfigManager)
        {
            fakeAppConfigManager.Get("SomeKey").Returns("SomeValue");
        }
    }
}
