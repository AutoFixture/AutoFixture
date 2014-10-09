using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TemplateMethodQueryTests
    {
        [Fact]
        public void SutIsIMethodQuery()
        {
            Action dummy = delegate { };
            var sut = new TemplateMethodQuery(dummy.Method);
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void InitializeWithNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new TemplateMethodQuery(null));
        }

        [Fact]
        public void TemplateIsCorrect()
        {
            Action dummy = delegate { };
            var sut = new TemplateMethodQuery(dummy.Method);
            Assert.Equal(dummy.Method, sut.Template);
        }

        [Fact]
        public void SelectMethodsWithNullThrows()
        {
            Action dummy = delegate { };
            var sut = new TemplateMethodQuery(dummy.Method);

            Assert.Throws<ArgumentNullException>(() => sut.SelectMethods(null));
        }

        [Theory]
        [InlineData(typeof(TypeWithSignatureMethods), "VoidMethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { null })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithStringArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" }, new[] { "a", "b" }, new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithGenerics", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithGenerics", typeof(TypeWithMethods), new object[] { 1 }, new object[] { 1 })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithEnumerable", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" }, new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1, "s" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1, "a" }, new object[] { new object[] { 1, "a" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1 } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1, new[] { "a", "b" } }, new object[] { new object[] { 1, "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithSameName", typeof(TypeWithMethods), new object[] { "s" }, new object[] { "s", "s" })]
        public void SelectMethodsReturnsCorrectResult(Type signatureType, string methodName, Type targetType, object[] arguments, object[] expected)
        {
            var signatureMethod = signatureType.GetMethod(methodName);
            var sut = new TemplateMethodQuery(signatureMethod);

            var results = sut.SelectMethods(targetType)
                .Select(m => m.Invoke(arguments))
                .ToArray();

            Assert.Equal(expected, results);
        }

        [Fact]
        public void SelectMethodsDoesNotReturnIncorrectResult()
        {
            var signatureMethod = typeof(TypeWithSignatureMethods).GetMethod("MethodWithFunc");
            var sut = new TemplateMethodQuery(signatureMethod);

            var results = sut.SelectMethods(typeof(TypeWithMethods))
                .Select(m => m.Invoke(new[] { new Func<string>(() => string.Empty) }));

            var result = Assert.Single(results);
            Assert.Equal(true, result);
        }

        [Theory]
        [InlineData(typeof(TypeWithSignatureMethods), typeof(TypeWithMethods), "MethodInOrder", new object[] { new[] { "a" } })]
        [InlineData(typeof(TypeWithSignatureMethods), typeof(TypeWithMethods), "MethodInOrderWithTwoParameters", new object[] { new[] { "a" }, new[] { "a" } })]
        public void SelectMethodsReturnsInCorrectOrder(Type signatureType, Type targetType, string methodName, object[] arguments)
        {
            var signatureMethod = signatureType.GetMethod(methodName);
            var sut = new TemplateMethodQuery(signatureMethod);

            var results = sut.SelectMethods(targetType)
                .Select(m => m.Invoke(arguments))
                .ToArray();

            var expected = Enumerable.Range(0, results.Count()).Cast<object>().ToArray();
            Assert.Equal(expected, results);
        }
    }

    public class TypeWithSignatureMethods
    {
        public static void VoidMethodWithString(string arg)
        {
        }

        public static void MethodWithString(string arg)
        {
        }
        public static void MethodWithStringArray(string[] arg)
        {
        }
        public static void MethodWithGenerics<T>(T arg)
        {
        }
        public static void MethodWithEnumerable<T>(IEnumerable<T> arg)
        {
        }
        public static void MethodWithArray<T>(T[] arg)
        {
        }
        public static void MethodWithOptionalParameter<T>(T arg)
        {
        }
        public static void MethodWithParamsParameter<T>(T arg)
        {
        }
        public static void MethodWithSameName(string arg)
        {
        }

        public static void MethodInOrder(string[] arg)
        {
        }
        public static void MethodInOrderWithTwoParameters(string[] arg1, IEnumerable<string> arg2)
        {
        }
        public static void MethodWithFunc<T>(Func<T> arg)
        {
        }
    }

    public class TypeWithMethods
    {
        public static void VoidMethodWithString(string arg)
        {
        }

        public static string MethodWithString(string arg)
        {
            return arg;
        }
        public static int MethodWithString(int arg)
        {
            return arg;
        }
        public static int[] MethodWithStringArray(int[] arg)
        {
            return arg;
        }
        public static object[] MethodWithStringArray(object[] arg)
        {
            return arg;
        }
        public static string[] MethodWithStringArray(string[] arg)
        {
            return arg;
        }
        public static IEnumerable<int> MethodWithStringArray(IEnumerable<int> arg)
        {
            return arg;
        }
        public static IEnumerable<string> MethodWithStringArray(IEnumerable<string> arg)
        {
            return arg;
        }
        public static T MethodWithGenerics<T>(T arg)
        {
            return arg;
        }
        public static IEnumerable<T> MethodWithEnumerable<T>(IEnumerable<T> arg)
        {
            return arg;
        }
        public static IEnumerable<T> MethodWithArray<T>(T[] arg)
        {
            return arg;
        }
        public static IEnumerable<T> MethodWithArray<T>(IEnumerable<T> arg)
        {
            return arg;
        }
        public static object[] MethodWithOptionalParameter<T>(T arg, string s = "s")
        {
            return new object[] { arg, s };
        }
        public static object[] MethodWithParamsParameter<T>(T arg, params string[] s)
        {
            return new object[] { arg }.Concat(s).ToArray();
        }
        public static object MethodWithSameName(object arg)
        {
            return arg;
        }
        public static string MethodWithSameName(string arg)
        {
            return arg;
        }
        public static int MethodInOrder(object arg)
        {
            return 6;
        }
        public static int MethodInOrder(string[] arg1, object arg2 = null)
        {
            return 1;
        }
        public static int MethodInOrder(string[] arg)
        {
            return 0;
        }
        public static int MethodInOrder(IEnumerable<object> arg)
        {
            return 5;
        }
        public static int MethodInOrder(IEnumerable<string> arg)
        {
            return 3;
        }
        public static int MethodInOrder(object[] arg)
        {
            return 4;
        }
        public static int MethodInOrder(string[] arg1, object arg2 = null, params object[] arg3)
        {
            return 2;
        }
        public static int MethodInOrderWithTwoParameters(IEnumerable<string> arg1, object arg2)
        {
            return 5;
        }
        public static int MethodInOrderWithTwoParameters(string[] arg1, IEnumerable arg2)
        {
            return 3;
        }
        public static int MethodInOrderWithTwoParameters(IEnumerable<string> arg1, IEnumerable arg2)
        {
            return 4;
        }
        public static int MethodInOrderWithTwoParameters(string[] arg1, IEnumerable<string> arg2, object arg3 = null)
        {
            return 1;
        }
        public static int MethodInOrderWithTwoParameters(IEnumerable<string> arg1, IEnumerable<string> arg2)
        {
            return 2;
        }
        public static int MethodInOrderWithTwoParameters(string[] arg1, IEnumerable<string> arg2)
        {
            return 0;
        }
        public static int MethodWithFunc<T>(Func<T, T> arg)
        {
            return -1;
        }
        public static bool MethodWithFunc<T>(Func<T> arg)
        {
            return true;
        }
    }
}