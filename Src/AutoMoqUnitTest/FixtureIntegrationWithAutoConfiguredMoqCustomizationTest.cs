using AutoFixture.AutoMoq.UnitTest.TestTypes;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class FixtureIntegrationWithAutoConfiguredMoqCustomizationTest
    {
        [Fact]
        public void ParameterlessMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithParameterlessMethod>();
            // Assert
            Assert.Same(frozenString, result.Method());
        }

        [Fact]
        public void MethodsFromBaseInterfacesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IDerivedInterface>();
            // Assert
            Assert.Same(frozenString, result.Method());
        }

        [Fact]
        public void InterfaceNewMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Assert
            Assert.Same(frozenString, result.Method(0));
        }

        [Fact]
        public void InterfaceShadowedMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Assert
            Assert.Same(frozenString, (result as IInterfaceWithShadowedMethod).Method(0));
        }

        [Fact]
        public void PropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Same(frozenString, result.Property);
        }

        [Fact]
        public void GetOnlyPropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithGetOnlyProperty>();
            // Assert
            Assert.Same(frozenString, result.GetOnlyProperty);
        }

        [Fact]
        public void IndexersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Assert
            Assert.Equal(frozenInt, result[2]);
        }

        [Fact]
        public void VirtualMembersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Mock<TypeWithVirtualMembers>>();
            // Assert
            Assert.Equal(frozenString, result.Object.VirtualGetOnlyProperty);
            Assert.Equal(frozenString, result.Object.VirtualMethod());
            Assert.Equal(frozenString, result.Object.VirtualProperty);
        }

        [Fact]
        public void MethodsWithParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithMethod>();
            // Assert
            Assert.Equal(frozenString, result.Method("hi"));
        }

        [Fact]
        public void MethodsWithOutParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithOutMethod>();
            // Assert
            int outResult;
            result.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void SealedSettablePropertiesAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Mock<TypeWithSealedMembers>>();
            // Assert
            Assert.Equal(frozenString, result.Object.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.Object.ImplicitlySealedProperty);
        }

        [Fact]
        public void OverridablePropertiesAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Equal(frozenString, result.Property);
        }

        [Fact]
        public void OverridablePropertiesAreStubbed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            result.Property = "a string";
            Assert.Equal("a string", result.Property);
        }

        [Fact]
        public void FieldsAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Mock<TypeWithPublicField>>();
            // Assert
            Assert.Equal(frozenString, result.Object.Field);
        }

        [Fact]
        public void SealedMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Mock<TypeWithSealedMembers> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<Mock<TypeWithSealedMembers>>()));
            Assert.NotEqual(frozenString, result.Object.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.Object.ExplicitlySealedMethod());
        }

        [Fact]
        public void RefMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            IInterfaceWithRefMethod result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<IInterfaceWithRefMethod>()));

            string refResult = "";
            string returnValue = result.Method(ref refResult);
            Assert.NotEqual(frozenString, refResult);
            Assert.NotEqual(frozenString, returnValue);
        }

        [Fact]
        public void GenericMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            IInterfaceWithGenericMethod result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<IInterfaceWithGenericMethod>()));

            Assert.NotEqual(frozenString, result.GenericMethod<string>());
        }

        [Fact]
        public void StaticMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithStaticMethod>>()));
        }

        [Fact]
        public void StaticPropertiesAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithStaticProperty>>()));
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void PrivateFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Mock<TypeWithPrivateField> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<Mock<TypeWithPrivateField>>()));

            Assert.NotEqual(frozenString, result.Object.GetPrivateField());
        }

        [Fact]
        public void ReadonlyFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Mock<TypeWithReadonlyField> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<Mock<TypeWithReadonlyField>>()));

            Assert.NotEqual(frozenString, result.Object.ReadonlyField);
        }

        [Fact]
        public void LiteralFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithConstField>>()));
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void StaticFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithStaticField>>()));
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void PropertiesWithCircularDependenciesAreNotAllowed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            // Act & Assert
            Assert.ThrowsAny<ObjectCreationException>(() => fixture.Create<IInterfaceWithPropertyWithCircularDependency>());
        }
    }
}
