using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest
{
    public class ClassAutoDataScenarioTests
    {
        [Theory]
        [ClassAutoData(typeof(EmptyClassData))]
        [ClassAutoData(typeof(ClassWithEmptyTestData))]
        [SuppressMessage("Usage", "xUnit1006:Theory methods should have parameters",
            Justification = "This tests a scenario supported by xUnit 2.")]
        public void TestWithNoParametersPasses()
        {
            Assert.True(true);
        }

        [Theory]
        [ClassAutoData(typeof(MixedTypeClassData))]
        public void TestWithMixedTypesPasses(int? a, string b, EnumType? c, Tuple<string, int> d)
        {
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);
            Assert.NotNull(d);
        }

        [Theory]
        [ClassAutoData(typeof(ParameterizedClassData), 42, "test-13", EnumType.Third)]
        public void TestWithParameterizedClassDataReceivesExpectedData(
            int a, string b, EnumType c, PropertyHolder<string> d)
        {
            Assert.Equal(42, a);
            Assert.Equal("test-13", b);
            Assert.Equal(EnumType.Third, c);
            Assert.NotNull(d?.Property);
        }

        [Theory]
        [ClassAutoData(typeof(ParameterizedClassData), 13, "test-46", EnumType.Second)]
        public void TestWithFrozenParametersReceivesExpectedData(
            [Frozen] int a, [Frozen] string b, [Frozen] EnumType c,
            PropertyHolder<int> a1, PropertyHolder<string> b1, PropertyHolder<EnumType> c1)
        {
            Assert.Equal(13, a);
            Assert.Equal("test-46", b);
            Assert.Equal(EnumType.Second, c);

            Assert.Equal(a, a1.Property);
            Assert.Equal(b, b1.Property);
            Assert.Equal(c, c1.Property);
        }

        [Theory]
        [ClassAutoData(typeof(ParameterizedClassData), 59, "hello-world", EnumType.Second)]
        public void TestWithInjectedValuesRespectsOtherParameterCustomizations(
            [Frozen] int a, [Frozen] string b, [Frozen] EnumType c,
            [FavorEnumerables] CompositeTypeWithOverloadedConstructors<int> numbers,
            [FavorArrays] CompositeTypeWithOverloadedConstructors<string> strings,
            [FavorLists] CompositeTypeWithOverloadedConstructors<EnumType> enums)
        {
            Assert.IsAssignableFrom<IEnumerable<int>>(numbers.Items);
            Assert.IsNotType<List<int>>(numbers.Items);
            Assert.IsNotType<int[]>(numbers.Items);
            Assert.All(numbers.Items, item => Assert.Equal(a, item));

            Assert.IsType<string[]>(strings.Items);
            Assert.All(strings.Items, item => Assert.Equal(b, item));

            Assert.IsType<List<EnumType>>(enums.Items);
            Assert.All(enums.Items, item => Assert.Equal(c, item));
        }
    }
}
