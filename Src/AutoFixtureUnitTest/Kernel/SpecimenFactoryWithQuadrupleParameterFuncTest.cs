using System;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithQuadrupleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            Func<bool, string, int, decimal, object> dummyFunc = (x, y, z, æ) => new object();
            // Exercise system
            var sut = new SpecimenFactory<bool, string, int, decimal, object>(dummyFunc);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<bool, int, DateTime, string, object>((Func<bool, int, DateTime, string, object>)null));
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            Func<ConcreteType, TimeZoneInfo, StringBuilder, DateTime, ConsoleColor> expectedFactory = (x, y, z, æ) => ConsoleColor.Black;
            var sut = new SpecimenFactory<ConcreteType, TimeZoneInfo, StringBuilder, DateTime, ConsoleColor>(expectedFactory);
            // Exercise system
            Func<ConcreteType, TimeZoneInfo, StringBuilder, DateTime, ConsoleColor> result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new SpecimenFactory<object, object, object, object, object>((x, y, z, æ) => x);
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
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
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }
    }
}
