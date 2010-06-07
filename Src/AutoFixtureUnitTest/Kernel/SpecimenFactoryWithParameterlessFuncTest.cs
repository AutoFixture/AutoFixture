using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenFactoryWithParameterlessFuncTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            Func<object> dummyFunc = () => new object();
            // Exercise system
            var sut = new SpecimenFactory<object>(dummyFunc);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new SpecimenFactory<object>((Func<object>)null));
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            Func<OperatingSystem> expectedFactory = () => new OperatingSystem(PlatformID.WinCE, new Version(10, 8));
            var sut = new SpecimenFactory<OperatingSystem>(expectedFactory);
            // Exercise system
            Func<OperatingSystem> result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            Func<object> creator = () => expectedSpecimen;
            var sut = new SpecimenFactory<object>(creator);
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
