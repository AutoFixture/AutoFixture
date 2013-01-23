using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public abstract class FixtureResolvingItselfTests<T>
    {
        [Fact]
        public void FixtureCanResolveItself()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var actual = sut.Create<T>();
            // Verify outcome
            Assert.Equal<object>(sut, actual);
            // Teardown
        }
    }

    public class FixtureResolvingItselfTestsOfFixture : FixtureResolvingItselfTests<Fixture> { }
    public class FixtureResolvingItselfTestsOfFixtureInterface : FixtureResolvingItselfTests<IFixture> { }
    public class FixtureResolvingItselfTestsOfSpecimenBuilder : FixtureResolvingItselfTests<ISpecimenBuilder> { }
}
