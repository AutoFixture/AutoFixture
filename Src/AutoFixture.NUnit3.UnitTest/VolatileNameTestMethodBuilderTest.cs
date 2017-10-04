using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    public class VolatileNameTestMethodBuilderTest
    {
        [Test]
        public void VolatileNameTestMethodBuilderIsResilientToFactoryException()
        {
            // Fixture setup
            var anyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod));
            var sut = new VolatileNameTestMethodBuilder();
            // Exercise system
            var testMethod = sut.Build(anyMethod, null, () => throw new Exception(), 0);
            // Verify outcome
            Assert.That(testMethod.Name, Is.EqualTo(nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod)));
            // Teardown
        }
    }
}
