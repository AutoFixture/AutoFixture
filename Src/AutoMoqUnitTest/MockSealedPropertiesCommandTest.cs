using System;
using System.Reflection;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    [Obsolete]
    public class MockSealedPropertiesCommandTest
    {
        [Fact]
        public void SetupThrowsWhenMockIsNull()
        {
            // Arrange
            var context = new Mock<ISpecimenContext>();
            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null, context.Object));
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Arrange
            var mock = new Mock<object>();
            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(mock, null));
        }

        [Fact]
        public void InitializesSealedPropertyUsingContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new MockSealedPropertiesCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            Assert.Equal(frozenString, mock.Object.ExplicitlySealedProperty);
            Assert.Equal(frozenString, mock.Object.ImplicitlySealedProperty);
        }

        [Fact]
        public void InitializesPublicFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPublicField>();

            var sut = new MockSealedPropertiesCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            Assert.Equal(frozenString, mock.Object.Field);
        }

        [Fact]
        public void IgnoresGetOnlyProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<TypeWithGetOnlyProperty>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresVirtualProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.VirtualProperty);
        }

        [Fact]
        public void IgnoresPropertiesWithPrivateSetter()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPropertyWithPrivateSetter>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.PropertyWithPrivateSetter);
        }

        [Fact]
        public void IgnoresPrivateProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPrivateProperty>();
            var privateProperty = typeof (TypeWithPrivateProperty)
                .GetProperty("PrivateProperty",
                             BindingFlags.Instance | BindingFlags.NonPublic);

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, privateProperty.GetValue(mock.Object, null));
        }

        [Fact]
        public void IgnoresInterfaceProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.Property);
        }

        [Fact]
        public void IgnoresStaticProperties()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithStaticProperty>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void IgnoresIndexers()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<TypeWithIndexer>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenInt, mock.Object[2]);
        }

        [Fact]
        public void IgnoresPrivateFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPrivateField>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.GetPrivateField());
        }

        [Fact]
        public void IgnoresReadonlyFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithReadonlyField>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.ReadonlyField);
        }

        [Fact]
        public void IgnoresLiteralFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithConstField>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void IgnoresStaticFields()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithStaticField>();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Arrange
            // The context mock has a strict behaviour - if any of its members are invoked, an exception will be thrown
            var context = new Mock<ISpecimenContext>(MockBehavior.Strict);
            var specimen = new TypeWithSealedMembers();

            var sut = new MockSealedPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context.Object)));
        }
    }
}
