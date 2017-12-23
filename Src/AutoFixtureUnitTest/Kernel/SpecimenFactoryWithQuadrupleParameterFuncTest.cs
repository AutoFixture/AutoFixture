using System;
using System.Linq;
using System.Text;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithQuadrupleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            Func<bool, string, int, decimal, object> dummyFunc = (x, y, z, æ) => new object();
            // Act
            var sut = new SpecimenFactory<bool, string, int, decimal, object>(dummyFunc);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<bool, int, DateTime, string, object>((Func<bool, int, DateTime, string, object>)null));
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Arrange
            Func<ConcreteType, TimeZoneInfo, StringBuilder, DateTime, ConsoleColor> expectedFactory = (x, y, z, æ) => ConsoleColor.Black;
            var sut = new SpecimenFactory<ConcreteType, TimeZoneInfo, StringBuilder, DateTime, ConsoleColor>(expectedFactory);
            // Act
            Func<ConcreteType, TimeZoneInfo, StringBuilder, DateTime, ConsoleColor> result = sut.Factory;
            // Assert
            Assert.Equal(expectedFactory, result);
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Arrange
            var sut = new SpecimenFactory<object, object, object, object, object>((x, y, z, æ) => x);
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
            var param4 = new { ExpectedRequest = typeof(int), Specimen = (object)7 };
            var subRequests = new[] { param1, param2, param3, param4 };

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => (from x in subRequests
                                        where x.ExpectedRequest.Equals(r)
                                        select x.Specimen).DefaultIfEmpty(new NoSpecimen()).SingleOrDefault();

            Func<decimal, TimeSpan, string, int, object> f = (d, ts, s, i) =>
                param1.Specimen.Equals(d) && param2.Specimen.Equals(ts) && param3.Specimen.Equals(s) && param4.Specimen.Equals(i) ? expectedSpecimen : new NoSpecimen();
            var sut = new SpecimenFactory<decimal, TimeSpan, string, int, object>(f);
            // Act
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
