using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    public class FrozenAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FrozenAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithSpecificRegisteredTypeReturnsCorrectResult()
        {
            // Fixture setup
            var registeredType = typeof(AbstractType);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(registeredType, freezer.RegisteredType);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithIncompatibleRegisteredTypeThrowsArgumentException()
        {
            // Fixture setup
            var registeredType = typeof(string);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => sut.GetCustomization(parameter));
            // Teardown
        }

        [Fact]
        public void GetCustomizationShouldMatchByExactParameterType()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute();
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            var exactTypeMatcher = matcher.Specifications.OfType<ExactTypeSpecification>().SingleOrDefault();
            Assert.NotNull(exactTypeMatcher);
            Assert.Equal(parameter.ParameterType, exactTypeMatcher.TargetType);
            // Teardown
        }

        [Fact]
        public void GetCustomizationShouldMatchBySeedRequestForParameterType()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute();
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            var seedRequestMatcher = matcher.Specifications.OfType<SeedRequestSpecification>().SingleOrDefault();
            Assert.NotNull(seedRequestMatcher);
            Assert.Equal(parameter.ParameterType, seedRequestMatcher.TargetType);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingByDirectBaseTypeShouldMatchByBaseType()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute(Matching.DirectBaseType);
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            var directBaseTypeMatcher = matcher.Specifications.OfType<DirectBaseTypeSpecification>().SingleOrDefault();
            Assert.NotNull(directBaseTypeMatcher);
            Assert.Equal(parameter.ParameterType, directBaseTypeMatcher.TargetType);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingByImplementedInterfacesShouldMatchByImplementedInterfaces()
        {
            // Fixture setup
            var parameter = AParameter<object>();
            var sut = new FrozenAttribute(Matching.ImplementedInterfaces);
            // Exercise system
            var customization = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            var interfaceTypeMatcher = matcher.Specifications.OfType<ImplementedInterfaceSpecification>().SingleOrDefault();
            Assert.NotNull(interfaceTypeMatcher);
            Assert.Equal(parameter.ParameterType, interfaceTypeMatcher.TargetType);
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
