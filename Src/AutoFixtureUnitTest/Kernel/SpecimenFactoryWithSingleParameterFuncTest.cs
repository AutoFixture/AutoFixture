using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithSingleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            Func<string, object> dummyFunc = s => new object();
            // Act
            var sut = new SpecimenFactory<string, object>(dummyFunc);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<int, object>((Func<int, object>)null));
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Arrange
            Func<int, string> expectedFactory = i => i.ToString();
            var sut = new SpecimenFactory<int, string>(expectedFactory);
            // Act
            Func<int, string> result = sut.Factory;
            // Assert
            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Arrange
            var sut = new SpecimenFactory<object, object>(x => x);
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Arrange
            var expectedSpecimen = new object();

            var dtSpecimen = DateTimeOffset.Now;
            var expectedParameterRequest = typeof(DateTimeOffset);
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedParameterRequest.Equals(r) ? (object)dtSpecimen : new NoSpecimen() };

            Func<DateTimeOffset, object> f = dt => dtSpecimen.Equals(dt) ? expectedSpecimen : new NoSpecimen();
            var sut = new SpecimenFactory<DateTimeOffset, object>(f);
            // Act
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
