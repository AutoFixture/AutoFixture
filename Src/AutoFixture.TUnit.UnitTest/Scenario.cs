using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest;

public class Scenario
{
    [Test, AutoData]
    public async Task AutoDataProvidesCorrectInteger(int primitiveValue)
    {
        await Assert.That(primitiveValue).IsNotEqualTo(0);
    }

    [Test, AutoData]
    public async Task AutoDataProvidesCorrectString(string text)
    {
        await Assert.That(text).StartsWith("text");
    }

    [Test, AutoData]
    public async Task AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
    {
        await Assert.That(ph).IsNotNull();
        await Assert.That(ph.Property).IsNotNull();
    }

    [Test, AutoData]
    public async Task AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<ConcreteType> spt)
    {
        await Assert.That(ph).IsNotNull();
        await Assert.That(ph.Property).IsNotNull();

        await Assert.That(spt).IsNotNull();
        await Assert.That(spt.Parameter).IsNotNull();
    }

    [Test]
    [ArgumentsAutoData("foo")]
    [ArgumentsAutoData("foo", "bar")]
    public async Task InlineAutoDataUsesSuppliedDataValues(string s1, string s2)
    {
        await Assert.That(s1).IsEqualTo("foo");
        await Assert.That(s2).IsNotNull();
    }

    [Test]
    [ArgumentsAutoData("foo")]
    [ArgumentsAutoData("foo", "bar")]
    public async Task InlineAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
    {
        await Assert.That(s1).IsEqualTo("foo");
        await Assert.That(s2).IsNotNull();
        await Assert.That(myClass).IsNotNull();
    }

    [Test]
    [ArgumentsAutoData("foo")]
    [ArgumentsAutoData("foo", "bar")]
    public async Task InlineAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
    {
        await Assert.That(s1).IsEqualTo("foo");
        await Assert.That(s2).IsNotNull();
        await Assert.That(s3).IsNotEqualTo("foo");
        await Assert.That(s3).IsNotEqualTo("bar");
    }

    // This test and its associated types is used to document one of the
    // InlineAutoDataAttribute constructor overloads.
    [Test]
    [MyCustomArgumentsAutoData(1337)]
    [MyCustomArgumentsAutoData(1337, 7)]
    [MyCustomArgumentsAutoData(1337, 7, 42)]
#pragma warning disable xUnit1026 // Test methods should use all of their parameters - it's required by the test logic.
    public async Task CustomInlineDataSuppliesExtraValues(int x, int y, int z)
