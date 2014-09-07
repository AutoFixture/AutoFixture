using System;
using System.Collections;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class FreezeOnMatchCustomizationTest
    {
        [Fact]
        public void SutShouldBeCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new FreezeOnMatchCustomization<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithoutMatchingStrategyShouldMatchByExactType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<string>();
            var requested = fixture.Create<string>();
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void CustomizeWithNullFixtureShouldThrowArgumentNullException()
        {
            // Fixture setup
            var sut = new FreezeOnMatchCustomization<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingExactTypeShouldReturnSameSpecimenForSameType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByExactType());
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<string>();
            var requested = fixture.Create<string>();
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingBaseTypeShouldReturnSameSpecimenForDirectBaseType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByBaseType());
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ConcreteType>();
            var requested = fixture.Create<AbstractType>();
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingBaseTypeShouldReturnSameSpecimenForSameType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByBaseType());
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ConcreteType>();
            var requested = fixture.Create<ConcreteType>();
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingBaseTypeShouldReturnDifferentSpecimensForIndirectBaseType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByBaseType());
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ConcreteType>();
            var requested = fixture.Create<object>();
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingInterfacesShouldReturnSameSpecimenForImplementedInterfaces()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ArrayList>(
                match => match.ByInterfaces());
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ArrayList>();
            Assert.Same(frozen, fixture.Create<IEnumerable>());
            Assert.Same(frozen, fixture.Create<IList>());
            Assert.Same(frozen, fixture.Create<ICollection>());
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingInterfacesShouldReturnSameSpecimenForSameType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByInterfaces());
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<ConcreteType>();
            var requested = fixture.Create<ConcreteType>();
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertiesOfSameNameAndType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByPropertyName("Property"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<string>>().Property;
            var requested = fixture.Create<PropertyHolder<string>>().Property;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnSameSpecimenForPropertiesOfSameNameAndCompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByPropertyName("Property"));
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
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByPropertyName("SomeOtherProperty"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<string>>().Property;
            var requested = fixture.Create<PropertyHolder<string>>().Property;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingPropertyNameShouldReturnDifferentSpecimensForPropertiesOfSameNameAndIncompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByPropertyName("Property"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<PropertyHolder<string>>().Property;
            var requested = fixture.Create<PropertyHolder<string>>().Property;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnSameSpecimenForParametersOfSameNameAndType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByParameterName("parameter"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<string>>().Parameter;
            var requested = fixture.Create<SingleParameterType<string>>().Parameter;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnSameSpecimenForParametersOfSameNameAndCompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByParameterName("parameter"));
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
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByParameterName("someOtherParameter"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<string>>().Parameter;
            var requested = fixture.Create<SingleParameterType<string>>().Parameter;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingParameterNameShouldReturnDifferentSpecimensForParametersOfSameNameAndIncompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByParameterName("parameter"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<SingleParameterType<string>>().Parameter;
            var requested = fixture.Create<SingleParameterType<string>>().Parameter;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnSameSpecimenForFieldsOfSameNameAndType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByFieldName("Field"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<string>>().Field;
            var requested = fixture.Create<FieldHolder<string>>().Field;
            Assert.Same(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnSameSpecimenForFieldsOfSameNameAndCompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByFieldName("Field"));
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
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByFieldName("SomeOtherField"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<string>>().Field;
            var requested = fixture.Create<FieldHolder<string>>().Field;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingFieldNameShouldReturnDifferentSpecimensForFieldsOfSameNameAndIncompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<ConcreteType>(
                match => match.ByFieldName("Field"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var frozen = fixture.Create<FieldHolder<string>>().Field;
            var requested = fixture.Create<FieldHolder<string>>().Field;
            Assert.NotSame(frozen, requested);
            // Teardown
        }

        [Fact]
        public void FreezeByMatchingMemberNameShouldReturnSameSpecimenForMembersOfMatchingNameAndCompatibleTypes()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new FreezeOnMatchCustomization<string>(
                match => match.ByParameterName("parameter")
                              .Or.ByPropertyName("Property")
                              .Or.ByFieldName("Field"));
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            var parameter = fixture.Create<SingleParameterType<string>>().Parameter;
            var property = fixture.Create<PropertyHolder<string>>().Property;
            var field = fixture.Create<FieldHolder<string>>().Field;
            Assert.Same(parameter, property);
            Assert.Same(property, field);
            // Teardown
        }
    }
}
