using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    public class FixedNameTestMethodBuilderTest
    {
        [Test]
        public void FixedNameTestMethodBuilderIsResilientToFactoryException()
        {
            // Fixture setup
            var dummyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            var sut = new FixedNameTestMethodBuilder();
            // Exercise system
            var testMethod = sut.Build(dummyMethod, null, () => throw new Exception(), 0);
            // Verify outcome
            Assert.That(testMethod.Name, Is.EqualTo(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod)));
            // Teardown
        }
    }
}