#pragma warning restore xUnit1026 // Test methods should use all of their parameters
    {
        await Assert.That(x).IsEqualTo(1337);

        // y can vary, so we can't express any meaningful assertion for it.
        await Assert.That(z).IsEqualTo(42);
    }

    public class MyCustomArgumentsAutoDataAttribute : ArgumentsAutoDataAttribute
    {
        public MyCustomArgumentsAutoDataAttribute(params object[] values)
            : base(() => new Fixture().Customize(new TheAnswer()), values)
        {
        }
    }

    [Test, MemberAutoData(nameof(StringData))]
    public async Task MemberAutoDataUsesSuppliedDataValues(string s1, string s2)
    {
        await Assert.That(s1).IsEqualTo("foo");
        await Assert.That(s2).IsNotNull();
    }

    [Test, MemberAutoData(nameof(StringData))]
    public async Task MemberAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
    {
        await Assert.That(s1).IsEqualTo("foo");
        await Assert.That(s2).IsNotNull();
        await Assert.That(myClass).IsNotNull();
    }

    [Test, MemberAutoData(nameof(StringData))]
    public async Task MemberAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
    {
        await Assert.That(s1).IsEqualTo("foo");
        await Assert.That(s2).IsNotNull();
        await Assert.That(s3).IsNotEqualTo("foo");
        await Assert.That(s3).IsNotEqualTo("bar");
    }

    [Test, MemberAutoData(nameof(GetParametrizedData), 21, 38, 43)]
    public async Task MemberAutoDataCanBeParametrized(int x, int y, int z)
    {
        await Assert.That(x).IsEqualTo(21);
        await Assert.That(y).IsEqualTo(38);
        await Assert.That(z).IsEqualTo(43);
    }

    [Test, MyCustomMemberAutoData(nameof(IntData))]
    public async Task CustomMemberAutoDataSuppliesExtraValues(int x, int y, int z)
    {
        await Assert.That(x).IsEqualTo(1337);
        await Assert.That(y).IsNotEqualTo(0);
        await Assert.That(z).IsEqualTo(42);
    }

    [Test, MyCustomMemberAutoData(nameof(GetParametrizedData), 21, 38, 43)]
    public async Task CustomMemberAutoDataCanBeParametrized(int x, int y, int z)
    {
        await Assert.That(x).IsEqualTo(21);
        await Assert.That(y).IsEqualTo(38);
        await Assert.That(z).IsEqualTo(43);
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

    public class MyCustomMemberAutoDataAttribute : MemberAutoDataAttribute
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
    public async Task FreezeFirstParameter([Frozen] Guid g1, Guid g2)
    {
        await Assert.That(g2).IsEqualTo(g1);
    }

    [Test, AutoData]
    public async Task FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen] Guid g2, Guid g3)
    {
        await Assert.That(g2).IsNotEqualTo(g1);
        await Assert.That(g3).IsNotEqualTo(g1);

        await Assert.That(g3).IsEqualTo(g2);
    }

    [Test, AutoData]
    public async Task IntroductoryTest(int expectedNumber, MyClass sut)
    {
        // Arrange
        // Act
        int result = sut.Echo(expectedNumber);

        // Assert
        await Assert.That(result).IsEqualTo(expectedNumber);
    }

    [Test, AutoData]
    public async Task ModestCreatesParameterWithModestConstructor([Modest] MultiUnorderedConstructorType p)
    {
        await Assert.That(string.IsNullOrEmpty(p.Text)).IsTrue();
        await Assert.That(p.Number).IsEqualTo(0);
    }

    [Test, AutoData]
    public async Task GreedyCreatesParameterWithGreedyConstructor([Greedy] MultiUnorderedConstructorType p)
    {
        await Assert.That(string.IsNullOrEmpty(p.Text)).IsFalse();
        await Assert.That(p.Number).IsNotEqualTo(0);
    }

    [Test, AutoData]
    public async Task BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter(
        [Frozen][Greedy] MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
    {
        await Assert.That(p1).IsNotNull();
        await Assert.That(string.IsNullOrEmpty(p2.Text)).IsFalse();
        await Assert.That(p2.Number).IsNotEqualTo(0);
    }

    [Test, AutoData]
    public async Task FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen] int[] numbers,
        [FavorArrays] ItemContainer<int> container)
    {
        await Assert.That(numbers.SequenceEqual(container.Items)).IsTrue();
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterShouldAssignSameInstanceToSecondParameter([Frozen] string p1,
        string p2)
    {
        await Assert.That(p2).IsEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByExactTypeShouldAssignSameInstanceToSecondParameter(
        [Frozen(Matching.ExactType)] ConcreteType p1,
        ConcreteType p2)
    {
        await Assert.That(p2).IsEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByExactTypeShouldNotAssignSameInstanceToSecondParameterOfDifferentType(
        [Frozen(Matching.ExactType)] ConcreteType p1,
        object p2)
    {
        await Assert.That(p2).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByDirectBaseTypeShouldAssignSameInstanceToSecondParameter(
        [Frozen(Matching.DirectBaseType)] ConcreteType p1,
        AbstractType p2)
    {
        await Assert.That(p2).IsEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfIndirectBaseType(
        [Frozen(Matching.DirectBaseType)] ConcreteType p1,
        object p2)
    {
        await Assert.That(p2).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.DirectBaseType)] ConcreteType p1,
        ConcreteType p2)
    {
        await Assert.That(p2).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByExactOrDirectBaseTypeShouldAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.ExactType | Matching.DirectBaseType)]
        ConcreteType p1,
        ConcreteType p2)
    {
        await Assert.That(p2).IsEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByInterfaceShouldAssignSameInstanceToSecondParameter(
        [Frozen(Matching.ImplementedInterfaces)]
        NoopInterfaceImplementer p1,
        IInterface p2)
    {
        await Assert.That(p2).IsEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfNonInterfaceType(
        [Frozen(Matching.ImplementedInterfaces)]
        NoopInterfaceImplementer p1,
        object p2)
    {
        await Assert.That(p2).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.ImplementedInterfaces)]
        NoopInterfaceImplementer p1,
        NoopInterfaceImplementer p2)
    {
        await Assert.That(p2).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByDirectOrInterfaceShouldAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.ExactType | Matching.ImplementedInterfaces)]
        NoopInterfaceImplementer p1,
        NoopInterfaceImplementer p2)
    {
        await Assert.That(p2).IsEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByParameterWithSameNameShouldAssignSameInstanceToSecondParameter(
        [Frozen(Matching.ParameterName)] string parameter,
        SingleParameterType<object> p2)
    {
        await Assert.That(p2.Parameter).IsEqualTo(parameter);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
        [Frozen(Matching.ParameterName)] string p1,
        SingleParameterType<object> p2)
    {
        await Assert.That(p2.Parameter).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.ParameterName)] string p1,
        SingleParameterType<string> p2)
    {
        await Assert.That(p2.Parameter).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByPropertyWithSameNameShouldAssignSameInstanceToSecondParameter(
        [Frozen(Matching.PropertyName)] string property,
        PropertyHolder<object> p2)
    {
        await Assert.That(p2.Property).IsEqualTo(property);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
        [Frozen(Matching.PropertyName)] string p1,
        PropertyHolder<object> p2)
    {
        await Assert.That(p2.Property).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.PropertyName)] string p1,
        PropertyHolder<string> p2)
    {
        await Assert.That(p2.Property).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByFieldWithSameNameShouldAssignSameInstanceToSecondParameter(
        [Frozen(Matching.FieldName)] string field,
        FieldHolder<object> p2)
    {
        await Assert.That(p2.Field).IsEqualTo(field);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
        [Frozen(Matching.FieldName)] string p1,
        FieldHolder<object> p2)
    {
        await Assert.That(p2.Field).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
        [Frozen(Matching.FieldName)] string p1,
        FieldHolder<string> p2)
    {
        await Assert.That(p2.Field).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingParameter(
        [Frozen(Matching.MemberName)] string parameter,
        SingleParameterType<object> p2)
    {
        await Assert.That(p2.Parameter).IsEqualTo(parameter);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameter(
        [Frozen(Matching.MemberName)] string p1,
        SingleParameterType<object> p2)
    {
        await Assert.That(p2.Parameter).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameterOfSameType(
        [Frozen(Matching.MemberName)] string p1,
        SingleParameterType<string> p2)
    {
        await Assert.That(p2.Parameter).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingProperty(
        [Frozen(Matching.MemberName)] string property,
        PropertyHolder<object> p2)
    {
        await Assert.That(p2.Property).IsEqualTo(property);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToProperty(
        [Frozen(Matching.MemberName)] string p1,
        PropertyHolder<object> p2)
    {
        await Assert.That(p2.Property).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToPropertyOfSameType(
        [Frozen(Matching.MemberName)] string p1,
        PropertyHolder<string> p2)
    {
        await Assert.That(p2.Property).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingField(
        [Frozen(Matching.MemberName)] string field,
        FieldHolder<object> p2)
    {
        await Assert.That(p2.Field).IsEqualTo(field);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToField(
        [Frozen(Matching.MemberName)] string p1,
        FieldHolder<object> p2)
    {
        await Assert.That(p2.Field).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToFieldOfSameType(
        [Frozen(Matching.MemberName)] string p1,
        FieldHolder<string> p2)
    {
        await Assert.That(p2.Field).IsNotEqualTo(p1);
    }

    [Test, AutoData]
    public async Task FreezeParameterWithStringLengthConstraintShouldCreateConstrainedSpecimen(
        [Frozen, StringLength(3)] string p)
    {
        await Assert.That(p.Length == 3).IsTrue();
    }

    [Test, ClassAutoData(typeof(StringDataClass))]
    public async Task ClassAutoDataUsesValuesSuppliedByClass(string s1, string s2, string s3)
    {
        await Assert.That(new[] { "foo", "dim" }).Contains(s1);
        await Assert.That(s2).IsNotEmpty();
        await Assert.That(s3).IsNotEmpty();
    }

    [Test, ClassAutoData(typeof(StringDataClass))]
    public async Task ClassAutoDataSuppliesDataSpecimens(string s1, string s2, string s3, MyClass myClass)
    {
        await Assert.That(s1).IsNotEmpty();
        await Assert.That(s2).IsNotEmpty();
        await Assert.That(s3).IsNotEmpty();
        await Assert.That(myClass).IsNotNull();
    }

    [Test, ClassAutoData(typeof(MixedDataClass))]
    public async Task ClassAutoDataSuppliesDataOfMixedTypes(int p1, string p2, PropertyHolder<string> p3, MyClass myClass)
    {
        await Assert.That(p1).IsNotEqualTo(0);
        await Assert.That(p2).IsNotEmpty();
        await Assert.That(p3).IsNotNull();
        await Assert.That(p3.Property).IsNotEmpty();
        await Assert.That(myClass).IsNotNull();
    }

    [Test, ClassAutoData(typeof(ParameterizedDataClass), 28, "bar", 93.102)]
    public async Task ClassAutoDataCanBeParameterized(int p1, string p2, double p3, RecordType<double> p4)
    {
        var actual = new object[] { p1, p2, p3 };
        var expected = new object[] { 28, "bar", 93.102 };

        await Assert.That(actual).IsEquivalentTo(expected);
        await Assert.That(p4).IsNotNull();
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