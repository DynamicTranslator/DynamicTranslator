using DynamicTranslator.Configuration.UniqueIdentifier;
using DynamicTranslator.Extensions;
using DynamicTranslator.TestBase;

using NSubstitute;

using Shouldly;

using Xunit;

namespace DynamicTranslator.Tests.ConfigurationTests
{
    public class UniqueIdentifierProviderTests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void HddBasedIdentifierProvider_Should_Work()
        {
            LocalIocManager.Register<IUniqueIdentifierProvider, HddBasedIdentifierProvider>();

            var hddBasedUniqueIdenfierProvider = Resolve<IUniqueIdentifierProvider>();

            hddBasedUniqueIdenfierProvider.Get().ShouldNotBeNull();
        }

        [Fact]
        public void CpuBasedIdentifierProvider_Should_Work()
        {
            LocalIocManager.Register<IUniqueIdentifierProvider, CpuBasedIdentifierProvider>();

            var hddBasedUniqueIdenfierProvider = Resolve<IUniqueIdentifierProvider>();

            hddBasedUniqueIdenfierProvider.Get().ShouldNotBeNull();
        }

        [Theory, AutoSubstituteData]
        public void UniqueIdentifierProvider_Should_ReturnConcatKeyWithBothProvider()
        {
            IUniqueIdentifierProvider cpuBasedProvider = Substitute.For<IUniqueIdentifierProvider, CpuBasedIdentifierProvider>();
            IUniqueIdentifierProvider hddBasedProvider = Substitute.For<IUniqueIdentifierProvider, HddBasedIdentifierProvider>();

            const string cpuBasedKey = "cpuBasedKey";
            const string hddBasedKey = "hddBasedKey";

            cpuBasedProvider.Get().Returns(cpuBasedKey);
            hddBasedProvider.Get().Returns(hddBasedKey);

            // Registration order important!
            Register(hddBasedProvider);
            Register(cpuBasedProvider);

            LocalIocManager.ResolveAll<IUniqueIdentifierProvider>().BuildForAll().ShouldBe(hddBasedKey + cpuBasedKey);
        }
    }
}
