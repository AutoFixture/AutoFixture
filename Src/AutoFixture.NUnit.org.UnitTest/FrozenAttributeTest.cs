using System;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit.org;
using Ploeh.TestTypeFoundation;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    [TestFixture]
    public class FrozenAttributeTest
    {
        [Test]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FrozenAttribute();
            // Verify outcome
            Assert.IsInstanceOf<CustomizeAttribute>(sut);
            // Teardown
        }

        [Test]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Test]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOf<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.AreSame(parameter.ParameterType, freezer.TargetType);
            // Teardown
        }

        [Test]
        public void GetCustomizationReturnsTheRegisteredTypeEqualToTheParameterType()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOf<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.AreSame(parameter.ParameterType, freezer.RegisteredType);
            // Teardown
        }

        [Test]
        public void GetCustomizationWithSpecificRegisteredTypeReturnsCorrectResult()
        {
            // Fixture setup
            var registeredType = typeof(AbstractType);
            var sut = new FrozenAttribute { As = registeredType };
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.AreSame(registeredType, freezer.RegisteredType);
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
