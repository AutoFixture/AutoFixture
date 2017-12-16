using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit2.Addins;
using NUnit.Framework;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class CompositeDataAttributeTest
    {
        [Test]
        public void SutIsDataAttribute()
        {
            // Arrange
            // Act
            var sut = new CompositeDataAttribute();
            // Assert
            Assert.IsInstanceOf<DataAttribute>(sut);
        }

        [Test]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
        }

        [Test]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Arrange
            Action a = delegate { };
            var method = a.Method;
            
            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Assert
            Assert.True(attributes.SequenceEqual(result));
        }

        [Test]
        public void InitializeWithNullEnumerableThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute((IEnumerable<DataAttribute>)null));
        }

        [Test]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            Action a = delegate { };
            var method = a.Method;
            
            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.True(attributes.SequenceEqual(result));
        }

        [Test]
        public void GetArgumentsWithNullMethodThrows()
        {
            // Arrange
            var sut = new CompositeDataAttribute();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null).ToList());
        }

        [Test]
        public void GetArgumentsOnMethodWithNoParametersReturnsNoTheory()
        {
            // Arrange
            Action a = delegate { };
            var method = a.Method;
            
            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>())
               );

            // Act & Assert
            var result = sut.GetData(a.Method);
            Array.ForEach(result.ToArray(), Assert.IsEmpty);
        }
    }
}