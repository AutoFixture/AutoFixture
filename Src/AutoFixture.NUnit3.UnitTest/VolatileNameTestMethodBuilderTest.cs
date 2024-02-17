using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3.UnitTest
{
    public class VolatileNameTestMethodBuilderTest
    {
        [Test]
        public void VolatileNameTestMethodBuilderIsResilientToParameterEnumerationException()
        {
            // Arrange
            const string methodName = nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod);
            var anyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), methodName);
            var sut = new VolatileNameTestMethodBuilder();
            var throwingParameters = Enumerable.Range(0, 1).Select<int, object>(_ => throw new Exception());
            const string expected = methodName + "()";

            // Act
            var testMethod = sut.Build(anyMethod, null, throwingParameters, 0);

            // Assert
            Assert.AreEqual(expected, testMethod.Name);
        }
    }
}
