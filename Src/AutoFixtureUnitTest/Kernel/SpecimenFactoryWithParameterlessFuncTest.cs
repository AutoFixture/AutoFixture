using System;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithParameterlessFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            Func<object> dummyFunc = () => new object();
            // Act
            var sut = new SpecimenFactory<object>(dummyFunc);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<object>((Func<object>)null));
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Arrange
            Func<ConcreteType> expectedFactory = () => new ConcreteType(42, "42");
            var sut = new SpecimenFactory<ConcreteType>(expectedFactory);
            // Act
            Func<ConcreteType> result = sut.Factory;
            // Assert
            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var expectedSpecimen = new object();
            Func<object> creator = () => expectedSpecimen;
            var sut = new SpecimenFactory<object>(creator);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
