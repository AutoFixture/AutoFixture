using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithDoubleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            Func<string, int, object> dummyFunc = (x, y) => new object();
            // Act
            var sut = new SpecimenFactory<string, int, object>(dummyFunc);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<int, string, object>((Func<int, string, object>)null));
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Arrange
            Func<string, int, decimal> expectedFactory = (x, y) => 0m;
            var sut = new SpecimenFactory<string, int, decimal>(expectedFactory);
            // Act
            Func<string, int, decimal> result = sut.Factory;
            // Assert
            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Arrange
            var sut = new SpecimenFactory<object, object, object>((x, y) => x);
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

            var param1 = new { ExpectedRequest = typeof(decimal), Specimen = (object)1m };
            var param2 = new { ExpectedRequest = typeof(TimeSpan), Specimen = (object)TimeSpan.FromDays(1) };
            var subRequests = new[] { param1, param2 };

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => (from x in subRequests
                                        where x.ExpectedRequest.Equals(r)
                                        select x.Specimen).DefaultIfEmpty(new NoSpecimen()).SingleOrDefault();

            Func<decimal, TimeSpan, object> f = (d, ts) => param1.Specimen.Equals(d) && param2.Specimen.Equals(ts) ? expectedSpecimen : new NoSpecimen();
            var sut = new SpecimenFactory<decimal, TimeSpan, object>(f);
            // Act
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
