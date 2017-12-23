using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MethodInvokerTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullPickerThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new MethodInvoker(null));
        }

        [Fact]
        public void QueryIsCorrect()
        {
            // Arrange
            var expectedPicker = new DelegatingMethodQuery();
            var sut = new MethodInvoker(expectedPicker);
            // Act
            IMethodQuery result = sut.Query;
            // Assert
            Assert.Equal(expectedPicker, result);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Arrange
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNullContainerWillThrow()
        {
            // Arrange
            var sut = new MethodInvoker(new ModestConstructorQuery());
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateFromNonTypeRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonTypeRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Act
            var result = sut.Create(nonTypeRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenQueryReturnsEmptyConstructors()
        {
            // Arrange
            var dummyRequest = typeof(object);
            var query = new DelegatingMethodQuery { OnSelectMethods = t => Enumerable.Empty<IMethod>() };
            var sut = new MethodInvoker(query);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromTypeRequestWhenContainerCannotSatisfyParameterRequestWillReturnCorrectResult()
        {
            // Arrange
            var type = typeof(string);
            var container = new DelegatingSpecimenContext { OnResolve = r => new NoSpecimen() };
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Act
            var result = sut.Create(type, container);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromTypeWithNoPublicConstructorWhenContainerCanSatisfyRequestReturnsCorrectResult()
        {
            // Arrange
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Act
            var result = sut.Create(typeof(AbstractType), container);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateFromTypeWhenParentCanGenerateOneParameterButNotTheOtherWillReturnCorrectNull()
        {
            // Arrange
            var requestedType = typeof(DoubleParameterType<string, int>);
            var parameters = requestedType.GetConstructors().Single().GetParameters();
            var container = new DelegatingSpecimenContext { OnResolve = r => parameters[0] == r ? new object() : new NoSpecimen() };
            var sut = new MethodInvoker(new ModestConstructorQuery());
            // Act
            var result = sut.Create(requestedType, container);
            // Assert
            Assert.IsAssignableFrom<NoSpecimen>(result);
        }

        [Fact]
        public void CreateFromTypeWhenParentCanGenerateBothParametersWillReturnCorrectResult()
        {
            // Arrange
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
            // Act
            var result = sut.Create(requestedType, container);
            // Assert
            var actual = (DoubleParameterType<int, decimal>)result;
            Assert.Equal(expectedParameterValues[0], actual.Parameter1);
            Assert.Equal(expectedParameterValues[1], actual.Parameter2);
        }

        [Fact]
        public void CreateFromTypeWillInvokeContainerCorrectly()
        {
            // Arrange
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
            // Act
            sut.Create(requestedType, contextMock);
            // Assert
            Assert.True(mockVerified, "Mock verification");
        }

        [Fact]
        public void CreateFromTypeWillUseFirstConstructorItCanSatisfy()
        {
            // Arrange
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
            // Act
            var result = sut.Create(requestedType, context);
            // Assert
            var actual = Assert.IsAssignableFrom<MultiUnorderedConstructorType>(result);
            Assert.Equal(expectedText, actual.Text);
            Assert.Equal(expectedNumber, actual.Number);
        }
    }
}
