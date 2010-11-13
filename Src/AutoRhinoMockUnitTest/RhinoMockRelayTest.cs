using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.AutoRhinoMock;
using Xunit.Extensions;
using Rhino.Mocks;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoRhinoMockRelayTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RhinoMockRelay();
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
                new RhinoMockRelay(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Fixture setup
            Func<Type, bool> expectedSpec = t => true;
            var sut = new RhinoMockRelay(expectedSpec);
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
            var sut = new RhinoMockRelay();
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
            var sut = new RhinoMockRelay();
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
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTimeOffset))]
        public void CreateWithNonAbstractRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new RhinoMockRelay();
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
            var sut = new RhinoMockRelay();
            var dummyContext = MockRepository.GenerateMock<ISpecimenContext>();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
            // Teardown
        }
    }
}
