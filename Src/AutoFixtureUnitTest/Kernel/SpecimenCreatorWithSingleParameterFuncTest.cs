using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenCreatorWithSingleParameterFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            Func<string, object> dummyFunc = s => new object();
            // Exercise system
            var sut = new SpecimenCreator<string, object>(dummyFunc);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecimenCreator<int, object>((Func<int, object>)null));
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new SpecimenCreator<object, object>(x => x);
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
            var container = new DelegatingSpecimenContainer { OnResolve = r => expectedParameterRequest.Equals(r) ? (object)dtSpecimen : new NoSpecimen(r) };

            Func<DateTimeOffset, object> f = dt => dtSpecimen.Equals(dt) ? expectedSpecimen : new NoSpecimen(dt);
            var sut = new SpecimenCreator<DateTimeOffset, object>(f);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }
    }
}
