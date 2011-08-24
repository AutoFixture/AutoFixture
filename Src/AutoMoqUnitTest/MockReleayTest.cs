using System;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockReleayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MockRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MockRelay(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Fixture setup
            Func<Type, bool> expectedSpec = t => true;
            var sut = new MockRelay(expectedSpec);
            // Exercise system
            Func<Type, bool> result = sut.MockableSpecification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new MockRelay();
            // Exercise system
            var result = sut.MockableSpecification;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new MockRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateWithNonAbstractionRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new MockRelay();
            var dummyContext = new Mock<ISpecimenContext>().Object;
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var mock = (Mock)Activator.CreateInstance(mockType);
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(mock);

            var sut = new MockRelay();
            // Exercise system
            var result = sut.Create(request, contextStub.Object);
            // Verify outcome
            Assert.Equal(mock.Object, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonMock()
        {
            // Fixture setup
            var request = typeof(IInterface);
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(new object());

            var sut = new MockRelay();
            // Exercise system
            var result = sut.Create(request, contextStub.Object);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateCorrectlyInvokesSpecification(Type request)
        {
            // Fixture setup
            var verified = false;
            Func<Type, bool> mockSpec = t => verified = t == request;
            var sut = new MockRelay(mockSpec);
            // Exercise system
            var contextDummy = new Mock<ISpecimenContext>();
            sut.Create(request, contextDummy.Object);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
