using System;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new NSubstituteRelay();
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
                new NSubstituteRelay(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Fixture setup
            Func<Type, bool> expectedSpec = t => true;
            var sut = new NSubstituteRelay(expectedSpec);
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
            var sut = new NSubstituteRelay();
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
            var sut = new NSubstituteRelay();
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
            var sut = new NSubstituteRelay();
            var dummyContext = Substitute.For<ISpecimenContext>();
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
            var mock = Substitute.For<IMockable>();
            var contextStub = Substitute.For<ISpecimenContext>();
            contextStub.Resolve(request).Returns(mock);

            var sut = new NSubstituteRelay();
            // Exercise system
            var result = sut.Create(request, contextStub);
            // Verify outcome
            Assert.Equal(mock, result);
            // Teardown
        }
/*
        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonMock()
        {
            // Fixture setup
            var request = typeof(IInterface);
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var contextStub = Substitute.For<ISpecimenContext>();
            contextStub.Resolve(mockType).Returns(new object());

            var sut = new NSubstituteRelay();
            // Exercise system
            var result = sut.Create(request, contextStub);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }
*/
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
            var sut = new NSubstituteRelay(mockSpec);
            // Exercise system
            var contextDummy = Substitute.For<ISpecimenContext>();
            sut.Create(request, contextDummy);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
