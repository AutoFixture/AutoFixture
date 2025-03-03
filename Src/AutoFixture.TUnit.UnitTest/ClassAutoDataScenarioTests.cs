using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest
{
    public class ClassAutoDataScenarioTests
    {
        [Test]
        [ClassAutoData(typeof(MixedTypeClassData))]
        public async Task TestWithMixedTypesPasses(int? a, string b, EnumType? c, Tuple<string, int> d)
        {
            await Assert.That(a).IsNotNull();
            await Assert.That(b).IsNotNull();
            await Assert.That(c).IsNotNull();
            await Assert.That(d).IsNotNull();
        }

        [Test]
        [ClassAutoData(typeof(ParameterizedClassData), 42, "test-13", EnumType.Third)]
        public async Task TestWithParameterizedClassDataReceivesExpectedData(
            int a, string b, EnumType c, PropertyHolder<string> d)
        {
            await Assert.That(a).IsEqualTo(42);
            await Assert.That(b).IsEqualTo("test-13");
            await Assert.That(c).IsEqualTo(EnumType.Third);
            await Assert.That(d?.Property).IsNotNull();
        }

        [Test]
        [ClassAutoData(typeof(ParameterizedClassData), 13, "test-46", EnumType.Second)]
        public async Task TestWithFrozenParametersReceivesExpectedData(
            [Frozen] int a, [Frozen] string b, [Frozen] EnumType c,
            PropertyHolder<int> a1, PropertyHolder<string> b1, PropertyHolder<EnumType> c1)
        {
            await Assert.That(a).IsEqualTo(13);
            await Assert.That(b).IsEqualTo("test-46");
            await Assert.That(c).IsEqualTo(EnumType.Second);

            await Assert.That(a1.Property).IsEqualTo(a);
            await Assert.That(b1.Property).IsEqualTo(b);
            await Assert.That(c1.Property).IsEqualTo(c);
        }

        [Test]
        [ClassAutoData(typeof(ParameterizedClassData), 59, "hello-world", EnumType.Second)]
        public async Task TestWithInjectedValuesRespectsOtherParameterCustomizations(
            [Frozen] int a, [Frozen] string b, [Frozen] EnumType c,
            [FavorEnumerables] CompositeTypeWithOverloadedConstructors<int> numbers,
            [FavorArrays] CompositeTypeWithOverloadedConstructors<string> strings,
            [FavorLists] CompositeTypeWithOverloadedConstructors<EnumType> enums)
        {
            await Assert.That(numbers.Items).IsAssignableFrom<IEnumerable<int>>();
            await Assert.That(numbers.Items).IsNotTypeOf<List<int>>();
            await Assert.That(numbers.Items).IsNotTypeOf<int[]>();
            await Assert.That(numbers.Items).All().Satisfy(item => item.IsEqualTo(a));

            await Assert.That(strings.Items).IsTypeOf<string[]>();
            await Assert.That(strings.Items).All().Satisfy(item => item.IsEqualTo(b));

            await Assert.That(enums.Items).IsTypeOf<List<EnumType>>();
            await Assert.That(enums.Items).All().Satisfy(item => item.IsEqualTo(c));
        }
    }
}
