using System;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class WritablePropertyAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new WritablePropertyAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new WritablePropertyAssertion(expectedComposer);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new WritablePropertyAssertion(null));
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new WritablePropertyAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
        }

        [Fact]
        public void VerifyIllBehavedPropertyGetterThrows()
        {
            // Arrange
            var composer = new Fixture();
            var sut = new WritablePropertyAssertion(composer);
            
            var propertyInfo = typeof(IllBehavedPropertyHolder<object>).GetProperty("PropertyIllBehavedGet");
            // Act & Assert
            var e = Assert.Throws<WritablePropertyException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.PropertyInfo);
        }

        [Fact]
        public void VerifyIllBehavedPropertySetterThrows()
        {
            // Arrange
            var composer = new Fixture();
            var sut = new WritablePropertyAssertion(composer);

            var propertyInfo = typeof(IllBehavedPropertyHolder<object>).GetProperty("PropertyIllBehavedSet");
            // Act & Assert
            var e = Assert.Throws<WritablePropertyException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.PropertyInfo);
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new WritablePropertyAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyWellBehavedWritablePropertyDoesNotThrow()
        {
            // Arrange
            var composer = new Fixture();
            var sut = new WritablePropertyAssertion(composer);

            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(propertyInfo)));
        }
    }
}
