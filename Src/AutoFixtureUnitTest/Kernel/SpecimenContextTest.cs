using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenContextTest
    {
        [Fact]
        public void SutIsSpecimenContext()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new SpecimenContext(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenContext>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullBuilderWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SpecimenContext(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new SpecimenContext(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var sut = new SpecimenContext(builder);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Resolve(dummyRequest);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.Equal(expectedRequest, r);
                    mockVerified = true;
                    return new object();
                };

            var sut = new SpecimenContext(builderMock);
            // Exercise system
            sut.Resolve(expectedRequest);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Fixture setup
            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();

            var sut = new SpecimenContext(builderMock);

            builderMock.OnCreate = (r, c) =>
            {
                Assert.Equal(sut, c);
                mockVerified = true;
                return new object();
            };
            // Exercise system
            var dummyRequest = new object();
            sut.Resolve(dummyRequest);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
