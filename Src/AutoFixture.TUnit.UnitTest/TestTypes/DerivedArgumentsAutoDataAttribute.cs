using System;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    internal class DerivedArgumentsAutoDataAttribute : ArgumentsAutoDataAttribute
    {
        public DerivedArgumentsAutoDataAttribute(Func<IFixture> fixtureFactory, params object[] values)
            : base(fixtureFactory, values)
        {
        }
    }
}