using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.Xunit.UnitTest
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
        public void InitializeShouldSetDefaultMatchingStrategy()
        {
            // Fixture setup
            // Exercise system
            var sut = new FrozenAttribute();
            // Verify outcome
            Assert.Equal(Matching.ExactType, sut.By);
            // Teardown
        }

        [Fact]
        public void InitializeShouldSetDefaultTargetName()
        {
            // Fixture setup
            // Exercise system
            var sut = new FrozenAttribute();
            // Verify outcome
            Assert.Null(sut.TargetName);
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
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(parameter.ParameterType, freezer.TargetType);
            // Teardown
        }

        [Fact]
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
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(parameter.ParameterType, freezer.RegisteredType);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithSpecificTypeShouldReturnCorrectResult()
        {
            // Fixture setup
            var registeredType = typeof(AbstractType);
            var sut = new FrozenAttribute { As = registeredType };
            var parameter = AParameter<ConcreteType>();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezingCustomization>(result);
            Assert.Equal(registeredType, freezer.RegisteredType);
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithIncompatibleSpecificTypeThrowsArgumentException()
        {
            // Fixture setup
            var registeredType = typeof(string);
            var sut = new FrozenAttribute { As = registeredType };
            var parameter = AParameter<ConcreteType>();
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
            var sut = new FrozenAttribute { By = Matching.DirectBaseType };
            // Exercise system
            var customization = sut.GetCustomization(AParameter<object>());
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            Assert.NotEmpty(matcher.Specifications.OfType<DirectBaseTypeSpecification>());
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingByImplementedInterfacesShouldMatchByImplementedInterfaces()
        {
            // Fixture setup
            var sut = new FrozenAttribute { By = Matching.ImplementedInterfaces };
            // Exercise system
            var customization = sut.GetCustomization(AParameter<object>());
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            Assert.NotEmpty(matcher.Specifications.OfType<ImplementedInterfaceSpecification>());
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingByParameterNameShouldMatchByParameter()
        {
            // Fixture setup
            var sut = new FrozenAttribute { By = Matching.ParameterName, TargetName = "parameter" };
            // Exercise system
            var customization = sut.GetCustomization(AParameter<object>());
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            Assert.NotEmpty(matcher.Specifications.OfType<ParameterSpecification>());
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingByPropertyNameShouldMatchByProperty()
        {
            // Fixture setup
            var sut = new FrozenAttribute { By = Matching.PropertyName, TargetName = "Property" };
            // Exercise system
            var customization = sut.GetCustomization(AParameter<object>());
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            Assert.NotEmpty(matcher.Specifications.OfType<PropertySpecification>());
            // Teardown
        }

        [Fact]
        public void GetCustomizationWithMatchingByFieldNameShouldMatchByField()
        {
            // Fixture setup
            var sut = new FrozenAttribute { By = Matching.FieldName, TargetName = "Field" };
            // Exercise system
            var customization = sut.GetCustomization(AParameter<object>());
            // Verify outcome
            var freezer = Assert.IsAssignableFrom<FreezeOnMatchCustomization>(customization);
            var matcher = Assert.IsType<OrRequestSpecification>(freezer.Matcher);
            Assert.NotEmpty(matcher.Specifications.OfType<FieldSpecification>());
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
