using System;

using DynamicTranslator.Extensions;
using DynamicTranslator.Runtime;
using DynamicTranslator.TestBase;

using Shouldly;

using Xunit;

namespace DynamicTranslator.Tests.Runtime
{
    public class VersionCheckerTests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void IsNew_Should_Be_False_For_CurrentVersion()
        {
            string currentVersion = ApplicationVersion.GetCurrentVersion();
            Register<IVersionChecker, VersionChecker>();
            var sut = Resolve<IVersionChecker>();

            sut.IsNew(currentVersion).ShouldBe(false);
        }

        [Fact]
        public void IsEqual_Should_Be_True_For_CurrentVersion()
        {
            string currentVersion = ApplicationVersion.GetCurrentVersion();
            Register<IVersionChecker, VersionChecker>();
            var sut = Resolve<IVersionChecker>();

            sut.IsEqual(currentVersion).ShouldBe(true);
        }

        [Fact]
        public void IsNew_Should_Be_True_For_IncrementedVersion()
        {
            var currentVersion = new Version(ApplicationVersion.GetCurrentVersion());
            Register<IVersionChecker, VersionChecker>();

            var sut = Resolve<IVersionChecker>();
            sut.IsNew(currentVersion.IncrementMinor().ToString());
        }
    }
}
