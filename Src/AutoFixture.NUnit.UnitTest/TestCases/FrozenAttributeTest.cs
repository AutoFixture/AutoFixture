using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class FrozenAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            var sut = new FrozenAttribute();

            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void GetCustomizationFromNullParamterThrows()
        {
            var sut = new FrozenAttribute();

            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            var sut = new FrozenAttribute();
            var parameter = typeof(TypeWithMembers)
                .GetMethod("DoSomething")
                .GetParameters()
                .Single();

            var result = sut.GetCustomization(parameter);

            Assert.IsInstanceOf<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.That(parameter.ParameterType, Is.EqualTo(freezer.TargetType));
        }

        [Test]
        public void GetCustomizationReturnsTheRegisteredTypeEqualToTheParameterType()
        {
            var sut = new FrozenAttribute();
            var parameter = typeof(TypeWithMembers)
                .GetMethod("DoSomething", new[] { typeof(object) })
                .GetParameters()
                .Single();
            
            var result = sut.GetCustomization(parameter);
            
            Assert.IsInstanceOf<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.That(parameter.ParameterType, Is.EqualTo(freezer.RegisteredType));
        }

        [Test]
        public void GetCustomizationWithSpecificRegisteredTypeReturnsCorrectResult()
        {
            var registeredType = typeof(AbstractType);
            var sut = new FrozenAttribute { As = registeredType };
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOf<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.That(registeredType, Is.EqualTo(freezer.RegisteredType));
            // Teardown
        }

        [Test]
        public void GetCustomizationWithIncompatibleRegisteredTypeThrowsArgumentException()
        {
            // Fixture setup
            var registeredType = typeof(string);
            var sut = new FrozenAttribute { As = registeredType };
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => sut.GetCustomization(parameter));
            // Teardown
        }
    }
}
