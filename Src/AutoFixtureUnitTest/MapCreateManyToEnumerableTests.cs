using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MapCreateManyToEnumerableTests
    {
        [Fact]
        public void SutIsCustomization()
        {
            var sut = new MapCreateManyToEnumerable();
            Assert.IsAssignableFrom<ICustomization>(sut);
        }
    }
}
