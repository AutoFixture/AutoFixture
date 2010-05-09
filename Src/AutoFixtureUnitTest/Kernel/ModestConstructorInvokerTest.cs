using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using System.Reflection;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ModestConstructorInvokerTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ModestConstructorInvoker();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var sut = new ModestConstructorInvoker();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateFromNonTypeRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonTypeRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(nonTypeRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonTypeRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeRequestWhenContainerCannotSatisfyParameterRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var type = typeof(string);
            var container = new DelegatingSpecimenContainer { OnResolve = r => new NoSpecimen(type) };
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(type, container);
            // Verify outcome
            var expectedResult = new NoSpecimen(type);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWithNoPublicConstructorWhenContainerCanSatisfyRequestWillReturnNull()
        {
            // Fixture setup
            var container = new DelegatingSpecimenContainer { OnResolve = r => new object() };
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(typeof(AbstractType), container);
            // Verify outcome
            var expectedResult = new NoSpecimen(typeof(AbstractType));
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWhenParentCanGenerateOneParameterButNotTheOtherWillReturnCorrectNull()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<string, int>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();
            var container = new DelegatingSpecimenContainer { OnResolve = r => parameters[0] == r ? new object() : new NoSpecimen(r) };
            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(requestedType, container);
            // Verify outcome
            Assert.IsAssignableFrom<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWhenParentCanGenerateBothParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedParameterValues = new object[] { 1, 2m };
            var parameterQueue = new Queue<object>(expectedParameterValues);

            var requestedType = typeof(DoubleParameterType<int, decimal>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();

            var container = new DelegatingSpecimenContainer();
            container.OnResolve = r =>
            {
                if (parameters.Any(r.Equals))
                {
                    return parameterQueue.Dequeue();
                }
                return null;
            };

            var sut = new ModestConstructorInvoker();
            // Exercise system
            var result = sut.Create(requestedType, container);
            // Verify outcome
            var actual = (DoubleParameterType<int, decimal>)result;
            Assert.Equal(expectedParameterValues[0], actual.Parameter1);
            Assert.Equal(expectedParameterValues[1], actual.Parameter2);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWillInvokeContainerCorrectly()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<long, short>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnResolve = r =>
                {
                    if (parameters.Any(r.Equals))
                    {
                        mockVerified = true;
                        var pType = ((ParameterInfo)r).ParameterType;
                        if (typeof(long) == pType)
                        {
                            return new long();
                        }
                        if (typeof(short) == pType)
                        {
                            return new short();
                        }
                    }
                    throw new ArgumentException("Unexpected container request.", "r");
                };

            var sut = new ModestConstructorInvoker();
            // Exercise system
            sut.Create(requestedType, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
