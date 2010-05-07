using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenCreatorWithParameterlessFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            Func<object> dummyFunc = () => new object();
            // Exercise system
            var sut = new SpecimenCreator<object>(dummyFunc);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecimenCreator<object>((Func<object>)null));
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            Func<object> creator = () => expectedSpecimen;
            var sut = new SpecimenCreator<object>(creator);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }
    }
}
