using System;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class DerivedInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DerivedInlineAutoDataAttribute(Func<IFixture> fixtureFactory, params object[] values)
            : base(fixtureFactory, values)
        {
        }
    }
}