using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class SignatureMethodQueryTests
    {
        [Fact]
        public void SutIsIMethodQuery()
        {
            Action dummy = delegate { };
            var sut = new SignatureMethodQuery(dummy.Method);
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void InitializeWithNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new SignatureMethodQuery(null));
        }

    }
}
