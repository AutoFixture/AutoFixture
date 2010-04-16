using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DefaultSpecimenContainerTest
    {
        [Fact]
        public void SutIsSpecimenContainer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DefaultSpecimenContainer(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenContainer>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullBuilderWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DefaultSpecimenContainer(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new DefaultSpecimenContainer(expectedBuilder);
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
            var sut = new DefaultSpecimenContainer(builder);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest);
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

            var sut = new DefaultSpecimenContainer(builderMock);
            // Exercise system
            sut.Create(expectedRequest);
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

            var sut = new DefaultSpecimenContainer(builderMock);

            builderMock.OnCreate = (r, c) =>
            {
                Assert.Equal(sut, c);
                mockVerified = true;
                return new object();
            };
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
