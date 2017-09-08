using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithDoubleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            Func<string, int, object> dummyFunc = (x, y) => new object();
            // Exercise system
            var sut = new SpecimenFactory<string, int, object>(dummyFunc);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<int, string, object>((Func<int, string, object>)null));
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            Func<string, int, decimal> expectedFactory = (x, y) => 0m;
            var sut = new SpecimenFactory<string, int, decimal>(expectedFactory);
            // Exercise system
            Func<string, int, decimal> result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new SpecimenFactory<object, object, object>((x, y) => x);
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
            var subRequests = new[] { param1, param2 };

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => (from x in subRequests
                                        where x.ExpectedRequest.Equals(r)
                                        select x.Specimen).DefaultIfEmpty(new NoSpecimen()).SingleOrDefault();

            Func<decimal, TimeSpan, object> f = (d, ts) => param1.Specimen.Equals(d) && param2.Specimen.Equals(ts) ? expectedSpecimen : new NoSpecimen();
            var sut = new SpecimenFactory<decimal, TimeSpan, object>(f);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }
    }
}
