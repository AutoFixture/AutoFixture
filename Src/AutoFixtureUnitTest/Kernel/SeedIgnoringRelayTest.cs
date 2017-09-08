using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SeedIgnoringRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new SeedIgnoringRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new SeedIgnoringRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new SeedIgnoringRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(()=>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateFromSeedWhenContainerCannotSatisfyWrappedRequestWillReturnNull()
        {
            // Fixture setup
            var anonymousSeed = new SeededRequest(typeof(object), new object());
            var unableContainer = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new SeedIgnoringRelay();
            // Exercise system
            var result = sut.Create(anonymousSeed, unableContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromSeedWhenContainerCanSatisfyWrappedRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var anonymousSeed = new SeededRequest(typeof(object), new object());

            var expectedResult = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedResult };

            var sut = new SeedIgnoringRelay();
            // Exercise system
            var result = sut.Create(anonymousSeed, container);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromSeedWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new SeedIgnoringRelay();
            var seededRequest = new SeededRequest(typeof(int), 1);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(typeof(int), r);
                mockVerified = true;
                return null;
            };
            // Exercise system
            sut.Create(seededRequest, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
