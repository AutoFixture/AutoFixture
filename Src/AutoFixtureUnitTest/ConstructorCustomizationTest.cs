using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class ConstructorCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Arrange
            var dummyType = typeof(object);
            var dummyQuery = new DelegatingMethodQuery();
            // Act
            var sut = new ConstructorCustomization(dummyType, dummyQuery);
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Arrange
            var dummyQuery = new DelegatingMethodQuery();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorCustomization(null, dummyQuery));
        }

        [Fact]
        public void InitializeWithNullQueryThrows()
        {
            // Arrange
            var dummyType = typeof(object);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorCustomization(dummyType, null));
        }

        [Fact]
        public void TargetTypeIsCorrect()
        {
            // Arrange
            var expectedType = typeof(string);
            var dummyQuery = new DelegatingMethodQuery();
            var sut = new ConstructorCustomization(expectedType, dummyQuery);
            // Act
            Type result = sut.TargetType;
            // Assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        public void QueryIsCorrect()
        {
            // Arrange
            var dummyType = typeof(object);
            var expectedQuery = new DelegatingMethodQuery();
            var sut = new ConstructorCustomization(dummyType, expectedQuery);
            // Act
            IMethodQuery result = sut.Query;
            // Assert
            Assert.Equal(expectedQuery, result);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var dummyType = typeof(object);
            var dummyQuery = new DelegatingMethodQuery();
            var sut = new ConstructorCustomization(dummyType, dummyQuery);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeWithGreedyQueryCorrectlyCustomizesFixture()
        {
            // Arrange
            var fixture = new Fixture();

            var type = typeof(MultiUnorderedConstructorType);
            var query = new GreedyConstructorQuery();
            var sut = new ConstructorCustomization(type, query);
            // Act
            sut.Customize(fixture);
            // Assert
            var specimen = fixture.Create<MultiUnorderedConstructorType>();
            Assert.False(string.IsNullOrEmpty(specimen.Text));
            Assert.NotEqual(0, specimen.Number);
        }

        [Fact]
        public void CustomizeWithModestQueryCorrectlyCustomizesFixture()
        {
            // Arrange
            var fixture = new Fixture();

            var type = typeof(MultiUnorderedConstructorType);
            var query = new ModestConstructorQuery();
            var sut = new ConstructorCustomization(type, query);
            // Act
            sut.Customize(fixture);
            // Assert
            var specimen = fixture.Create<MultiUnorderedConstructorType>();
            Assert.True(string.IsNullOrEmpty(specimen.Text));
            Assert.Equal(0, specimen.Number);
        }
    }
}
