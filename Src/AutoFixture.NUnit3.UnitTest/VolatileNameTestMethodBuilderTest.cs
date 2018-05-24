﻿using System;
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
            var anyMethod = new MethodWrapper(typeof(TestNameStrategiesFixture), nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod));
            var sut = new VolatileNameTestMethodBuilder();
            var throwingParameters = Enumerable.Range(0, 1).Select<int, object>(_ => throw new Exception());

            // Act
            var testMethod = sut.Build(anyMethod, null, throwingParameters, 0);
            // Assert
            Assert.That(testMethod.Name, Is.EqualTo(nameof(TestNameStrategiesFixture.VolatileNameDecoratedMethod)));
        }
    }
}
