using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SpecimenCreatedEventArgsTest
    {
        [Fact]
        public void SutIsSpecimenTraceEventArgs()
        {
            // Arrange
            var dummyRequest = new object();
            var dummySpecimen = new object();
            var dummyDepth = 0;
            // Act
            var sut = new SpecimenCreatedEventArgs(dummyRequest, dummySpecimen, dummyDepth);
            // Assert
            Assert.IsAssignableFrom<RequestTraceEventArgs>(sut);
        }

        [Fact]
        public void ConstructorExplicitlyAllowsTracingNullRequestsIfThatShouldEverBeWarranted()
        {
            // Arrange
            var dummyDepth = 0;
            var dummySpecimen = new object();
            // Act & assert
            Assert.Null(Record.Exception(() => new SpecimenCreatedEventArgs(null, dummySpecimen, dummyDepth)));
        }

        [Fact]
        public void ConstructorExplicitlyAllowsNullSpecimen()
        {
            // Arrange
            var dummyRequest = new object();
            var dummyDepth = 0;
            // Act & assert
            Assert.Null(Record.Exception(() => new SpecimenCreatedEventArgs(dummyRequest, null, dummyDepth)));
        }

        [Fact]
        public void RequestMatchesConstructorArgument()
        {
            // Arrange
            var expectedRequest = new object();
            var dummySpecimen = new object();
            var dummyDepth = 0;
            var sut = new SpecimenCreatedEventArgs(expectedRequest, dummySpecimen, dummyDepth);
            // Act
            var result = sut.Request;
            // Assert
            Assert.Equal(expectedRequest, result);
        }

        [Fact]
        public void DepthMatchesConstructorArgument()
        {
            // Arrange
            var dummyRequest = new object();
            var dummySpecimen = new object();
            var expectedDepth = 1;
            var sut = new SpecimenCreatedEventArgs(dummyRequest, dummySpecimen, expectedDepth);
            // Act
            int result = sut.Depth;
            // Assert
            Assert.Equal(expectedDepth, result);
        }

        [Fact]
        public void SpecimenMatchesConstructorArgument()
        {
            // Arrange
            var dummyRequest = new object();
            var expectedSpecimen = new object();
            var dummyDepth = 0;
            var sut = new SpecimenCreatedEventArgs(dummyRequest, expectedSpecimen, dummyDepth);
            // Act
            object result = sut.Specimen;
            // Assert
            Assert.Equal(expectedSpecimen, result);
        }
    }
}
