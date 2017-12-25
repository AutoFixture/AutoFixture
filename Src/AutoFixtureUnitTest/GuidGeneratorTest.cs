using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class GuidGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnNonDefaultGuid()
        {
            // Arrange
            var unexpectedGuid = default(Guid);
            // Act
            var result = GuidGenerator.CreateAnonymous();
            // Assert
            Assert.NotEqual<Guid>(unexpectedGuid, result);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnNonDefaultGuid()
        {
            // Arrange
            var unexpectedGuid = default(Guid);
            // Act
            var result = GuidGenerator.Create();
            // Assert
            Assert.NotEqual<Guid>(unexpectedGuid, result);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousTwiceWillReturnDifferentValues()
        {
            // Arrange
            var unexpectedGuid = GuidGenerator.CreateAnonymous();
            // Act
            var result = GuidGenerator.CreateAnonymous();
            // Assert
            Assert.NotEqual<Guid>(unexpectedGuid, result);
        }

        [Fact]
        [Obsolete]
        public void CreateTwiceWillReturnDifferentValues()
        {
            // Arrange
            var unexpectedGuid = GuidGenerator.Create();
            // Act
            var result = GuidGenerator.Create();
            // Assert
            Assert.NotEqual<Guid>(unexpectedGuid, result);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new GuidGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new GuidGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var sut = new GuidGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonGuidRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonGuidRequest = new object();
            var sut = new GuidGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(nonGuidRequest, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithGuidRequestWillReturnCorrectResult()
        {
            // Arrange
            var guidRequest = typeof(Guid);
            var sut = new GuidGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(guidRequest, dummyContext);
            // Assert
            Assert.NotEqual(default(Guid), result);
        }

        [Fact]
        public void CreateWithGuidRequestTwiceWillReturnDifferentResults()
        {
            // Arrange
            var sut = new GuidGenerator();

            var guidRequest = typeof(Guid);
            var dummyContext = new DelegatingSpecimenContext();
            var unexpectedResult = sut.Create(guidRequest, dummyContext);
            // Act
            var result = sut.Create(guidRequest, dummyContext);
            // Assert
            Assert.NotEqual(unexpectedResult, result);
        }
    }
}
