using System;
using System.CodeDom;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class LateBindingMethodQueryTests
    {
        [Fact]
        public void SutIsIMethodQuery()
        {
            Action dummy = delegate { };
            var sut = new LateBindingMethodQuery(dummy.Method);
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void InitializeWithNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new LateBindingMethodQuery(null));
        }

        [Theory]
        [InlineData(typeof(TypeWithSignatureMethods), "VoidMethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { null })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithString", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithStringArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" }, new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithGenerics", typeof(TypeWithMethods), new[] { "s" }, new object[] { "s" })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithGenerics", typeof(TypeWithMethods), new object[] { 1 }, new object[] { 1 })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithEnumerable", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithArray", typeof(TypeWithMethods), new object[] { new[] { "a", "b" } }, new object[] { new[] { "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1, "s" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithOptionalParameter", typeof(TypeWithMethods), new object[] { 1, "a" }, new object[] { new object[] { 1, "a" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1 }, new object[] { new object[] { 1 } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithParamsParameter", typeof(TypeWithMethods), new object[] { 1, new[] { "a", "b" } }, new object[] { new object[] { 1, "a", "b" } })]
        [InlineData(typeof(TypeWithSignatureMethods), "MethodWithSameName", typeof(TypeWithMethods), new object[] { "s" }, new object[] { "s", "s" })]
        public void SelectMethodsReturnsCorrectResult(Type signatureType, string methodName, Type targetType, object[] arguments, object[] expected)
        {
            var signatureMethod = signatureType.GetMethod(methodName);
            var sut = new LateBindingMethodQuery(signatureMethod);

            var results = sut.SelectMethods(targetType)
                .Select(m => m.Invoke(arguments))
                .ToArray();

            Assert.Equal(expected, results);
        }

    }
}
