using System;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    /// <summary>
    /// A stub of <see cref="AutoDataAttribute"/> for the benefit of unit testing
    /// </summary>
    public class AutoDataAttributeStub : AutoDataAttribute
    {
        [Obsolete]
        public AutoDataAttributeStub(IFixture fixture)
            : base(fixture)
        {
        }

        public AutoDataAttributeStub(Func<IFixture> fixtureFactory)
            : base(fixtureFactory)
        {
        }
    }
}