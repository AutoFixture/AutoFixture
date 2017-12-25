using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithTripleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            Func<string, int, decimal, object> dummyFunc = (x, y, z) => new object();
            // Act
            var sut = new SpecimenFactory<string, int, decimal, object>(dummyFunc);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<bool, int, string, object>((Func<bool, int, string, object>)null));
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Arrange
            Func<Random, Uri, string, int> expectedFactory = (x, y, z) => 0;
            var sut = new SpecimenFactory<Random, Uri, string, int>(expectedFactory);
            // Act
            Func<Random, Uri, string, int> result = sut.Factory;
            // Assert
            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Arrange
            var sut = new SpecimenFactory<object, object, object, object>((x, y, z) => x);
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
            var param3 = new { ExpectedRequest = typeof(string), Specimen = (object)"Anonymous value - with Foo!" };
            var subRequests = new[] { param1, param2, param3 };

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => (from x in subRequests
                                        where x.ExpectedRequest.Equals(r)
                                        select x.Specimen).DefaultIfEmpty(new NoSpecimen()).SingleOrDefault();

            Func<decimal, TimeSpan, string, object> f = (d, ts, s) =>
                param1.Specimen.Equals(d) && param2.Specimen.Equals(ts) && param3.Specimen.Equals(s) ? expectedSpecimen : new NoSpecimen();
            var sut = new SpecimenFactory<decimal, TimeSpan, string, object>(f);
            // Act
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
