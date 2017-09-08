using System;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockAroundAdviceTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = MockRepository.GenerateMock<ISpecimenBuilder>();
            // Exercise system
            var sut = new RhinoMockAroundAdvice(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RhinoMockAroundAdvice((ISpecimenBuilder)null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = MockRepository.GenerateMock<ISpecimenBuilder>();
            var sut = new RhinoMockAroundAdvice(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateWithNonMockRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var dummyBuilder = MockRepository.GenerateMock<ISpecimenBuilder>();
            var sut = new RhinoMockAroundAdvice(dummyBuilder);
            // Exercise system
            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
