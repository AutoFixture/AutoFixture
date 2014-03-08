using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitEnumerableParameterRequestRelayTests
    {
        [Fact]
        public void SutIsSpecimentBuilder()
        {
            var sut = new OmitEnumerableParameterRequestRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }
    }
}
