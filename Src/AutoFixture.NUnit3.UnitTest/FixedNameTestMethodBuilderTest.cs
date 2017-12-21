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
            // Arrange
            var dummyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            var sut = new FixedNameTestMethodBuilder();
            var throwingParameters = Enumerable.Range(0,1).Select<int, object>(_ => throw new Exception());
            // Act
            var testMethod = sut.Build(dummyMethod, null, throwingParameters, 0);
            // Assert
            Assert.That(testMethod.Name, Is.EqualTo(nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod)));
        }
    }
}
