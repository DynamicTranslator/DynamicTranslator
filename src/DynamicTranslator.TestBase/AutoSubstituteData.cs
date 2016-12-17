using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit2;

namespace DynamicTranslator.TestBase
{
    public class AutoSubstituteData : AutoDataAttribute
    {
        public AutoSubstituteData() : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
