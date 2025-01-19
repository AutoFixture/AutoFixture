using System;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class DerivedAutoDataAttribute : AutoDataAttribute
    {
        public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
            : base(fixtureFactory)
        {
        }
    }
}
