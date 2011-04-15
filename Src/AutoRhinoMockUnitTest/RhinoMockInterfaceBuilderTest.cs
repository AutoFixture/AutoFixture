using System;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using Rhino.Mocks;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockInterfaceBuilderTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RhinoMockInterfaceBuilder();
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
                new RhinoMockInterfaceBuilder(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Fixture setup
            Func<Type, bool> expectedSpec = t => true;
            var sut = new RhinoMockInterfaceBuilder(expectedSpec);
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
            var sut = new RhinoMockInterfaceBuilder();
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
            var sut = new RhinoMockInterfaceBuilder();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonTypeIsCorrect()
        {
            // Fixture setup
            var sut = new RhinoMockInterfaceBuilder(t => true);

            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            // Exercise system
            Assembly request = typeof(RhinoMockInterfaceBuilder).Assembly;
            var result = sut.Create(request, dummyContext);

            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTimeOffset))]
        public void CreateWithNonAbstractRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new RhinoMockInterfaceBuilder();
            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTimeOffset))]
        public void CreateWithNonAbstractRequestReturnsNonMockedResult(object request)
        {
            // Fixture setup
            var sut = new RhinoMockInterfaceBuilder();
            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void CreateGenericTypeWithNonDefaultConstructorIsCorrect()
        {
            // Fixture setup
            var sut = new RhinoMockInterfaceBuilder();
            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            // Exercise system
            var result = sut.Create(typeof(GenericType<ConcreteType>), dummyContext);

            // Verify outcome
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
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
            var sut = new RhinoMockInterfaceBuilder(mockSpec);
            // Exercise system
            var contextDummy = MockRepository.GenerateMock<ISpecimenContext>();
            sut.Create(request, contextDummy);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }
    }
}
