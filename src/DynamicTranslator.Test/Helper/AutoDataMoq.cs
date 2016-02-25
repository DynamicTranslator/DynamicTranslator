#region using

using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

#endregion

namespace DynamicTranslator.Test.Helper
{
    public class AutoDataMoq : AutoDataAttribute
    {
        public AutoDataMoq()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }

        public AutoDataMoq(Type type, params object[] parameters)
            : this()
        {
            object obj = Activator.CreateInstance(type, parameters);

            base.Fixture.Customize(obj as ICustomization);
        }
    }
}
