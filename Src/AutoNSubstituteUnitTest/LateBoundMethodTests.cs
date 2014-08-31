using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class LateBoundMethodTests
    {
        [Fact]
        public void SutIsIMethod()
        {
            var method = Substitute.For<IMethod>();
            var sut = new LateBoundMethod(method);
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LateBoundMethod(null));
        }

        [Fact]
        public void MethodIsCorrect()
        {
            var expectedMethod = Substitute.For<IMethod>();
            var sut = new LateBoundMethod(expectedMethod);

            var result = sut.Method;

            Assert.Equal(expectedMethod, result);
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            Action<int, double> dummy = delegate { };
            var expectedParameters = dummy.Method.GetParameters();
            var method = Substitute.For<IMethod>();
            method.Parameters.Returns(expectedParameters);
            var sut = new LateBoundMethod(method);

            var result = sut.Parameters;

            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Fact]
        public void InvokeWithCorrectArgumentsReturnsCorrectResult()
        {
            var arguments = new[] { new object(), new object() };
            var expected = new object();
            Action<object, object> dummy = delegate { };
            var method = Substitute.For<IMethod>();
            method.Parameters.Returns(dummy.Method.GetParameters());
            method.Invoke(Arg.Is<IEnumerable<object>>(x => arguments.SequenceEqual(x))).Returns(expected);
            var sut = new LateBoundMethod(method);

            var result = sut.Invoke(arguments);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgument", new object[] { }, new[] { 0, 100 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgument", new object[] { 1 }, new[] { 1, 100 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgument", new object[] { 1, 2 }, new[] { 1, 2 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgumentString", new object[] { }, new[] { null, "100" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgumentString", new object[] { "1" }, new[] { "1", "100" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgumentString", new object[] { "1", "2" }, new[] { "1", "2" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgument", new object[] { }, new[] { 0 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgument", new object[] { 1 }, new[] { 1 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgument", new object[] { 1, new[] { 2, 3 } }, new[] { 1, 2, 3 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgumentString", new object[] { }, new string[] { null })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgumentString", new object[] { "1" }, new[] { "1" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgumentString", new object[] { "1", new[] { "2", "3" } }, new[] { "1", "2", "3" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] { }, new[] { 0, 200 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] { 1 }, new[] { 1, 200 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] { 1, 2 }, new[] { 1, 2 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] { 1, 2, new[] { 3, 4 } }, new[] { 1, 2, 3, 4 })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] { }, new[] { null, "200" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] { "1" }, new[] { "1", "200" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] { "1", "2" }, new[] { "1", "2" })]
        [InlineData(typeof(TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] { "1", "2", new[] { "3", "4" } }, new[] { "1", "2", "3", "4" })]
        public void InvokeReturnsCorrectResult(Type targetType, string methodName, object[] arguments, object expected)          
        {
            var method = Substitute.For<IMethod>();
            var methodInfo = targetType.GetMethod(methodName);
            method.Parameters.Returns(methodInfo.GetParameters());
            method.Invoke(null).ReturnsForAnyArgs(x => methodInfo.Invoke(null, x.Arg<IEnumerable<object>>().ToArray()));
            var sut = new LateBoundMethod(method);

            var result = sut.Invoke(arguments);

            Assert.Equal(expected, result);
        }
    }
}