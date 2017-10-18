using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MissingParametersSupplyingMethodTests
    {
        [Fact]
        public void SutIsIMethod()
        {
            var method = new DelegatingMethod();
            var sut = new MissingParametersSupplyingMethod(method);
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MissingParametersSupplyingMethod(null));
        }

        [Fact]
        public void MethodIsCorrect()
        {
            var expectedMethod = new DelegatingMethod();
            var sut = new MissingParametersSupplyingMethod(expectedMethod);

            var result = sut.Method;

            Assert.Equal(expectedMethod, result);
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            Action<int, double> dummy = delegate { };
            var expectedParameters = dummy.GetMethodInfo().GetParameters();
            var method = new DelegatingMethod {OnParameters = () => expectedParameters};
            var sut = new MissingParametersSupplyingMethod(method);

            var result = sut.Parameters;

            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Fact]
        public void InvokeWithCorrectArgumentsReturnsCorrectResult()
        {
            var arguments = new[] {new object(), new object()};
            var expected = new object();
            Action<object, object> dummy = delegate { };
            var method = new DelegatingMethod();
            method.OnParameters = () => dummy.GetMethodInfo().GetParameters();
            method.OnInvoke = args => arguments.SequenceEqual(args) ? expected : null;
            var sut = new MissingParametersSupplyingMethod(method);

            var result = sut.Invoke(arguments);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgument", new object[] {}, new[] {0, 100})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgument", new object[] {1}, new[] {1, 100})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgument", new object[] {1, 2}, new[] {1, 2})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgumentString", new object[] {}, new[] {null, "100"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgumentString", new object[] {"1"}, new[] {"1", "100"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalArgumentString", new object[] {"1", "2"}, new[] {"1", "2"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgument", new object[] {}, new[] {0})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgument", new object[] {1}, new[] {1})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgument", new object[] {1, new[] {2, 3}}, new[] {1, 2, 3})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgumentString", new object[] {}, new string[] {null})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgumentString", new object[] {"1"}, new[] {"1"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithParamsArgumentString", new object[] {"1", new[] {"2", "3"}}, new[] {"1", "2", "3"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] {}, new[] {0, 200})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] {1}, new[] {1, 200})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] {1, 2}, new[] {1, 2})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArguments", new object[] {1, 2, new[] {3, 4}}, new[] {1, 2, 3, 4})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] {}, new[] {null, "200"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] {"1"}, new[] {"1", "200"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] {"1", "2"}, new[] {"1", "2"})]
        [InlineData(typeof (TypeWithMethodsWithOptionalArguments), "MethodWithOptionalAndParamsArgumentsString", new object[] {"1", "2", new[] {"3", "4"}}, new[] {"1", "2", "3", "4"})]
        public void InvokeReturnsCorrectResult(Type targetType, string methodName, object[] arguments, object expected)
        {
            var method = new DelegatingMethod();
            var methodInfo = targetType.GetMethod(methodName);
            method.OnParameters = methodInfo.GetParameters;
            method.OnInvoke = args => methodInfo.Invoke(null, args.ToArray());
            var sut = new MissingParametersSupplyingMethod(method);

            var result = sut.Invoke(arguments);

            Assert.Equal(expected, result);
        }

        public static class TypeWithMethodsWithOptionalArguments
        {
            public static string[] MethodWithOptionalArgumentString(string argument, string optional = "100")
            {
                return new[] { argument, optional };
            }

            public static int[] MethodWithOptionalArgument(int argument, int optional = 100)
            {
                return new[] { argument, optional };
            }

            public static string[] MethodWithParamsArgumentString(string argument, params string[] arguments)
            {
                return new[] { argument }.Concat(arguments).ToArray();
            }

            public static int[] MethodWithParamsArgument(int argument, params int[] arguments)
            {
                return new[] { argument }.Concat(arguments).ToArray();
            }

            public static int[] MethodWithOptionalAndParamsArguments(int argument, int optional = 200, params int[] arguments)
            {
                return new[] { argument, optional }.Concat(arguments).ToArray();
            }

            public static string[] MethodWithOptionalAndParamsArgumentsString(string argument, string optional = "200", params string[] arguments)
            {
                return new[] { argument, optional }.Concat(arguments).ToArray();
            }
        }
    }
}