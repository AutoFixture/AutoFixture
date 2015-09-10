using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class CriterionTests
    {
        [Fact]
        public void SutIsEquatable()
        {
            var sut = new Criterion<string>(
                "ploeh",
                new DelegatingEqualityComparer<string>());
            Assert.IsAssignableFrom<IEquatable<string>>(sut);
        }
    }
}
