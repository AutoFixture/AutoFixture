using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;

namespace AutoFixture.NUnit4.UnitTest
{
    public class FixedNameTestMethodBuilderTest
    {
        [Test]
        public void FixedNameTestMethodBuilderIsResilientToParameterEnumeratingException()
        {
            // Arrange
            var dummyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod));
            var sut = new FixedNameTestMethodBuilder();
            var throwingParameters = Enumerable.Range(0, 1).Select<int, object>(_ => throw new Exception());
            var expected = nameof(TestNameStrategiesFixture.FixedNameDecoratedMethod) + "()";

            // Act
            var testMethod = sut.Build(dummyMethod, null, throwingParameters, 0);

            // Assert
            ClassicAssert.AreEqual(expected, testMethod.Name);
        }
    }
}
