using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest
{
    public class ClassAutoDataScenarioTests
    {
        [Test]
        [ClassAutoData(typeof(MixedTypeClassData))]
        public void TestWithMixedTypesPasses(int? a, string b, EnumType? c, Tuple<string, int> d)
        {
            Assert.That(a).IsNotNull();
            Assert.That(b).IsNotNull();
            Assert.That(c).IsNotNull();
            Assert.That(d).IsNotNull();
        }

        [Test]
        [ClassAutoData(typeof(ParameterizedClassData), 42, "test-13", EnumType.Third)]
        public void TestWithParameterizedClassDataReceivesExpectedData(
            int a, string b, EnumType c, PropertyHolder<string> d)
        {
            Assert.That(a).IsEqualTo(42);
            Assert.That(b).IsEqualTo("test-13");
            Assert.That(c).IsEqualTo(EnumType.Third);
            Assert.That(d?.Property).IsNotNull();
        }

        [Test]
        [ClassAutoData(typeof(ParameterizedClassData), 13, "test-46", EnumType.Second)]
        public void TestWithFrozenParametersReceivesExpectedData(
            [Frozen] int a, [Frozen] string b, [Frozen] EnumType c,
            PropertyHolder<int> a1, PropertyHolder<string> b1, PropertyHolder<EnumType> c1)
        {
            Assert.That(a).IsEqualTo(13);
            Assert.That(b).IsEqualTo("test-46");
            Assert.That(c).IsEqualTo(EnumType.Second);

            Assert.That(a1.Property).IsEqualTo(a);
            Assert.That(b1.Property).IsEqualTo(b);
            Assert.That(c1.Property).IsEqualTo(c);
        }

        [Test]
        [ClassAutoData(typeof(ParameterizedClassData), 59, "hello-world", EnumType.Second)]
        public void TestWithInjectedValuesRespectsOtherParameterCustomizations(
            [Frozen] int a, [Frozen] string b, [Frozen] EnumType c,
            [FavorEnumerables] CompositeTypeWithOverloadedConstructors<int> numbers,
            [FavorArrays] CompositeTypeWithOverloadedConstructors<string> strings,
            [FavorLists] CompositeTypeWithOverloadedConstructors<EnumType> enums)
        {
            Assert.That(numbers.Items).IsAssignableFrom<IEnumerable<int>>();
            Assert.That(numbers.Items).IsNotTypeOf<List<int>>();
            Assert.That(numbers.Items).IsNotTypeOf<int[]>();
            Assert.That(numbers.Items).All().Satisfy(item => item.IsEqualTo(a));

            Assert.That(strings.Items).IsTypeOf<string[]>();
            Assert.That(strings.Items).All().Satisfy(item => item.IsEqualTo(b));
            
            Assert.That(enums.Items).IsTypeOf<List<EnumType>>();
            Assert.That(enums.Items).All().Satisfy(item => item.IsEqualTo(c));
        }
    }
}
