using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest
{
    public class MemberAutoDataAttributeScenarioTests
    {
        [Theory]
        [MemberAutoData(nameof(GetSingleStringValueTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetEmptyTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetSingleStringValueTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetStringTestsFromArgument),
            parameters: "argument")]
        public void SingleStringValueTest(string value)
        {
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            Assert.False(string.IsNullOrWhiteSpace(value));
        }

        [Theory]
        [MemberAutoData(nameof(GetMultipleValueTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetMultipleValueTestData))]
        public void MultipleValueTest(string a, int b, decimal c)
        {
            Assert.NotNull(a);
            Assert.NotEmpty(a);
            Assert.False(string.IsNullOrWhiteSpace(a));

            Assert.True(b != default, "Value should not be default");
            Assert.True(c != default, "Value should not be default");
        }

        [Theory]
        [MemberAutoData(nameof(GetSingleStringValueTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetSingleStringValueTestData))]
        public void FreezesUninjectedValues(
            string a, [Frozen] string b, string c,
            PropertyHolder<string> d)
        {
            // Assert "a" ends with any possible ending from the test data
            var aSuffix = a.Split('-').Last();
            Assert.Contains(new[] { "one", "two", "three" }, x => aSuffix == x);

            Assert.NotNull(b);
            Assert.NotEmpty(b);
            Assert.False(string.IsNullOrWhiteSpace(b));

            Assert.Equal(b, c);

            Assert.NotNull(d);
            Assert.Equal(b, d.Property);
        }

        [Theory]
        [MemberAutoData(nameof(GetMultipleValueTestData))]
        public void InjectsValues([Frozen] string a,
            [Frozen] int b,
            [Frozen] decimal c,
            PropertyHolder<string> a1,
            PropertyHolder<int> b1,
            PropertyHolder<decimal> c1)
        {
            // Assert "a" ends with any possible ending from the test data
            var aSuffix = a.Split('-').Last();
            Assert.Contains(new[] { "one", "two", "three" }, x => aSuffix == x);

            Assert.Contains(new[] { 22, 75, 19 }, x => x == b);
            Assert.Contains(new[] { 25.7m, 228.1m, 137.09m }, x => x == c);

            Assert.NotNull(a1);
            Assert.Equal(a, a1.Property);

            Assert.NotNull(b1);
            Assert.Equal(b, b1.Property);

            Assert.NotNull(c1);
            Assert.Equal(c, c1.Property);
        }

        [Theory]
        [MemberAutoData(nameof(GetStringValuesTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetStringValuesTestData))]
        public void DoesNotAlterTestDataValuesWhenFrozen(
            [Frozen] string a, string b, string c)
        {
            var aSuffix = a.Split('-').Last();
            var bSuffix = b.Split('-').Last();
            Assert.Contains(new[] { "one", "two", "three" }, x => aSuffix == x);
            Assert.Contains(new[] { "uno", "dos", "tres" }, x => bSuffix == x);

            Assert.NotEqual(a, b);
            Assert.Equal(a, c);
        }

        [Theory]
        [MemberAutoData(nameof(GetStringValuesTestData))]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetStringValuesTestData))]
        public void LastInjectedValueIsFrozen(
            [Frozen] string a, [Frozen] string b, string c)
        {
            var aSuffix = a.Split('-').Last();
            var bSuffix = b.Split('-').Last();
            Assert.Contains(new[] { "one", "two", "three" }, x => aSuffix == x);
            Assert.Contains(new[] { "uno", "dos", "tres" }, x => bSuffix == x);

            Assert.NotEqual(a, c);
            Assert.Equal(b, c);
        }

        [Theory]
        [MemberAutoData(
            memberType: typeof(TestTypeWithMethodData),
            memberName: nameof(TestTypeWithMethodData.GetTestWithComplexTypesData))]
        public void InjectsComplexTypes(
            [Frozen] PropertyHolder<string> a,
            PropertyHolder<string> b,
            PropertyHolder<string> c)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);

            Assert.NotSame(a, b);
            Assert.Same(a, c);
        }

        public static IEnumerable<object[]> GetSingleStringValueTestData()
        {
            yield return new object[] { "test-one" };
            yield return new object[] { "test-two" };
            yield return new object[] { "test-three" };
        }

        public static IEnumerable<object[]> GetMultipleValueTestData()
        {
            yield return new object[] { "test-one", 22, 25.7m };
            yield return new object[] { "test-two", 75, 228.1m };
            yield return new object[] { "test-three", 19, 137.09m };
        }

        public static IEnumerable<object[]> GetStringValuesTestData()
        {
            yield return new object[] { "test-one", "test-uno" };
            yield return new object[] { "test-two", "test-dos" };
            yield return new object[] { "test-three", "test-tres" };
        }
    }
}