using System;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class DerivedAutoDataAttribute : AutoDataAttribute
    {
        public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
            : base(fixtureFactory)
        {
        }
    }
}
