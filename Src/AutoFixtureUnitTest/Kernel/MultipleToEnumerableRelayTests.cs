using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultipleToEnumerableRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new MultipleToEnumerableRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }
    }
}
