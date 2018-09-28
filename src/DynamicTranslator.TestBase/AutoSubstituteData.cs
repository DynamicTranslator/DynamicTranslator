using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace DynamicTranslator.TestBase
{
    public class AutoSubstituteData : AutoDataAttribute
    {
        public AutoSubstituteData() : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}