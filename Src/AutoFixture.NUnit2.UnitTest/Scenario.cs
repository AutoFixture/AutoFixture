using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class Scenario
    {
        [Test, AutoTestCase]
        public void AutoTestCaseProvidesCorrectInteger(int primitiveValue)
        {
            Assert.AreNotEqual(0, primitiveValue);
        }

        [Test, AutoTestCase]
        public void AutoTestCaseProvidesCorrectString(string text)
        {
            Assert.True(text.StartsWith("text"));
        }

        [Test, AutoTestCase]
        public void AutoTestCaseProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [Test, AutoTestCase]
        public void AutoTestCaseProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<OperatingSystem> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [Test, AutoTestCase(typeof(CustomizedFixture))]
        public void AutoTestCaseProvidesCustomizedObject(PropertyHolder<string> ph)
        {
            Assert.AreEqual("Ploeh", ph.Property);
        }

        [Test, AutoTestCase]
        public void FreezeFirstParameter([Frozen]Guid g1, Guid g2)
        {
            Assert.AreEqual(g1, g2);
        }

        [Test, AutoTestCase]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen]Guid g2, Guid g3)
        {
            Assert.AreNotEqual(g1, g2);
            Assert.AreNotEqual(g1, g3);

            Assert.AreEqual(g2, g3);
        }

        [Test, AutoTestCase]
        public void IntroductoryTest(
            int expectedNumber, MyClass sut)
        {
            // Fixture setup
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.AreEqual(expectedNumber, result);
            // Teardown
        }

        [Test, AutoTestCase]
        public void ModestCreatesParameterWithModestConstructor([Modest]MultiUnorderedConstructorType p)
        {
            Assert.True(string.IsNullOrEmpty(p.Text));
            Assert.AreEqual(0, p.Number);
        }

        [Test, AutoTestCase]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy]MultiUnorderedConstructorType p)
        {
            Assert.False(string.IsNullOrEmpty(p.Text));
            Assert.AreNotEqual(0, p.Number);
        }

        [Test, AutoTestCase]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter([Frozen][Greedy]MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
        {
            Assert.False(string.IsNullOrEmpty(p2.Text));
            Assert.AreNotEqual(0, p2.Number);
        }

        [Test, AutoTestCase]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen]int[] numbers, [FavorArrays]ItemContainer<int> container)
        {
            Assert.True(numbers.SequenceEqual(container.Items));
        }

        [Test, AutoTestCase]
        public void FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType(
            [Frozen(As = typeof(AbstractType))]ConcreteType p1,
            AbstractType p2)
        {
            Assert.AreSame(p1, p2);
        }

        [Test, AutoTestCase]
        public void FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType(
            [Frozen(As = null)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreSame(p1, p2);
        }
    }
}
