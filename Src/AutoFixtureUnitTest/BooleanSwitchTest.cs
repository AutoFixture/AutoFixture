using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class BooleanSwitchTest
    {
        [Fact]
        public void CreateWillReturnTrueOnFirstCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            // Exercise system
            bool result = sut.Create();
            // Verify outcome
            Assert.True(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTrueOnFirstCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.True(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [Fact]
        public void CreateWillReturnFalseOnSecondCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.Create();
            // Exercise system
            bool result = sut.Create();
            // Verify outcome
            Assert.False(result, "CreateAnonymous called an even number of times");
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnFalseOnSecondCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.False(result, "CreateAnonymous called an even number of times");
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTrueOnThirdCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.True(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }
        [Fact]
        public void CreateWillReturnTrueOnThirdCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.Create();
            sut.Create();
            // Exercise system
            bool result = sut.Create();
            // Verify outcome
            Assert.True(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnFalseOnFourthCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.False(result, "CreateAnonymous called an even number of times");
            // Teardown
        }
        [Fact]
        public void CreateWillReturnFalseOnFourthCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.Create();
            sut.Create();
            sut.Create();
            // Exercise system
            bool result = sut.Create();
            // Verify outcome
            Assert.False(result, "CreateAnonymous called an even number of times");
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new BooleanSwitch();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Fixture setup
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonBooleanRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonBooleanRequest = new object();
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonBooleanRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnFirstCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.True((bool)result);
            // Teardown
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.False((bool)result);
            // Teardown
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnThirdCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.True((bool)result);
            // Teardown
        }

        [Fact]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnFourthCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.False((bool)result);
            // Teardown
        }
    }
}
