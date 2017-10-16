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
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null, context));
            // Teardown
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Fixture setup
            var substitute = Substitute.For<object>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(substitute, null));
            // Teardown
        }

        [Fact]
        public void InitializesSealedPropertyUsingContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithSealedMembers>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            Assert.Equal(frozenString, substitute.ExplicitlySealedProperty);
            Assert.Equal(frozenString, substitute.ImplicitlySealedProperty);
            // Teardown
        }

        [Fact]
        public void InitializesPublicFields()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPublicField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            Assert.Equal(frozenString, substitute.Field);
            // Teardown
        }

        [Fact]
        public void IgnoresGetOnlyProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithGetOnlyProperty>();
            var sut = new NSubstituteSealedPropertiesCommand();
            //Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.GetOnlyProperty);
        }

        [Fact]
        public void IgnoresVirtualProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithVirtualMembers>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.VirtualProperty);
        }

        [Fact]
        public void IgnoresPropertiesWithPrivateSetter()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPropertyWithPrivateSetter>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.PropertyWithPrivateSetter);
        }

        [Fact]
        public void IgnoresPrivateProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPrivateProperty>();
            var privateProperty = typeof(TypeWithPrivateProperty).GetProperty("PrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic);

            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, privateProperty.GetValue(substitute, null));
        }

        [Fact]
        public void IgnoresInterfaceProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithProperty>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.Property);
        }

        [Fact]
        public void IgnoresStaticProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithStaticProperty>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void IgnoresIndexers()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<TypeWithIndexer>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenInt, substitute[2]);
        }

        [Fact]
        public void IgnoresPrivateFields()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithPrivateField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.GetPrivateField());
        }

        [Fact]
        public void IgnoresReadonlyFields()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithReadonlyField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.ReadonlyField);
        }

        [Fact]
        public void IgnoresLiteralFields()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithConstField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void IgnoresStaticFields()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithStaticField>();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void IgnoresNonSubstituteSpecimens()
        {
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var specimen = new ConcreteTypeWithSealedMembers();
            var sut = new NSubstituteSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));

            context.DidNotReceiveWithAnyArgs().Resolve(null);
        }
    }
}
