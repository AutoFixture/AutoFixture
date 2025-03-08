using System;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

public class DerivedAutoDataAttribute : AutoDataAttribute
{
    public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
        : base(fixtureFactory)
    {
    }
}