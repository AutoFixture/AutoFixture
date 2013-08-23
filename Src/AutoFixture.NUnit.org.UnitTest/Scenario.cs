using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org;
using Ploeh.TestTypeFoundation;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    [TestFixture]
    public class Scenario
    {
        [Test, AutoData]
        public void AutoDataProvidesCorrectInteger(int primitiveValue)
        {
            Assert.AreNotEqual(0, primitiveValue);
        }

        [Test, AutoData]
        public void AutoDataProvidesCorrectString(string text)
        {
            Assert.True(text.StartsWith("text"));
        }

        [Test, AutoData]
        public void AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);
        }

        [Test, AutoData]
        public void AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<OperatingSystem> spt)
        {
            Assert.NotNull(ph);
            Assert.NotNull(ph.Property);

            Assert.NotNull(spt);
            Assert.NotNull(spt.Parameter);
        }

        [Test, AutoData(typeof(CustomizedFixture))]
        public void AutoDataProvidesCustomizedObject(PropertyHolder<string> ph)
        {
            Assert.AreEqual("Ploeh", ph.Property);
        }

        [Test, AutoData]
        public void FreezeFirstParameter([Frozen]Guid g1, Guid g2)
        {
            Assert.AreEqual(g1, g2);
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
            // Fixture setup
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.AreEqual(expectedNumber, result);
            // Teardown
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

        [Test, AutoData]
        public void FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType(
            [Frozen(As = typeof(AbstractType))]ConcreteType p1,
            AbstractType p2)
        {
            Assert.ReferenceEquals(p1, p2);
        }

        [Test, AutoData]
        public void FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType(
            [Frozen(As = null)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.ReferenceEquals(p1, p2);
        }
    }
}
