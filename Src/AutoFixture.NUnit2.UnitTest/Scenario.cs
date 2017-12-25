using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class Scenario
    {
        [Test, AutoData]
        public void AutoTestCaseProvidesCorrectInteger(int primitiveValue)
        {
            Assert.AreNotEqual(0, primitiveValue);
        }

        [Test, AutoData]
        public void AutoTestCaseProvidesCorrectString(string text)
        {
            Assert.True(text.StartsWith("text"));
        }

        [Test, AutoData]
        public void AutoTestCaseProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [Test, AutoData]
        public void AutoTestCaseProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<OperatingSystem> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [Test, AutoData]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen]Guid g2, Guid g3)
        {
            Assert.AreNotEqual(g1, g2);
            Assert.AreNotEqual(g1, g3);

            Assert.AreEqual(g2, g3);
        }

        [Test, AutoData]
        public void IntroductoryTest(
            int expectedNumber, MyClass sut)
        {
            // Arrange
            // Act
            int result = sut.Echo(expectedNumber);
            // Assert
            Assert.AreEqual(expectedNumber, result);
        }

        [Test, AutoData]
        public void ModestCreatesParameterWithModestConstructor([Modest]MultiUnorderedConstructorType p)
        {
            Assert.True(string.IsNullOrEmpty(p.Text));
            Assert.AreEqual(0, p.Number);
        }

        [Test, AutoData]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy]MultiUnorderedConstructorType p)
        {
            Assert.False(string.IsNullOrEmpty(p.Text));
            Assert.AreNotEqual(0, p.Number);
        }

        [Test, AutoData]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter([Frozen][Greedy]MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
        {
            Assert.False(string.IsNullOrEmpty(p2.Text));
            Assert.AreNotEqual(0, p2.Number);
        }

        [Test, AutoData]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen]int[] numbers, [FavorArrays]ItemContainer<int> container)
        {
            Assert.True(numbers.SequenceEqual(container.Items));
        }

        [TestFixture]
        [Obsolete]
        public class Obsoleted
        {
            [Test, AutoData]
            public void FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType(
#pragma warning disable 0618
                [Frozen(As = typeof(AbstractType))]ConcreteType p1,
#pragma warning restore 0618
                AbstractType p2)
            {
                Assert.AreSame(p1, p2);
            }

            [Test, AutoData]
            public void FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType(
#pragma warning disable 0618
                [Frozen(As = null)]ConcreteType p1,
#pragma warning restore 0618
                ConcreteType p2)
            {
                Assert.AreSame(p1, p2);
            }

#pragma warning disable 618
            [Test, AutoData(typeof(CustomizedFixture))]
#pragma warning restore 618
            public void AutoTestCaseProvidesCustomizedObject(PropertyHolder<string> ph)
            {
                Assert.AreEqual("Ploeh", ph.Property);
            }

            [Test, AutoData]
            public void FreezeFirstParameter([Frozen]Guid g1, Guid g2)
            {
                Assert.AreEqual(g1, g2);
            }
        }

        [Test, AutoData]
        public void FreezeFirstParameterShouldAssignSameInstanceToSecondParameter(
            [Frozen]string p1,
            string p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByExactTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ExactType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByExactTypeShouldNotAssignSameInstanceToSecondParameterOfDifferentType(
            [Frozen(Matching.ExactType)]ConcreteType p1,
            object p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            AbstractType p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfIndirectBaseType(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            object p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByExactOrDirectBaseTypeShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.DirectBaseType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByInterfaceShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            IInterface p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfNonInterfaceType(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            object p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByDirectOrInterfaceShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByParameterWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)]string parameter,
            SingleParameterType<object> p2)
        {
            Assert.AreEqual(parameter, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)]string p1,
            SingleParameterType<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ParameterName)]string p1,
            SingleParameterType<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByPropertyWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)]string property,
            PropertyHolder<object> p2)
        {
            Assert.AreEqual(property, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)]string p1,
            PropertyHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.PropertyName)]string p1,
            PropertyHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByFieldWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)]string field,
            FieldHolder<object> p2)
        {
            Assert.AreEqual(field, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)]string p1,
            FieldHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.FieldName)]string p1,
            FieldHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingParameter(
            [Frozen(Matching.MemberName)]string parameter,
            SingleParameterType<object> p2)
        {
            Assert.AreEqual(parameter, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameter(
            [Frozen(Matching.MemberName)]string p1,
            SingleParameterType<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameterOfSameType(
            [Frozen(Matching.MemberName)]string p1,
            SingleParameterType<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingProperty(
            [Frozen(Matching.MemberName)]string property,
            PropertyHolder<object> p2)
        {
            Assert.AreEqual(property, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToProperty(
            [Frozen(Matching.MemberName)]string p1,
            PropertyHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToPropertyOfSameType(
            [Frozen(Matching.MemberName)]string p1,
            PropertyHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingField(
            [Frozen(Matching.MemberName)]string field,
            FieldHolder<object> p2)
        {
            Assert.AreEqual(field, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToField(
            [Frozen(Matching.MemberName)]string p1,
            FieldHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [Test, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToFieldOfSameType(
            [Frozen(Matching.MemberName)]string p1,
            FieldHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [Theory, AutoData]
        public void FreezeParameterWithStringLengthConstraintShouldCreateConstrainedSpecimen(
            [Frozen, StringLength(3)]string p)
        {
            Assert.True(p.Length == 3);
        }
    }
}
