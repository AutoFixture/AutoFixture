using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SpecimenCreatedEventArgsTest
    {
        [Fact]
        public void SutIsSpecimenTraceEventArgs()
        {
            // Fixture setup
            var dummyRequest = new object();
            var dummySpecimen = new object();
            var dummyDepth = 0;
            // Exercise system
            var sut = new SpecimenCreatedEventArgs(dummyRequest, dummySpecimen, dummyDepth);
            // Verify outcome
            Assert.IsAssignableFrom<RequestTraceEventArgs>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorExplicitlyAllowsTracingNullRequestsIfThatShouldEverBeWarranted()
        {
            // Fixture setup
            var dummyDepth = 0;
            var dummySpecimen = new object();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => new SpecimenCreatedEventArgs(null, dummySpecimen, dummyDepth)));
            // Teardown
        }

        [Fact]
        public void ConstructorExplicitlyAllowsNullSpecimen()
        {
            // Fixture setup
            var dummyRequest = new object();
            var dummyDepth = 0;
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => new SpecimenCreatedEventArgs(dummyRequest, null, dummyDepth)));
            // Teardown
        }

        [Fact]
        public void RequestMatchesConstructorArgument()
        {
            // Fixture setup
            var expectedRequest = new object();
            var dummySpecimen = new object();
            var dummyDepth = 0;
            var sut = new SpecimenCreatedEventArgs(expectedRequest, dummySpecimen, dummyDepth);
            // Exercise system
            var result = sut.Request;
            // Verify outcome
            Assert.Equal(expectedRequest, result);
            // Teardown
        }

        [Fact]
        public void DepthMatchesConstructorArgument()
        {
            // Fixture setup
            var dummyRequest = new object();
            var dummySpecimen = new object();
            var expectedDepth = 1;
            var sut = new SpecimenCreatedEventArgs(dummyRequest, dummySpecimen, expectedDepth);
            // Exercise system
            int result = sut.Depth;
            // Verify outcome
            Assert.Equal(expectedDepth, result);
            // Teardown
        }

        [Fact]
        public void SpecimenMatchesConstructorArgument()
        {
            // Fixture setup
            var dummyRequest = new object();
            var expectedSpecimen = new object();
            var dummyDepth = 0;
            var sut = new SpecimenCreatedEventArgs(dummyRequest, expectedSpecimen, dummyDepth);
            // Exercise system
            object result = sut.Specimen;
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }
    }
}
