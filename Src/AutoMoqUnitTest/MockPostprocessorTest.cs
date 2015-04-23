using System;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockPostprocessorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new Mock<ISpecimenBuilder>().Object;
            // Exercise system
            var sut = new MockPostprocessor(dummyBuilder);
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
                new MockPostprocessor(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new Mock<ISpecimenBuilder>().Object;
            var sut = new MockPostprocessor(expectedBuilder);
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
        [InlineData(typeof(Mock))]
        [InlineData(typeof(Mock<>))]
        public void CreateWithNonMockRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var dummyBuilder = new Mock<ISpecimenBuilder>().Object;
            var sut = new MockPostprocessor(dummyBuilder);
            // Exercise system
            var dummyContext = new Mock<ISpecimenContext>().Object;
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateMockRequestReturnsCorrectMock()
        {
            // Fixture setup
            var request = typeof(Mock<object>);
            var context = new Mock<ISpecimenContext>().Object;

            var builderStub = new Mock<ISpecimenBuilder>();
            builderStub.Setup(b => b.Create(request, context)).Returns(new Mock<object>());

            var sut = new MockPostprocessor(builderStub.Object);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var mock = Assert.IsAssignableFrom<Mock<object>>(result);
            Assert.True(mock.CallBase);
            Assert.Equal(DefaultValue.Mock, mock.DefaultValue);
            // Teardown
        }

        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void CreateFromMockRequestWhenDecoratedBuilderReturnsValidNonMockSpecimenReturnsCorrectResult(
            object validNonMockSpecimen)
        {
            // Fixture setup
            var request = typeof (Mock<object>);
            var context = new Mock<ISpecimenContext>().Object;

            var builderStub = new Mock<ISpecimenBuilder>();
            builderStub.Setup(b => b.Create(request, context)).Returns(validNonMockSpecimen);

            var sut = new MockPostprocessor(builderStub.Object);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(validNonMockSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateFromMockRequestWhenDecoratedBuilderReturnsMockOfWrongGenericTypeReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(Mock<IInterface>);
            var context = new Mock<ISpecimenContext>().Object;

            var builderStub = new Mock<ISpecimenBuilder>();
            builderStub.Setup(b => b.Create(request, context)).Returns(new Mock<AbstractType>());

            var sut = new MockPostprocessor(builderStub.Object);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
