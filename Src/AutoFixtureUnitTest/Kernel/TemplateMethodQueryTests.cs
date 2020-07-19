using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TemplateMethodQueryTests
    {
        [Fact]
        public void SutIsIMethodQuery()
        {
            Action dummy = () => { };
            var sut = new TemplateMethodQuery(dummy.GetMethodInfo());
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void InitializeWithNullTemplateThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new TemplateMethodQuery(null));
        }

        [Fact]
        public void InitializeSecondConstructorWithNullTemplateThrows()
        {
            var anonymousOwner = new object();
            Assert.Throws<ArgumentNullException>(() => new TemplateMethodQuery(null, anonymousOwner));
        }

        [Fact]
        public void InitializeWithNullOwnerThrows()
        {
            Action dummy = () => { };
            Assert.Throws<ArgumentNullException>(() => new TemplateMethodQuery(dummy.GetMethodInfo(), null));
        }

        [Fact]
        public void TemplateIsCorrect()
        {
            Action dummy = () => { };
            var sut = new TemplateMethodQuery(dummy.GetMethodInfo());
            Assert.Equal(dummy.GetMethodInfo(), sut.Template);
        }

        [Fact]
        public void OwnerIsCorrect()
        {
            Action dummy = () => { };
            var owner = new object();

            var sut = new TemplateMethodQuery(dummy.GetMethodInfo(), owner);

            Assert.Equal(owner, sut.Owner);
        }

        [Fact]
        public void SelectMethodsWithNullThrows()
        {
            Action dummy = () => { };
            var sut = new TemplateMethodQuery(dummy.GetMethodInfo());

            Assert.Throws<ArgumentNullException>(() => sut.SelectMethods(null));
        }

        [Theory]
        [InlineData(typeof(TypeWithTemplateMethods), "VoidMethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { null })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithStringArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" }, new[] { "a", "b" }, new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithGenerics", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithGenerics", typeof(TypeWithMethods), new object[] { 1 }, new object[] { 1 })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithEnumerable", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" }, new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1, "s" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1, "a" }, new object[] { new object[] { 1, "a" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1 } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1, new[] { "a", "b" } }, new object[] { new object[] { 1, "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithSameName", typeof(TypeWithMethods), new object[] { "s" }, new object[] { "s", "s" })]
        public void SelectMethodsReturnsCorrectResult(Type templateType, string methodName, Type targetType, object[] arguments, object[] expected)
        {
            var templateMethod = templateType.GetMethod(methodName);
            var owner = Activator.CreateInstance(targetType);
            var sut = new TemplateMethodQuery(templateMethod, owner);

            var results = sut.SelectMethods(targetType)
                .Select(m => m.Invoke(arguments))
                .ToArray();

            Assert.Equal(expected, results);
        }

        [Theory]
        [InlineData(typeof(TypeWithTemplateMethods), "VoidMethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { null })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithStringArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithGenerics", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithGenerics", typeof(TypeWithMethods), new object[] { 1 }, new object[] { 1 })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithEnumerable", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1, "s" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1, "a" }, new object[] { new object[] { 1, "a" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1 } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1, new[] { "a", "b" } }, new object[] { new object[] { 1, "a", "b" } })]
        [InlineData(typeof(TypeWithTemplateMethods), "MethodWithSameName", typeof(TypeWithMethods), new object[] { "s" }, new object[] { "s" })]
        public void SelectMethodsReturnsCorrectResultWhenInitializedWithNullOwner(Type templateType, string methodName, Type targetType, object[] arguments, object[] expected)
        {
            var templateMethod = templateType.GetMethod(methodName);
            var sut = new TemplateMethodQuery(templateMethod);

            var results = sut.SelectMethods(targetType)
                .Select(m => m.Invoke(arguments))
                .ToArray();

            Assert.Equal(expected, results);
        }

        [Fact]
        public void SelectMethodsDoesNotReturnIncorrectResult()
        {
            var templateMethod = typeof(TypeWithTemplateMethods).GetMethod("MethodWithFunc");
            var sut = new TemplateMethodQuery(templateMethod);

            var results = sut.SelectMethods(typeof(TypeWithMethods))
                .Select(m => m.Invoke(new[] { new Func<string>(() => string.Empty) }));

            var result = Assert.Single(results);
            Assert.True((bool)result);
        }

        [Theory]
        [InlineData(typeof(TypeWithTemplateMethods), typeof(TypeWithMethods), "MethodInOrder", new object[] { new[] { "a" } })]
        [InlineData(typeof(TypeWithTemplateMethods), typeof(TypeWithMethods), "MethodInOrderWithTwoParameters", new object[] { new[] { "a" }, new[] { "a" } })]
        public void SelectMethodsReturnsInCorrectOrderWhenInitializedWithNullOwner(Type templateType, Type targetType, string methodName, object[] arguments)
        {
            var templateMethod = templateType.GetMethod(methodName);
            var owner = Activator.CreateInstance(targetType);
            var sut = new TemplateMethodQuery(templateMethod, owner);

            var results = sut.SelectMethods(targetType)
                .Select(m => m.Invoke(arguments))
                .ToArray();

            var expected = Enumerable.Range(0, results.Length).Cast<object>().ToArray();
            Assert.Equal(expected, results);
        }
    }

    public class TypeWithTemplateMethods
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
        public string[] MethodWithStringArray(string[] arg)
        {
            return arg;
        }
        public static IEnumerable<int> MethodWithStringArray(IEnumerable<int> arg)
        {
            return arg;
        }
        public IEnumerable<string> MethodWithStringArray(IEnumerable<string> arg)
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
        public IEnumerable<T> MethodWithArray<T>(IEnumerable<T> arg)
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
        public string MethodWithSameName(string arg)
        {
            return arg;
        }
        public static int MethodInOrder(object arg)
        {
            return 6;
        }
        public int MethodInOrder(string[] arg1, object arg2 = null)
        {
            return 1;
        }
        public static int MethodInOrder(string[] arg)
        {
            return 0;
        }
        public int MethodInOrder(IEnumerable<object> arg)
        {
            return 5;
        }
        public static int MethodInOrder(IEnumerable<string> arg)
        {
            return 3;
        }
        public int MethodInOrder(object[] arg)
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
        public int MethodInOrderWithTwoParameters(string[] arg1, IEnumerable arg2)
        {
            return 3;
        }
        public static int MethodInOrderWithTwoParameters(IEnumerable<string> arg1, IEnumerable arg2)
        {
            return 4;
        }
        public int MethodInOrderWithTwoParameters(string[] arg1, IEnumerable<string> arg2, object arg3 = null)
        {
            return 1;
        }
        public static int MethodInOrderWithTwoParameters(IEnumerable<string> arg1, IEnumerable<string> arg2)
        {
            return 2;
        }
        public int MethodInOrderWithTwoParameters(string[] arg1, IEnumerable<string> arg2)
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