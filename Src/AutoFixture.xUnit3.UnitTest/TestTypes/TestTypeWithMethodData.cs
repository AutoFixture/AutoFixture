using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    public class TestTypeWithMethodData
    {
        public IEnumerable<object[]> NonStaticSource()
        {
            yield return new[] { new object() };
            yield return new[] { new object() };
            yield return new[] { new object() };
        }

        public static MethodInfo GetNonStaticSourceMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(NonStaticSource));
        }

        public static object NonEnumerableMethod()
        {
            return new object();
        }

        public static MethodInfo GetNonEnumerableMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(NonEnumerableMethod));
        }

        public static IEnumerable<object[]> TestDataWithNoValues()
        {
            yield return new object[] { };
            yield return new object[] { };
            yield return new object[] { };
        }

        public void SingleStringValueTest(string value)
        {
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.False(string.IsNullOrWhiteSpace(value));
        }

        public static MethodInfo GetSingleStringValueTestMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(SingleStringValueTest));
        }

        public static IEnumerable<object[]> GetSingleStringValueTestData()
        {
            yield return new object[] { "value-one" };
            yield return new object[] { "value-two" };
            yield return new object[] { "value-three" };
        }

        public static IEnumerable<object[]> GetStringTestsFromArgument(string argument)
        {
            yield return new object[] { argument + "-one" };
            yield return new object[] { argument + "-two" };
            yield return new object[] { argument + "-three" };
        }

        public static MethodInfo GetStringTestsFromArgumentMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(GetStringTestsFromArgument));
        }

        public void MultipleValueTest(string a, int b, decimal c)
        {
            Assert.NotNull(a);
            Assert.NotEmpty(a);
            Assert.False(string.IsNullOrWhiteSpace(a));

            Assert.True(b != 0, "Value should not be default");
            Assert.True(c != 0, "Value should not be default");
        }

        public static MethodInfo GetMultipleValueTestMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(MultipleValueTest));
        }

        public static IEnumerable<object[]> GetMultipleValueTestData()
        {
            yield return new object[] { "value-one", 12, 23.3m };
            yield return new object[] { "value-two", 38, 12.7m };
            yield return new object[] { "value-three", 94, 52.21m };
        }

        public void TestWithFrozenParameter(string a, [Frozen] string b, string c)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);

            Assert.NotEqual(a, b);
            Assert.Equal(b, c);
        }

        public static IEnumerable<object[]> GetDataForTestWithFrozenParameter()
        {
            yield return new object[] { "value-one", "value-two" };
            yield return new object[] { "value-two", "value-three" };
            yield return new object[] { "value-three", "value-one" };
        }

        public static MethodInfo GetTestWithFrozenParameter()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(TestWithFrozenParameter));
        }

        public void TestWithComplexTypes([Frozen] PropertyHolder<string> a, PropertyHolder<string> b)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);

            Assert.Same(a, b);
        }

        public static IEnumerable<object[]> GetTestWithComplexTypesData()
        {
            yield return new object[]
            {
                new PropertyHolder<string> { Property = "1647400C-9011-4158-BA5A-F841185AF6EF" },
                new PropertyHolder<string>()
            };
            yield return new object[]
            {
                new PropertyHolder<string> { Property = "E0F5F4F1-4B6B-4B6B-8F4A-7C0F6F4F4F4F" },
                new PropertyHolder<string> { Property = "00000000-0000-0000-0000-000000000000" }
            };
            yield return new object[]
            {
                new PropertyHolder<string> { Property = "B0B0B0B0-B0B0-B0B0-B0B0-B0B0B0B0B0B0" },
                new PropertyHolder<string> { Property = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF" }
            };
        }

        public static MethodInfo GetTestWithComplexTypes()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(TestWithComplexTypes));
        }

        public static IEnumerable<object[]> GetStringValuesTestData()
        {
            yield return new object[] { "test-one", "test-uno" };
            yield return new object[] { "test-two", "test-dos" };
            yield return new object[] { "test-three", "test-tres" };
        }

        public static IEnumerable<object[]> GetEmptyTestData() => Enumerable.Empty<object[]>();
    }
}