using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest
{
    public class Scenario
    {
        [Test, AutoData]
        public void AutoDataProvidesCorrectInteger(int primitiveValue)
        {
            Assert.NotEqual(0, primitiveValue);
        }

        [Test, AutoData]
        public void AutoDataProvidesCorrectString(string text)
        {
            Assert.StartsWith("text", text);
        }

        [Test, AutoData]
        public void AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [Test, AutoData]
        public void AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<ConcreteType> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [Test]
        [ArgumentsAutoData("foo")]
        [ArgumentsAutoData("foo", "bar")]
        public void InlineAutoDataUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
        }

        [Test]
        [ArgumentsAutoData("foo")]
        [ArgumentsAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotNull(myClass);
        }

        [Test]
        [ArgumentsAutoData("foo")]
        [ArgumentsAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotEqual("foo", s3);
            Assert.NotEqual("bar", s3);
        }

        // This test and its associated types is used to document one of the
        // InlineAutoDataAttribute constructor overloads.
        [Test]
        [MyCustomArgumentsAutoData(1337)]
        [MyCustomArgumentsAutoData(1337, 7)]
        [MyCustomArgumentsAutoData(1337, 7, 42)]
#pragma warning disable xUnit1026 // Test methods should use all of their parameters - it's required by the test logic.
        public void CustomInlineDataSuppliesExtraValues(int x, int y, int z)
#pragma warning restore xUnit1026 // Test methods should use all of their parameters
        {
            Assert.Equal(1337, x);

            // y can vary, so we can't express any meaningful assertion for it.
            Assert.Equal(42, z);
        }

        private class MyCustomArgumentsAutoDataAttribute : ArgumentsAutoDataAttribute
        {
            public MyCustomArgumentsAutoDataAttribute(params object[] values)
                : base(() => new Fixture().Customize(new TheAnswer()), values)
            {
            }
        }

        [Test, MemberAutoData(nameof(StringData))]
        public void MemberAutoDataUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
        }

        [Test, MemberAutoData(nameof(StringData))]
        public void MemberAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotNull(myClass);
        }

        [Test, MemberAutoData(nameof(StringData))]
        public void MemberAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotEqual("foo", s3);
            Assert.NotEqual("bar", s3);
        }

        [Test, MemberAutoData(nameof(GetParametrizedData), 21, 38, 43)]
        public void MemberAutoDataCanBeParametrized(int x, int y, int z)
        {
            Assert.Equal(21, x);
            Assert.Equal(38, y);
            Assert.Equal(43, z);
        }

        [Test, MyCustomMemberAutoData(nameof(IntData))]
        public void CustomMemberAutoDataSuppliesExtraValues(int x, int y, int z)
        {
            Assert.Equal(1337, x);
            Assert.NotEqual(0, y);
            Assert.Equal(42, z);
        }

        [Test, MyCustomMemberAutoData(nameof(GetParametrizedData), 21, 38, 43)]
        public void CustomMemberAutoDataCanBeParametrized(int x, int y, int z)
        {
            Assert.Equal(21, x);
            Assert.Equal(38, y);
            Assert.Equal(43, z);
        }

        public static IEnumerable<object[]> StringData
        {
            get
            {
                yield return ["foo"];
                yield return ["foo", "bar"];
            }
        }

        public static IEnumerable<object[]> IntData
        {
            get
            {
                yield return [1337];
                yield return [1337, 7];
                yield return [1337, 7, 42];
            }
        }

        public static IEnumerable<object[]> GetParametrizedData(int x, int y, int z)
        {
            yield return [x, y, z];
        }

        private class MyCustomMemberAutoDataAttribute : MemberAutoDataAttribute
        {
            public MyCustomMemberAutoDataAttribute(string memberName, params object[] parameters)
                : base(() => new Fixture().Customize(new TheAnswer()), memberName, parameters)
            {
            }
        }

        private class TheAnswer : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Inject(42);
            }
        }

        [Test, AutoData]
        public void FreezeFirstParameter([Frozen] Guid g1, Guid g2)
        {
            Assert.Equal(g1, g2);
        }

        [Test, AutoData]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen] Guid g2, Guid g3)
        {
            Assert.NotEqual(g1, g2);
            Assert.NotEqual(g1, g3);

            Assert.Equal(g2, g3);
        }

        [Test, AutoData]
        public void IntroductoryTest(int expectedNumber, MyClass sut)
        {
            // Arrange
            // Act
            int result = sut.Echo(expectedNumber);

            // Assert
            Assert.Equal(expectedNumber, result);
        }

        [Test, AutoData]
        public void ModestCreatesParameterWithModestConstructor([Modest] MultiUnorderedConstructorType p)
        {
            Assert.True(string.IsNullOrEmpty(p.Text));
            Assert.Equal(0, p.Number);
        }

        [Test, AutoData]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy] MultiUnorderedConstructorType p)
        {
            Assert.False(string.IsNullOrEmpty(p.Text));
            Assert.NotEqual(0, p.Number);
        }

        [Test, AutoData]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter(
            [Frozen] [Greedy] MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
        {
            Assert.NotNull(p1);
            Assert.False(string.IsNullOrEmpty(p2.Text));
            Assert.NotEqual(0, p2.Number);
        }

        [Test, AutoData]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen] int[] numbers,
            [FavorArrays] ItemContainer<int> container)
        {
            Assert.True(numbers.SequenceEqual(container.Items));
        }

        [Test, AutoData]
        public void FreezeFirstParameterShouldAssignSameInstanceToSecondParameter([Frozen] string p1,
            string p2)
        {
            Assert.Equal(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByExactTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ExactType)] ConcreteType p1,
            ConcreteType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByExactTypeShouldNotAssignSameInstanceToSecondParameterOfDifferentType(
            [Frozen(Matching.ExactType)] ConcreteType p1,
            object p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.DirectBaseType)] ConcreteType p1,
            AbstractType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfIndirectBaseType(
            [Frozen(Matching.DirectBaseType)] ConcreteType p1,
            object p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.DirectBaseType)] ConcreteType p1,
            ConcreteType p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByExactOrDirectBaseTypeShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.DirectBaseType)]
            ConcreteType p1,
            ConcreteType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByInterfaceShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            IInterface p2)
        {
            Assert.Equal(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfNonInterfaceType(
            [Frozen(Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            object p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectOrInterfaceShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.Equal(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByParameterWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)] string parameter,
            SingleParameterType<object> p2)
        {
            Assert.Equal(parameter, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)] string p1,
            SingleParameterType<object> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void
            FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
                [Frozen(Matching.ParameterName)] string p1,
                SingleParameterType<string> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByPropertyWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)] string property,
            PropertyHolder<object> p2)
        {
            Assert.Equal(property, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)] string p1,
            PropertyHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void
            FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
                [Frozen(Matching.PropertyName)] string p1,
                PropertyHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByFieldWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)] string field,
            FieldHolder<object> p2)
        {
            Assert.Equal(field, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)] string p1,
            FieldHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.FieldName)] string p1,
            FieldHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingParameter(
            [Frozen(Matching.MemberName)] string parameter,
            SingleParameterType<object> p2)
        {
            Assert.Equal(parameter, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameter(
            [Frozen(Matching.MemberName)] string p1,
            SingleParameterType<object> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameterOfSameType(
            [Frozen(Matching.MemberName)] string p1,
            SingleParameterType<string> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingProperty(
            [Frozen(Matching.MemberName)] string property,
            PropertyHolder<object> p2)
        {
            Assert.Equal(property, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToProperty(
            [Frozen(Matching.MemberName)] string p1,
            PropertyHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToPropertyOfSameType(
            [Frozen(Matching.MemberName)] string p1,
            PropertyHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingField(
            [Frozen(Matching.MemberName)] string field,
            FieldHolder<object> p2)
        {
            Assert.Equal(field, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToField(
            [Frozen(Matching.MemberName)] string p1,
            FieldHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToFieldOfSameType(
            [Frozen(Matching.MemberName)] string p1,
            FieldHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeParameterWithStringLengthConstraintShouldCreateConstrainedSpecimen(
            [Frozen, StringLength(3)] string p)
        {
            Assert.True(p.Length == 3);
        }

        [Test, ClassAutoData(typeof(StringDataClass))]
        public void ClassAutoDataUsesValuesSuppliedByClass(string s1, string s2, string s3)
        {
            Assert.Contains(s1, new[] { "foo", "dim" });
            Assert.NotEmpty(s2);
            Assert.NotEmpty(s3);
        }

        [Test, ClassAutoData(typeof(StringDataClass))]
        public void ClassAutoDataSuppliesDataSpecimens(string s1, string s2, string s3, MyClass myClass)
        {
            Assert.NotEmpty(s1);
            Assert.NotEmpty(s2);
            Assert.NotEmpty(s3);
            Assert.NotNull(myClass);
        }

        [Test, ClassAutoData(typeof(MixedDataClass))]
        public void ClassAutoDataSuppliesDataOfMixedTypes(int p1, string p2, PropertyHolder<string> p3, MyClass myClass)
        {
            Assert.NotEqual(0, p1);
            Assert.NotEmpty(p2);
            Assert.NotNull(p3);
            Assert.NotEmpty(p3.Property);
            Assert.NotNull(myClass);
        }

        [Test, ClassAutoData(typeof(ParameterizedDataClass), 28, "bar", 93.102)]
        public void ClassAutoDataCanBeParameterized(int p1, string p2, double p3, RecordType<double> p4)
        {
            var actual = new object[] { p1, p2, p3 };
            var expected = new object[] { 28, "bar", 93.102 };

            Assert.Equal(expected, actual);
            Assert.NotNull(p4);
        }

        public class StringDataClass : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return ["foo", "bar", "foobar"];
                yield return ["dim", "sum", "dimsum"];
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        public class MixedDataClass : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return [1];
                yield return [4, "testValue"];
                yield return [20, "otherValue", new PropertyHolder<string> { Property = "testValue1" }];
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        public class ParameterizedDataClass : IEnumerable<object[]>
        {
            private readonly int p1;
            private readonly string p2;
            private readonly double p3;

            public ParameterizedDataClass(int p1, string p2, double p3)
            {
                this.p1 = p1;
                this.p2 = p2;
                this.p3 = p3;
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return [this.p1, this.p2, this.p3];
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}