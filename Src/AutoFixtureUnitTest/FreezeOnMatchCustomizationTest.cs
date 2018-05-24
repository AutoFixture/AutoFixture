using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class FreezeOnMatchCustomizationTest
    {
        [Fact]
        public void SutShouldBeCustomization()
        {
            // Arrange
            // Act
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void InitializeWithNullTargetTypeShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization(null, new FalseRequestSpecification()));
        }

        [Fact]
        public void InitializeWithNullMatcherShouldThrowArgumentNullException()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new FreezeOnMatchCustomization(typeof(object), null));
        }

        [Fact]
        public void InitializeWithTargetTypeShouldSetCorrespondingProperty()
        {
            // Arrange
            var targetType = typeof(object);
            // Act
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Assert
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
            // Arrange
            var matcher = new TrueRequestSpecification();
            // Act
            var sut = new FreezeOnMatchCustomization(typeof(object), matcher);
            // Assert
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
            // Arrange
            var sut = new FreezeOnMatchCustomization(typeof(object));
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeWithCompetingSpecimenBuilderForTheSameTypeShouldReturnTheFrozenSpecimen()
        {
            // Arrange
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var frozenType = typeof(object);
            var competingBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (request, ctx) =>
                    request.Equals(frozenType)
                        ? new object()
                        : new NoSpecimen()
#pragma warning restore 618
            };
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ExactTypeSpecification(frozenType));
            // Act
            fixture.Customizations.Add(competingBuilder);
            sut.Customize(fixture);
            // Assert
            Assert.Equal(context.Resolve(frozenType), context.Resolve(frozenType));
        }

        [Fact]
        public void CustomizeWithCompetingSpecimenBuilderForTheSameRequestShouldReturnTheFrozenSpecimen()
        {
            // Arrange
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
            // Act
            fixture.Customizations.Add(competingBuilder);
            sut.Customize(fixture);
            // Assert
            Assert.Equal(context.Resolve(requestType), context.Resolve(requestType));
        }

        [Fact]
        public void CustomizeWithEqualRequestsShouldFreezeSpecimen()
        {
            // Arrange
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);

            var freezingRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property1));
            var equalRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property1));

            var sut = new FreezeOnMatchCustomization(freezingRequest);
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = context.Resolve(freezingRequest);
            var requested = context.Resolve(equalRequest);
            Assert.True(frozen.Equals(requested));
        }

        [Fact]
        public void CustomizeWithNotEqualRequestsShouldNotFreezeSpecimen()
        {
            // Arrange
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);

            var freezingRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property1));
            var anotherRequest = typeof(ConcreteType).GetProperty(
                nameof(ConcreteType.Property2));

            var sut = new FreezeOnMatchCustomization(freezingRequest);
            // Act
            sut.Customize(fixture);
            // Assert
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
            // Arrange
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ExactTypeSpecification(frozenType));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(areEqual, frozen.Equals(requested));
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
            // Arrange
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new DirectBaseTypeSpecification(frozenType));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Equal(areEqual, frozen.Equals(requested));
        }

        [Theory]
        [InlineData(typeof(ConcreteType), typeof(ConcreteType))]
        [InlineData(typeof(ArrayList), typeof(IEnumerable))]
        [InlineData(typeof(DelegatingEqualityComparer<object>), typeof(IEqualityComparer<object>))]
        public void FreezeByMatchingImplementedInterfacesShouldReturnTheRightSpecimen(
            Type frozenType,
            Type requestedType)
        {
            // Arrange
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ImplementedInterfaceSpecification(frozenType));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = context.Resolve(frozenType);
            var requested = context.Resolve(requestedType);
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertiesOfSameNameAndType()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertiesOfSameNameAndCompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<AbstractType>>().Property;
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnDifferentSpecimensForPropertiesOfDifferentNamesAndSameType()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var propertyName = "SomeOtherProperty";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.NotSame(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnDifferentSpecimensForPropertiesOfSameNameAndIncompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(string);
            var propertyName = "Property";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new PropertySpecification(frozenType, propertyName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var requested = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            Assert.NotSame(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnSameSpecimenForParametersOfSameNameAndType()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var parameterName = "parameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnSameSpecimenForParametersOfSameNameAndCompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var parameterName = "parameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<AbstractType>>().Parameter;
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnDifferentSpecimensForParametersOfDifferentNamesAndSameType()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var parameterName = "SomeOtherParameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            Assert.NotSame(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnDifferentSpecimensForParametersOfSameNameAndIncompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(string);
            var parameterName = "parameter";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new ParameterSpecification(frozenType, parameterName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var requested = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            Assert.NotSame(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnSameSpecimenForFieldsOfSameNameAndType()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var fieldName = "Field";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnSameSpecimenForFieldsOfSameNameAndCompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var fieldName = "Field";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<AbstractType>>().Field;
            Assert.Same(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnDifferentSpecimensForFieldsOfDifferentNamesAndSameType()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var fieldName = "SomeOtherField";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.NotSame(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnDifferentSpecimensForFieldsOfSameNameAndIncompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(string);
            var fieldName = "Field";
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new FieldSpecification(frozenType, fieldName));
            // Act
            sut.Customize(fixture);
            // Assert
            var frozen = fixture.Create<FieldHolder<ConcreteType>>().Field;
            var requested = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.NotSame(frozen, requested);
        }

        [Fact]
        public void FreezeByMatchingMemberNameShouldReturnSameSpecimenForMembersOfMatchingNameAndCompatibleTypes()
        {
            // Arrange
            var frozenType = typeof(ConcreteType);
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization(
                frozenType,
                new OrRequestSpecification(
                    new ParameterSpecification(frozenType, "parameter"),
                    new PropertySpecification(frozenType, "Property"),
                    new FieldSpecification(frozenType, "Field")));
            // Act
            sut.Customize(fixture);
            // Assert
            var parameter = fixture.Create<SingleParameterType<ConcreteType>>().Parameter;
            var property = fixture.Create<PropertyHolder<ConcreteType>>().Property;
            var field = fixture.Create<FieldHolder<ConcreteType>>().Field;
            Assert.Same(parameter, property);
            Assert.Same(property, field);
        }
    }
}
