using System;
using System.Collections;
using System.Collections.Generic;
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
                new FreezeOnMatchCustomization(null, new FalseRequestSpecification()));
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
#pragma warning disable 618
            Assert.Equal(targetType, sut.TargetType);
#pragma warning restore 618
        }

        [Fact]
        public void InitializeWithTargetTypeAndMatcherShouldSetCorrespondingProperty()
        {
            var targetType = typeof(object);
            var matcher = new TrueRequestSpecification();
            var sut = new FreezeOnMatchCustomization(typeof(object), matcher);
#pragma warning disable 618
            Assert.Equal(targetType, sut.TargetType);
#pragma warning restore 618
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
        public void InitializeWithSingleNullRequestArgumentShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization((object)null));
        }

        [Fact]
        public void InitializeWithNullRequestShouldThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization((object)null, new FalseRequestSpecification()));
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public void InitializeWithRequestAndNullMatcherShouldThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                  new FreezeOnMatchCustomization(new object(), null));
            Assert.Equal("matcher", ex.ParamName);
        }

        [Fact]
        public void InitializeWithSingleRequestShouldSetCorrespondingProperty()
        {
            var request = new object();
            var sut = new FreezeOnMatchCustomization(request);
            Assert.Same(request, sut.Request);
        }

        [Fact]
        public void InitializeWithObjectRequestShouldSetEqualRequestSpecificationMatcher()
        {
            var request = new object();
            var sut = new FreezeOnMatchCustomization(request);
            Assert.IsAssignableFrom<EqualRequestSpecification>(sut.Matcher);
        }

        [Fact]
        public void InitializeWithRequestShouldSetCorrespondingProperty()
        {
            var request = new object();
            var matcher = new TrueRequestSpecification();
            var sut = new FreezeOnMatchCustomization(request, matcher);
            Assert.Same(request, sut.Request);
        }

        [Fact]
        public void InitializeWithRequestAndMatcherShouldSetCorrespondingProperty()
        {
            var matcher = new TrueRequestSpecification();
            var sut = new FreezeOnMatchCustomization(new object(), matcher);
            Assert.Same(matcher, sut.Matcher);
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

        [Fact]
        public void CustomizeWithCompetingSpecimenBuilderForTheSameRequestShouldReturnTheFrozenSpecimen()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var request = new object();
            var requestType = typeof(object);
            var competingBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (req, ctx) =>
                    req.Equals(request)
                        ? new object()
                        : new NoSpecimen()
            };
            var sut = new FreezeOnMatchCustomization(
                request,
                new ExactTypeSpecification(requestType));
            // Exercise system
            fixture.Customizations.Add(competingBuilder);
            sut.Customize(fixture);
            // Verify outcome
            Assert.Equal(context.Resolve(requestType), context.Resolve(requestType));
        }

        [Fact]
        public void CustomizeWithEqualRequestsShouldFreezeSpecimen()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);

            var freezingRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property1));
            var equalRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property1));

            var sut = new FreezeOnMatchCustomization(freezingRequest);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(freezingRequest);
            var requested = context.Resolve(equalRequest);
            Assert.True(frozen.Equals(requested));
        }

        [Fact]
        public void CustomizeWithNotEqualRequestsShouldNotFreezeSpecimen()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);

            var freezingRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property1));
            var anotherRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property2));

            var sut = new FreezeOnMatchCustomization(freezingRequest);
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = context.Resolve(freezingRequest);
            var requested = context.Resolve(anotherRequest);
            Assert.False(frozen.Equals(requested));
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
