using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture.Xunit3.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit3.UnitTest
{
    public class Scenario
    {
        [Theory, AutoData]
        public void AutoDataProvidesCorrectInteger(int primitiveValue)
        {
            Assert.NotEqual(0, primitiveValue);
        }

        [Theory, AutoData]
        public void AutoDataProvidesCorrectString(string text)
        {
            Assert.StartsWith("text", text);
        }

        [Theory, AutoData]
        public void AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [Theory, AutoData]
        public void AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<ConcreteType> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [Theory]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
        }

        [Theory]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotNull(myClass);
        }

        [Theory]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotEqual("foo", s3);
            Assert.NotEqual("bar", s3);
        }

        // This test and its associated types is used to document one of the
        // InlineAutoDataAttribute constructor overloads.
        [Theory]
        [MyCustomInlineAutoData(1337)]
        [MyCustomInlineAutoData(1337, 7)]
        [MyCustomInlineAutoData(1337, 7, 42)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters - it's required by the test logic.
        public void CustomInlineDataSuppliesExtraValues(int x, int y, int z)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            Assert.Equal(1337, x);

            // y can vary, so we can't express any meaningful assertion for it.
            Assert.Equal(42, z);
        }

        private class MyCustomInlineAutoDataAttribute : InlineAutoDataAttribute
        {
            public MyCustomInlineAutoDataAttribute(params object[] values)
                : base(() => new Fixture().Customize(new TheAnswer()), values)
            {
            }
        }

        [Theory, MemberAutoData(nameof(StringData))]
        public void MemberAutoDataUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
        }

        [Theory, MemberAutoData(nameof(StringData))]
        public void MemberAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotNull(myClass);
        }

        [Theory, MemberAutoData(nameof(StringData))]
        public void MemberAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.Equal("foo", s1);
            Assert.NotNull(s2);
            Assert.NotEqual("foo", s3);
            Assert.NotEqual("bar", s3);
        }

        [Theory, MemberAutoData(nameof(GetParametrizedData), 21, 38, 43)]
        public void MemberAutoDataCanBeParametrized(int x, int y, int z)
        {
            Assert.Equal(21, x);
            Assert.Equal(38, y);
            Assert.Equal(43, z);
        }

        [Theory, MyCustomMemberAutoData(nameof(IntData))]
        public void CustomMemberAutoDataSuppliesExtraValues(int x, int y, int z)
        {
            Assert.Equal(1337, x);
            Assert.NotEqual(0, y);
            Assert.Equal(42, z);
        }

        [Theory, MyCustomMemberAutoData(nameof(GetParametrizedData), 21, 38, 43)]
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
                yield return new object[] { "foo", };
                yield return new object[] { "foo", "bar" };
            }
        }

        public static IEnumerable<object[]> IntData
        {
            get
            {
                yield return new object[] { 1337, };
                yield return new object[] { 1337, 7 };
                yield return new object[] { 1337, 7, 42 };
            }
        }

        public static IEnumerable<object[]> GetParametrizedData(int x, int y, int z)
        {
            yield return new object[] { x, y, z };
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

        [Theory, AutoData]
        public void FreezeFirstParameter([Frozen] Guid g1, Guid g2)
        {
            Assert.Equal(g1, g2);
        }

        [Theory, AutoData]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen] Guid g2, Guid g3)
        {
            Assert.NotEqual(g1, g2);
            Assert.NotEqual(g1, g3);

            Assert.Equal(g2, g3);
        }

        [Theory, AutoData]
        public void IntroductoryTest(int expectedNumber, MyClass sut)
        {
            // Arrange
            // Act
            int result = sut.Echo(expectedNumber);

            // Assert
            Assert.Equal(expectedNumber, result);
        }

        [Theory, AutoData]
        public void ModestCreatesParameterWithModestConstructor([Modest] MultiUnorderedConstructorType p)
        {
            Assert.True(string.IsNullOrEmpty(p.Text));
            Assert.Equal(0, p.Number);
        }

        [Theory, AutoData]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy] MultiUnorderedConstructorType p)
        {
            Assert.False(string.IsNullOrEmpty(p.Text));
            Assert.NotEqual(0, p.Number);
        }

        [Theory, AutoData]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter(
            [Frozen] [Greedy] MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
        {
            Assert.NotNull(p1);
            Assert.False(string.IsNullOrEmpty(p2.Text));
            Assert.NotEqual(0, p2.Number);
        }

        [Theory, AutoData]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen] int[] numbers,
            [FavorArrays] ItemContainer<int> container)
        {
            Assert.True(numbers.SequenceEqual(container.Items));
        }

        [Theory, AutoData]
        public void FreezeFirstParameterShouldAssignSameInstanceToSecondParameter([Frozen] string p1,
            string p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByExactTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ExactType)] ConcreteType p1,
            ConcreteType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByExactTypeShouldNotAssignSameInstanceToSecondParameterOfDifferentType(
            [Frozen(Matching.ExactType)] ConcreteType p1,
            object p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.DirectBaseType)] ConcreteType p1,
            AbstractType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfIndirectBaseType(
            [Frozen(Matching.DirectBaseType)] ConcreteType p1,
            object p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.DirectBaseType)] ConcreteType p1,
            ConcreteType p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByExactOrDirectBaseTypeShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.DirectBaseType)]
            ConcreteType p1,
            ConcreteType p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByInterfaceShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            IInterface p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfNonInterfaceType(
            [Frozen(Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            object p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.NotEqual(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByDirectOrInterfaceShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.ImplementedInterfaces)]
            NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.Equal(p1, p2);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByParameterWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)] string parameter,
            SingleParameterType<object> p2)
        {
            Assert.Equal(parameter, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)] string p1,
            SingleParameterType<object> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Theory, AutoData]
        public void
            FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
                [Frozen(Matching.ParameterName)] string p1,
                SingleParameterType<string> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByPropertyWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)] string property,
            PropertyHolder<object> p2)
        {
            Assert.Equal(property, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)] string p1,
            PropertyHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Theory, AutoData]
        public void
            FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
                [Frozen(Matching.PropertyName)] string p1,
                PropertyHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByFieldWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)] string field,
            FieldHolder<object> p2)
        {
            Assert.Equal(field, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)] string p1,
            FieldHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.FieldName)] string p1,
            FieldHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingParameter(
            [Frozen(Matching.MemberName)] string parameter,
            SingleParameterType<object> p2)
        {
            Assert.Equal(parameter, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameter(
            [Frozen(Matching.MemberName)] string p1,
            SingleParameterType<object> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameterOfSameType(
            [Frozen(Matching.MemberName)] string p1,
            SingleParameterType<string> p2)
        {
            Assert.NotEqual(p1, p2.Parameter);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingProperty(
            [Frozen(Matching.MemberName)] string property,
            PropertyHolder<object> p2)
        {
            Assert.Equal(property, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToProperty(
            [Frozen(Matching.MemberName)] string p1,
            PropertyHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToPropertyOfSameType(
            [Frozen(Matching.MemberName)] string p1,
            PropertyHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Property);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingField(
            [Frozen(Matching.MemberName)] string field,
            FieldHolder<object> p2)
        {
            Assert.Equal(field, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToField(
            [Frozen(Matching.MemberName)] string p1,
            FieldHolder<object> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToFieldOfSameType(
            [Frozen(Matching.MemberName)] string p1,
            FieldHolder<string> p2)
        {
            Assert.NotEqual(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeParameterWithStringLengthConstraintShouldCreateConstrainedSpecimen(
            [Frozen, StringLength(3)] string p)
        {
            Assert.True(p.Length == 3);
        }

        [Theory, ClassAutoData(typeof(StringDataClass))]
        public void ClassAutoDataUsesValuesSuppliedByClass(string s1, string s2, string s3)
        {
            Assert.Contains(s1, new[] { "foo", "dim" });
            Assert.NotEmpty(s2);
            Assert.NotEmpty(s3);
        }

        [Theory, ClassAutoData(typeof(StringDataClass))]
        public void ClassAutoDataSuppliesDataSpecimens(string s1, string s2, string s3, MyClass myClass)
        {
            Assert.NotEmpty(s1);
            Assert.NotEmpty(s2);
            Assert.NotEmpty(s3);
            Assert.NotNull(myClass);
        }

        [Theory, ClassAutoData(typeof(MixedDataClass))]
        public void ClassAutoDataSuppliesDataOfMixedTypes(int p1, string p2, PropertyHolder<string> p3, MyClass myClass)
        {
            Assert.NotEqual(0, p1);
            Assert.NotEmpty(p2);
            Assert.NotNull(p3);
            Assert.NotEmpty(p3.Property);
            Assert.NotNull(myClass);
        }

        [Theory, ClassAutoData(typeof(ParameterizedDataClass), 28, "bar", 93.102)]
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
                yield return new object[] { "foo", "bar", "foobar" };
                yield return new object[] { "dim", "sum", "dimsum" };
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        public class MixedDataClass : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1 };
                yield return new object[] { 4, "testValue" };
                yield return new object[] { 20, "otherValue", new PropertyHolder<string> { Property = "testValue1" } };
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
                yield return new object[] { this.p1, this.p2, this.p3 };
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}