using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenContextTest
    {
        [Fact]
        public void SutIsSpecimenContext()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new SpecimenContext(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenContext>(sut);
        }

        [Fact]
        public void CreateWithNullBuilderWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new SpecimenContext(null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new SpecimenContext(expectedBuilder);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Arrange
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var sut = new SpecimenContext(builder);
            // Act
            var dummyRequest = new object();
            var result = sut.Resolve(dummyRequest);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Arrange
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
            // Act
            sut.Resolve(expectedRequest);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Arrange
            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();

            var sut = new SpecimenContext(builderMock);

            builderMock.OnCreate = (r, c) =>
            {
                Assert.Equal(sut, c);
                mockVerified = true;
                return new object();
            };
            // Act
            var dummyRequest = new object();
            sut.Resolve(dummyRequest);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }
    }
}
