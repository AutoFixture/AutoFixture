using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class FreezingCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            var dummyType = typeof(object);
            // Act
            var sut = new FreezingCustomization(dummyType);
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullTargetTypeThrowsArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FreezingCustomization(null));
        }

        [Fact]
        public void InitializeWithNullRegisteredTypeThrowsArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FreezingCustomization(typeof(object), null));
        }

        [Fact]
        public void InitializeWithRegisteredTypeIncompatibleWithTargetTypeThrowsArgumentException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(() =>
                new FreezingCustomization(typeof(int), typeof(string)));
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Arrange
            var expectedType = typeof(string);
            var sut = new FreezingCustomization(expectedType);
            // Act
            Type result = sut.TargetType;
            // Assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        public void RegisteredTypeIsCorrect()
        {
            // Arrange
            var targetType = typeof(string);
            var registeredType = typeof(object);
            var sut = new FreezingCustomization(targetType, registeredType);
            // Act
            Type result = sut.RegisteredType;
            // Assert
            Assert.Equal(registeredType, result);
        }

        [Fact]
        public void TargetTypeAndRegisteredTypeAreCorrect()
        {
            // Arrange
            var targetType = typeof(string);
            var registeredType = typeof(object);
            var sut = new FreezingCustomization(targetType, registeredType);
            // Act & assert
            Assert.Equal(targetType, sut.TargetType);
            Assert.Equal(registeredType, sut.RegisteredType);
        }

        [Fact]
        public void TargetTypeIsTheSameAsRegisteredTypeWhenOnlyTargetTypeIsSpecified()
        {
            // Arrange
            var targetType = typeof(string);
            var sut = new FreezingCustomization(targetType);
            // Act & assert
            Assert.Equal(sut.TargetType, sut.RegisteredType);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var dummyType = typeof(object);
            var sut = new FreezingCustomization(dummyType);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeCorrectlyCustomizesFixture()
        {
            // Arrange
            var targetType = typeof(int);
            var fixture = new Fixture();

            var sut = new FreezingCustomization(targetType);
            // Act
            sut.Customize(fixture);
            // Assert
            var i1 = fixture.Create<int>();
            var i2 = fixture.Create<int>();
            Assert.Equal(i1, i2);
        }

        [Fact]
        public void CustomizeWithRegisteredTypeCorrectlyCustomizesFixture()
        {
            // Arrange
            var targetType = typeof(int);
            var registeredType = typeof(object);
            var fixture = new Fixture();
            var sut = new FreezingCustomization(targetType, registeredType);
            // Act
            sut.Customize(fixture);
            // Assert
            object i1 = fixture.Create<int>();
            object i2 = fixture.Create<object>();
            Assert.Equal(i1, i2);
        }
    }
}
