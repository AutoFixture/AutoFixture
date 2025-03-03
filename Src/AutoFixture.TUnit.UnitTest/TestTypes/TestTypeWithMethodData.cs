using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class TestTypeWithMethodData
    {
        public IEnumerable<object[]> NonStaticSource()
        {
            yield return [new object()];
            yield return [new object()];
            yield return [new object()];
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
            yield return [];
            yield return [];
            yield return [];
        }

        public async Task SingleStringValueTest(string value)
        {
            await Assert.That(value).IsNotNull();
            await Assert.That(value).IsNotEmpty();
            await Assert.That(string.IsNullOrWhiteSpace(value)).IsFalse();
        }

        public static MethodInfo GetSingleStringValueTestMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(SingleStringValueTest));
        }

        public static IEnumerable<object[]> GetSingleStringValueTestData()
        {
            yield return ["value-one"];
            yield return ["value-two"];
            yield return ["value-three"];
        }

        public static IEnumerable<object[]> GetStringTestsFromArgument(string argument)
        {
            yield return [argument + "-one"];
            yield return [argument + "-two"];
            yield return [argument + "-three"];
        }

        public static MethodInfo GetStringTestsFromArgumentMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(GetStringTestsFromArgument));
        }

        public async Task MultipleValueTest(string a, int b, decimal c)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(a).IsNotEmpty();
            await Assert.That(string.IsNullOrWhiteSpace(a)).IsFalse();

            await Assert.That(b != 0, "Value should not be default").IsTrue();
            await Assert.That(c != 0, "Value should not be default").IsTrue();
        }

        public static MethodInfo GetMultipleValueTestMethodInfo()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(MultipleValueTest));
        }

        public static IEnumerable<object[]> GetMultipleValueTestData()
        {
            yield return ["value-one", 12, 23.3m];
            yield return ["value-two", 38, 12.7m];
            yield return ["value-three", 94, 52.21m];
        }

        public async Task TestWithFrozenParameter(string a, [Frozen] string b, string c)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(b).IsNotNull();
            await Assert.That(c).IsNotNull();

            await Assert.That(b).IsNotEqualTo(a);
            await Assert.That(c).IsEqualTo(b);
        }

        public static IEnumerable<object[]> GetDataForTestWithFrozenParameter()
        {
            yield return ["value-one", "value-two"];
            yield return ["value-two", "value-three"];
            yield return ["value-three", "value-one"];
        }

        public static MethodInfo GetTestWithFrozenParameter()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(TestWithFrozenParameter));
        }

        public async Task TestWithComplexTypes([Frozen] PropertyHolder<string> a, PropertyHolder<string> b)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(b).IsNotNull();

            Assert.Same(a, b);
        }

        public static IEnumerable<object[]> GetTestWithComplexTypesData()
        {
            yield return
            [
                new PropertyHolder<string> { Property = "1647400C-9011-4158-BA5A-F841185AF6EF" },
                new PropertyHolder<string>()
            ];
            yield return
            [
                new PropertyHolder<string> { Property = "E0F5F4F1-4B6B-4B6B-8F4A-7C0F6F4F4F4F" },
                new PropertyHolder<string> { Property = "00000000-0000-0000-0000-000000000000" }
            ];
            yield return
            [
                new PropertyHolder<string> { Property = "B0B0B0B0-B0B0-B0B0-B0B0-B0B0B0B0B0B0" },
                new PropertyHolder<string> { Property = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF" }
            ];
        }

        public static MethodInfo GetTestWithComplexTypes()
        {
            return typeof(TestTypeWithMethodData)
                .GetMethod(nameof(TestWithComplexTypes));
        }

        public static IEnumerable<object[]> GetStringValuesTestData()
        {
            yield return ["test-one", "test-uno"];
            yield return ["test-two", "test-dos"];
            yield return ["test-three", "test-tres"];
        }

        public static IEnumerable<object[]> GetEmptyTestData() => [];
    }
}