using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3.UnitTest
{
    public class FixedNameTestMethodBuilderTest
    {
        [Test]
        public void FixedNameTestMethodBuilderIsResilientToParameterEnumeratingException()
        {
            // Fixture setup
            var dummyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            var sut = new FixedNameTestMethodBuilder();
            var throwingParameters = Enumerable.Range(0,1).Select<int, object>(_ => throw new Exception());
            // Exercise system
            var testMethod = sut.Build(dummyMethod, null, throwingParameters, 0);
            // Verify outcome
            Assert.That(testMethod.Name, Is.EqualTo(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod)));
            // Teardown
        }
    }
}
