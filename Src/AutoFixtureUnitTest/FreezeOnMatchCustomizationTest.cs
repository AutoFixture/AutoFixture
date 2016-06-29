using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class FreezeOnMatchCustomizationTest
    {
        [Fact]
        public void SutShouldBeCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTargetTypeShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization((Type)null, new FalseRequestSpecification()));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullMatcherShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization(typeof(object), null));
            // Teardown
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var targetType = typeof(object);
            // Exercise system
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Verify outcome
            Assert.Equal(targetType, sut.TargetType);
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetMatcherToMatchThatExactType()
        {
            // Fixture setup
            var targetType = typeof(object);
            // Exercise system
            var sut = new FreezeOnMatchCustomization(targetType);
            // Verify outcome
            var matcher = Assert.IsType<ExactTypeSpecification>(sut.Matcher);
            Assert.Equal(targetType, matcher.TargetType);
        }

        [Fact]
        public void InitializeWithMatcherShouldSetCorrespondingProperty()
        {
            // Fixture setup
            var matcher = new TrueRequestSpecification();
            // Exercise system
            var sut = new FreezeOnMatchCustomization(typeof(object), matcher);
            // Verify outcome
            Assert.Equal(matcher, sut.Matcher);
        }

        [Fact]
        public void InitializeWithNullParameterInfoShouldThrowArgumentNullException()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization((ParameterInfo)null, new FalseRequestSpecification()));
            // Teardown
        }

        [Fact]
        public void InitializeWithParameterInfoAndNullMatcherShouldThrowArgumentNullException()
        {
            // Fixture setup
            var parameter = typeof(UnguardedMethodHost).GetMethod("ConsumeUnguardedString").GetParameters().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization(parameter, null));
            // Teardown
        }

        [Fact]
        public void InitializeWithParameterInfoAndMatcherShouldSetCorrespondingProperties()
        {
            // Fixture setup
            var parameter = typeof(UnguardedMethodHost).GetMethod("ConsumeUnguardedString").GetParameters().First();
            var matcher = new TrueRequestSpecification();
            // Exercise system
            var sut = new FreezeOnMatchCustomization(parameter, matcher);
            // Verify outcome
            Assert.Equal(parameter, sut.ParameterInfo);
            Assert.Equal(matcher, sut.Matcher);
        }

        [Fact]
        public void InitializeWithParameterInfoShouldSetTargetTypeProperty()
        {
            // Fixture setup
            var parameter = typeof(UnguardedMethodHost).GetMethod("ConsumeUnguardedString").GetParameters().First();
            var matcher = new TrueRequestSpecification();
            var expected = typeof(string);
            // Exercise system
            var sut = new FreezeOnMatchCustomization(parameter, matcher);
            // Verify outcome
            Assert.Equal(expected, sut.TargetType);
        }

        [Fact]
        public void CustomizeWithNullFixtureShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void CustomizeWithCompetingSpecimenBuilderForTheSameTypeShouldReturnTheFrozenSpecimen()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var frozenType = typeof(object);
            var competingBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (request, ctx) =>
                    request.Equals(frozenType)
                        ? new object()
#pragma warning disable 618
                        : new NoSpecimen(request)
#pragma warning restore 618
            };
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ExactTypeSpecification(frozenType));
            // Exercise system
            fixture.Customizations.Add(competingBuilder);
            sut.Customize(fixture);
            // Verify outcome
            Assert.Equal(context.Resolve(frozenType), context.Resolve(frozenType));
        }

        [Theory]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(int), typeof(int), true)]
        [InlineData(typeof(object), typeof(bool), false)]
        [InlineData(typeof(int), typeof(string), false)]
        public void FreezeByMatchingExactTypeShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType,
            bool areEqual)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ExactTypeSpecification(frozenType));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(areEqual, frozen.Equals(requested));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ConcreteType), typeof(ConcreteType), true)]
        [InlineData(typeof(ConcreteType), typeof(AbstractType), true)]
        [InlineData(typeof(string), typeof(object), true)]
        [InlineData(typeof(ConcreteType), typeof(object), false)]
        [InlineData(typeof(int), typeof(string), false)]
        public void FreezeByMatchingBaseTypeShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType,
            bool areEqual)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new DirectBaseTypeSpecification(frozenType));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(areEqual, frozen.Equals(requested));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ConcreteType), typeof(ConcreteType))]
        [InlineData(typeof(ArrayList), typeof(IEnumerable))]
        [InlineData(typeof(DelegatingEqualityComparer<object>), typeof(IEqualityComparer<object>))]
        public void FreezeByMatchingImplementedInterfacesShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType)
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ImplementedInterfaceSpecification(frozenType));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertiesOfSameNameAndType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertiesOfSameNameAndCompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<AbstractType>>().Property;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnDifferentSpecimensForPropertiesOfDifferentNamesAndSameType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var propertyName = "SomeOtherProperty";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnDifferentSpecimensForPropertiesOfSameNameAndIncompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(string);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnSameSpecimenForParametersOfSameNameAndType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var parameterName = "parameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnSameSpecimenForParametersOfSameNameAndCompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var parameterName = "parameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<AbstractType>>().Parameter;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnDifferentSpecimensForParametersOfDifferentNamesAndSameType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var parameterName = "SomeOtherParameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnDifferentSpecimensForParametersOfSameNameAndIncompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(string);
            var parameterName = "parameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnSameSpecimenForFieldsOfSameNameAndType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var fieldName = "Field";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnSameSpecimenForFieldsOfSameNameAndCompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var fieldName = "Field";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<AbstractType>>().Field;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnDifferentSpecimensForFieldsOfDifferentNamesAndSameType()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var fieldName = "SomeOtherField";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnDifferentSpecimensForFieldsOfSameNameAndIncompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(string);
            var fieldName = "Field";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingMemberNameShouldReturnSameSpecimenForMembersOfMatchingNameAndCompatibleTypes()
        {
            // Fixture setup
            var frozenType = typeof(ConcreteType);
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new OrRequestSpecification(
                    new ParameterSpecification(frozenType, "parameter"),
                    new PropertySpecification(frozenType, "Property"),
                    new FieldSpecification(frozenType, "Field")));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var parameter = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var property = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var field = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.Same(parameter, property);
            Assert.Same(property, field);
            // Teardown
        }
    }
}
