using System;
using System.Reflection;
using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteSealedPropertiesCommandTest
    {
        [Fact]
        public void SetupThrowsWhenSubstituteIsNull()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null, context));
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Arrange
            var substitute = Substitute.For<object>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(substitute, null));
        }

        [Fact]
        public void InitializesSealedPropertyUsingContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithSealedMembers>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            Assert.Equal(frozenString, substitute.ExplicitlySealedProperty);
            Assert.Equal(frozenString, substitute.ImplicitlySealedProperty);
        }

        [Fact]
        public void InitializesPublicFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPublicField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            Assert.Equal(frozenString, substitute.Field);
        }

        [Fact]
        public void IgnoresGetOnlyProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithGetOnlyProperty>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.GetOnlyProperty);
        }

        [Fact]
        public void IgnoresVirtualProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithVirtualMembers>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.VirtualProperty);
        }

        [Fact]
        public void IgnoresPropertiesWithPrivateSetter()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPropertyWithPrivateSetter>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.PropertyWithPrivateSetter);
        }

        [Fact]
        public void IgnoresPrivateProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPrivateProperty>();
            var privateProperty = typeof(TypeWithPrivateProperty).GetProperty("PrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic);

            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, privateProperty.GetValue(substitute, null));
        }

        [Fact]
        public void IgnoresInterfaceProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithProperty>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.Property);
        }

        [Fact]
        public void IgnoresStaticProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithStaticProperty>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void IgnoresIndexers()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<TypeWithIndexer>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenInt, substitute[2]);
        }

        [Fact]
        public void IgnoresPrivateFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPrivateField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.GetPrivateField());
        }

        [Fact]
        public void IgnoresReadonlyFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithReadonlyField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.ReadonlyField);
        }

        [Fact]
        public void IgnoresLiteralFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithConstField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void IgnoresStaticFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithStaticField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void IgnoresNonSubstituteSpecimens()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            var specimen = new ConcreteTypeWithSealedMembers();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));

            context.DidNotReceiveWithAnyArgs().Resolve(null);
        }
    }
}
