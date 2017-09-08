using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ParameterRequestRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ParameterRequestRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new ParameterRequestRelay();
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
            var sut = new ParameterRequestRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateFromNonParameterRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonParameterRequest = new object();
            var sut = new ParameterRequestRelay();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonParameterRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromParameterRequestWillReturnNullWhenContainerCannotSatisfyRequest()
        {
            // Fixture setup
            var parameterInfo = typeof(SingleParameterType<string>).GetTypeInfo().GetConstructors().First().GetParameters().First();
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new ParameterRequestRelay();
            // Exercise system
            var result = sut.Create(parameterInfo, container);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromParameterRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var parameterInfo = typeof(SingleParameterType<string>).GetTypeInfo().GetConstructors().First().GetParameters().First();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedSpecimen };
            var sut = new ParameterRequestRelay();
            // Exercise system
            var result = sut.Create(parameterInfo, container);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateFromParameterRequestWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new ParameterRequestRelay();
            var parameterInfo = typeof(SingleParameterType<string>).GetTypeInfo().GetConstructors().First().GetParameters().First();
            var expectedRequest = new SeededRequest(parameterInfo.ParameterType, parameterInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContext();
            containerMock.OnResolve = r =>
            {
                Assert.Equal(expectedRequest, r);
                mockVerified = true;
                return null;
            };
            // Exercise system
            sut.Create(parameterInfo, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
