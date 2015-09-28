using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
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
            Assert.IsAssignableFrom<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.AreEqual(parameter.ParameterType, freezer.TargetType);
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
            Assert.IsAssignableFrom<FreezingCustomization>(result);
            var freezer = (FreezingCustomization)result;
            Assert.AreEqual(parameter.ParameterType, freezer.RegisteredType);
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
            Assert.AreEqual(registeredType, freezer.RegisteredType);
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

        [Test]
        public void GetCustomizationShouldMatchByExactParameterType()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute();
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var freezer = (FreezeOnMatchCustomization)customization;
            Assert.IsInstanceOf<OrRequestSpecification>(freezer.Matcher);
            var matcher = (OrRequestSpecification)freezer.Matcher;
            var exactTypeMatcher = matcher.Specifications.OfType<ExactTypeSpecification>().SingleOrDefault();
            Assert.NotNull(exactTypeMatcher);
            Assert.AreEqual(parameter.ParameterType, exactTypeMatcher.TargetType);
            // Teardown
        }

        [Test]
        public void GetCustomizationShouldMatchBySeedRequestForParameterType()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute();
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var freezer = (FreezeOnMatchCustomization)customization;
            Assert.IsInstanceOf<OrRequestSpecification>(freezer.Matcher);
            var matcher = (OrRequestSpecification)freezer.Matcher;
            var seedRequestMatcher = matcher.Specifications.OfType<SeedRequestSpecification>().SingleOrDefault();
            Assert.NotNull(seedRequestMatcher);
            Assert.AreEqual(parameter.ParameterType, seedRequestMatcher.TargetType);
            // Teardown
        }

        [Test]
        public void GetCustomizationWithMatchingByDirectBaseTypeShouldMatchByBaseType()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute(Matching.DirectBaseType);
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var freezer = (FreezeOnMatchCustomization)customization;
            Assert.IsInstanceOf<OrRequestSpecification>(freezer.Matcher);
            var matcher = (OrRequestSpecification)freezer.Matcher;
            var directBaseTypeMatcher = matcher.Specifications.OfType<DirectBaseTypeSpecification>().SingleOrDefault();
            Assert.NotNull(directBaseTypeMatcher);
            Assert.AreEqual(parameter.ParameterType, directBaseTypeMatcher.TargetType);
            // Teardown
        }

        [Test]
        public void GetCustomizationWithMatchingByImplementedInterfacesShouldMatchByImplementedInterfaces()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute(Matching.ImplementedInterfaces);
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var freezer = (FreezeOnMatchCustomization)customization;
            Assert.IsInstanceOf<OrRequestSpecification>(freezer.Matcher);
            var matcher = (OrRequestSpecification)freezer.Matcher;
            var interfaceTypeMatcher = matcher.Specifications.OfType<ImplementedInterfaceSpecification>().SingleOrDefault();
            Assert.NotNull(interfaceTypeMatcher);
            Assert.AreEqual(parameter.ParameterType, interfaceTypeMatcher.TargetType);
            // Teardown
        }

        private static ParameterInfo AParameter<T>()
        {
            return typeof(SingleParameterType<T>)
                .GetConstructor(new[] { typeof(T) })
                .GetParameters()
                .Single(p => p.Name == "parameter");
        }
    }
}
