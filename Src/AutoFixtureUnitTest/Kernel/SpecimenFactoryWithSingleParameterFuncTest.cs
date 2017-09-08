using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithSingleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            Func<string, object> dummyFunc = s => new object();
            // Exercise system
            var sut = new SpecimenFactory<string, object>(dummyFunc);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<int, object>((Func<int, object>)null));
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            Func<int, string> expectedFactory = i => i.ToString();
            var sut = new SpecimenFactory<int, string>(expectedFactory);
            // Exercise system
            Func<int, string> result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new SpecimenFactory<object, object>(x => x);
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

            var dtSpecimen = DateTimeOffset.Now;
            var expectedParameterRequest = typeof(DateTimeOffset);
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedParameterRequest.Equals(r) ? (object)dtSpecimen : new NoSpecimen() };

            Func<DateTimeOffset, object> f = dt => dtSpecimen.Equals(dt) ? expectedSpecimen : new NoSpecimen();
            var sut = new SpecimenFactory<DateTimeOffset, object>(f);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }
    }
}
