using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class PropertyRequestRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new PropertyRequestRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new PropertyRequestRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new PropertyRequestRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateFromNonPropertyRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonParameterRequest = new object();
            var sut = new PropertyRequestRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonParameterRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromPropertyRequestWillReturnCorrectResultWhenContainerCannotSatisfyRequest()
        {
            // Fixture setup
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new PropertyRequestRelay();
            // Exercise system
            var result = sut.Create(propertyInfo, container);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromPropertyRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedSpecimen };
            var sut = new PropertyRequestRelay();
            // Exercise system
            var result = sut.Create(propertyInfo, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateFromParameterRequestWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new PropertyRequestRelay();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var expectedRequest = new SeededRequest(propertyInfo.PropertyType, propertyInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(expectedRequest, r);
                mockVerified = true;
                return null;
            };
            // Exercise system
            sut.Create(propertyInfo, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
