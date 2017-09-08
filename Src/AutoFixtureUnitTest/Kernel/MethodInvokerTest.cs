using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MethodInvokerTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullPickerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodInvoker(null));
            // Teardown
        }

        [Fact]
        public void QueryIsCorrect()
        {
            // Fixture setup
            var expectedPicker = new DelegatingMethodQuery();
            var sut = new MethodInvoker(expectedPicker);
            // Exercise system
            IMethodQuery result = sut.Query;
            // Verify outcome
            Assert.Equal(expectedPicker, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new MethodInvoker(new ModestConstructorQuery());
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
            var sut = new MethodInvoker(new ModestConstructorQuery());
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
            var dummyContainer = new DelegatingSpecimenContext();
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Exercise system
            var result = sut.Create(nonTypeRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenQueryReturnsEmptyConstructors()
        {
            // Fixture setup
            var dummyRequest = typeof(object);
            var query = new DelegatingMethodQuery { OnSelectMethods = t => Enumerable.Empty<IMethod>() };
            var sut = new MethodInvoker(query);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeRequestWhenContainerCannotSatisfyParameterRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var type = typeof(string);
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Exercise system
            var result = sut.Create(type, container);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWithNoPublicConstructorWhenContainerCanSatisfyRequestReturnsCorrectResult()
        {
            // Fixture setup
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Exercise system
            var result = sut.Create(typeof(AbstractType), container);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWhenParentCanGenerateOneParameterButNotTheOtherWillReturnCorrectNull()
        {
            // Fixture setup
            var requestedType = typeof(DoubleParameterType<string, int>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();
            var container = new DelegatingSpecimenContext { OnResolve = r => parameters[0] == r ? new object() : new NoSpecimen() };
            var sut = new MethodInvoker(new ModestConstructorQuery());
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

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r =>
            {
                if (parameters.Any(r.Equals))
                {
                    return parameterQueue.Dequeue();
                }
                return null;
            };

            var sut = new MethodInvoker(new ModestConstructorQuery());
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
            var contextMock = new DelegatingSpecimenContext();
            contextMock.OnResolve = r =>
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
                throw new ArgumentException("Unexpected container request.", nameof(r));
            };

            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Exercise system
            sut.Create(requestedType, contextMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }

        [Fact]
        public void CreateFromTypeWillUseFirstConstructorItCanSatisfy()
        {
            // Fixture setup
            var requestedType = typeof(MultiUnorderedConstructorType);
            var ctor1 = requestedType.GetConstructor(new[] { typeof(MultiUnorderedConstructorType.ParameterObject) });
            var ctor2 = requestedType.GetConstructor(new[] { typeof(string), typeof(int) });

            var picker = new DelegatingMethodQuery { OnSelectMethods = t => new IMethod[] { new ConstructorMethod(ctor1), new ConstructorMethod(ctor2) } };
            var sut = new MethodInvoker(picker);

            var ctor2Params = ctor2.GetParameters();
            var expectedText = "Anonymous text";
            var expectedNumber = 14;

            var context = new DelegatingSpecimenContext();
            context.OnResolve = r =>
            {
                if (ctor2Params.Any(r.Equals))
                {
                    var pType = ((ParameterInfo)r).ParameterType;
                    if (typeof(string) == pType)
                    {
                        return expectedText;
                    }
                    if (typeof(int) == pType)
                    {
                        return expectedNumber;
                    }
                }
                return new NoSpecimen();
            };
            // Exercise system
            var result = sut.Create(requestedType, context);
            // Verify outcome
            var actual = Assert.IsAssignableFrom<MultiUnorderedConstructorType>(result);
            Assert.Equal(expectedText, actual.Text);
            Assert.Equal(expectedNumber, actual.Number);
            // Teardown
        }
    }
}
