using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CreatingAbstractClassWithPublicConstructorTests
    {
        [Fact]
        public void CreateAbstractWithPublicConstructorWillThrow()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create<AbstractClassWithPublicConstructor>());
        }
    }
}
