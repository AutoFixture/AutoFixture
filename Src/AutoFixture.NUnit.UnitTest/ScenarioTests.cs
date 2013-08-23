using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    internal class ScenarioTests
    {
        [AutoTestCase(typeof(ScenarioTests),"AutoDataProvidesCorrectInteger")]
        public void AutoDataProvidesCorrectInteger(int primitiveValue)
        {
            Assert.That(primitiveValue, Is.Not.EqualTo(0));
        }

        [AutoTestCase(typeof(ScenarioTests), "AutoDataProvidesCorrectString")]
        public void AutoDataProvidesCorrectString(string text)
        {
            Assert.True(text.StartsWith("text"));
        }

        [AutoTestCase(typeof(ScenarioTests), "AutoDataProvidesCorrectObject")]
        public void AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [AutoTestCase(typeof(ScenarioTests), "AutoDataProvidesMultipleObjects")]
        public void AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, 
            SingleParameterType<OperatingSystem> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [AutoTestCase(typeof(ScenarioTests), "AutoDataProvidesCustomizedObject", typeof(CustomizedFixture))]
        public void AutoDataProvidesCustomizedObject(PropertyHolder<string> ph)
        {
            Assert.AreEqual(ph.Property, "Ploeh");
        }

        [AutoTestCase(typeof(ScenarioTests), "AutoTestCaseUsesSuppliedDataValues", "foo")]
        [AutoTestCase(typeof(ScenarioTests), "AutoTestCaseUsesSuppliedDataValues", "foo", "bar")]
        public void AutoTestCaseUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.That(s1, Is.EqualTo("foo"));
            Assert.NotNull(s2);
        }

        [AutoTestCase(typeof(ScenarioTests), "AutoTestCaseSupplyDataSpecimensForNonProvidedValues", "foo")]
        [AutoTestCase(typeof(ScenarioTests), "AutoTestCaseSupplyDataSpecimensForNonProvidedValues", "foo", "bar")]
        public void AutoTestCaseSupplyDataSpecimensForNonProvidedValues(string s1, string s2, MyTestClass myClass)
        {
            Assert.AreEqual(s1,"foo");
            Assert.NotNull(s2);
            Assert.NotNull(myClass);
        }

        [AutoTestCase(
            typeof(ScenarioTests),
            "AutoTestCaseSupplyDataSpecimensOnlyForNonProvidedValues", 
            "foo")]
        [AutoTestCase(
            typeof(ScenarioTests),
            "AutoTestCaseSupplyDataSpecimensOnlyForNonProvidedValues", 
            "foo", 
            "bar")]
        public void AutoTestCaseSupplyDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.AreEqual(s1, "foo");
            Assert.That(s2, Is.Not.Null);
            Assert.AreNotEqual(s3, "foo");
            Assert.AreNotEqual(s3, "bar");
        }

        [AutoTestCase(typeof(ScenarioTests), "FreezeFirstParameter")]
        public void FreezeFirstParameter([Frozen]Guid g1, Guid g2)
        {
            Assert.AreEqual(g1, g2);
        }

        [AutoTestCase(typeof(ScenarioTests), "FreezeSecondParameterOnlyFreezesSubsequentParameters")]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen]Guid g2, Guid g3)
        {
            Assert.AreNotEqual(g1,g2);
            Assert.AreNotEqual(g1,g3);

            Assert.AreEqual(g2,g3);
        }

        [AutoTestCase(typeof(ScenarioTests), "IntroductoryTest")]
        public void IntroductoryTest(
            int expectedNumber, MyTestClass sut)
        {
            // Fixture setup
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.That(expectedNumber, Is.EqualTo(result));
            // Teardown
        }

        [AutoTestCase(typeof(ScenarioTests), "ModestCreatesParameterWithModestConstructor")]
        public void ModestCreatesParameterWithModestConstructor([Modest]MultiUnorderedConstructorType p)
        {
            Assert.True(string.IsNullOrEmpty(p.Text));
            Assert.That(p.Number, Is.EqualTo(0));
        }

        [AutoTestCase(typeof(ScenarioTests), "GreedyCreatesParameterWithGreedyConstructor")]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy]MultiUnorderedConstructorType p)
        {
            Assert.False(string.IsNullOrEmpty(p.Text));
            Assert.AreNotEqual(p.Number, 0);
        }

        [AutoTestCase(typeof(ScenarioTests), "BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter")]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter(
            [Frozen][Greedy]MultiUnorderedConstructorType p1, 
            MultiUnorderedConstructorType p2)
        {
            Assert.False(string.IsNullOrEmpty(p2.Text));
            Assert.AreNotEqual(p2.Number, 0);
        }

        [AutoTestCase(typeof(ScenarioTests), "FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems")]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems(
            [Frozen]int[] numbers, 
            [FavorArrays]ItemContainer<int> container)
        {
            Assert.True(numbers.SequenceEqual(container.Items));
        }

        [AutoTestCase(typeof(ScenarioTests), 
            "FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType")]
        public void FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType(
            [Frozen(As = typeof(AbstractType))]ConcreteType p1,
            AbstractType p2)
        {
            Assert.AreSame(p1, p2);
        }

        [AutoTestCase(typeof(ScenarioTests), 
            "FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType")]
        public void FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType(
            [Frozen(As = null)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreSame(p1, p2);
        }
    }
}