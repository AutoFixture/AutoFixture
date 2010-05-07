using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ValueIgnoringSeedUnwrapperTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ValueIgnoringSeedUnwrapper();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new ValueIgnoringSeedUnwrapper();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new ValueIgnoringSeedUnwrapper();
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
            var unableContainer = new DelegatingSpecimenContainer { OnResolve = r => new NoSpecimen(anonymousSeed) };
            var sut = new ValueIgnoringSeedUnwrapper();
            // Exercise system
            var result = sut.Create(anonymousSeed, unableContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(anonymousSeed);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromSeedWhenContainerCanSatisfyWrappedRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var anonymousSeed = new SeededRequest(typeof(object), new object());

            var expectedResult = new object();
            var container = new DelegatingSpecimenContainer { OnResolve = r => expectedResult };

            var sut = new ValueIgnoringSeedUnwrapper();
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
            var sut = new ValueIgnoringSeedUnwrapper();
            var seededRequest = new SeededRequest(typeof(int), 1);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
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
