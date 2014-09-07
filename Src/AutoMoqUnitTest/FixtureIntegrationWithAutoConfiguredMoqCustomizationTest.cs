using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class FixtureIntegrationWithAutoConfiguredMoqCustomizationTest
    {
        [Fact]
        public void ParameterlessMethodsReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithParameterlessMethod>();
            // Verify outcome
            Assert.Same(frozenString, result.Method());
            // Teardown
        }

        [Fact]
        public void MethodsFromBaseInterfacesReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IDerivedInterface>();
            // Verify outcome
            Assert.Same(frozenString, result.Method());
            // Teardown
        }

        [Fact]
        public void InterfaceNewMethodsReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Verify outcome
            Assert.Same(frozenString, result.Method(0));
            // Teardown
        }

        [Fact]
        public void PropertiesReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithProperty>();
            // Verify outcome
            Assert.Same(frozenString, result.Property);
            // Teardown
        }

        [Fact]
        public void IndexersReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenInt = fixture.Freeze<int>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Verify outcome
            Assert.Equal(frozenInt, result[2]);
            // Teardown
        }

        [Fact]
        public void VirtualMembersReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<Mock<TypeWithVirtualMembers>>();
            // Verify outcome
            Assert.Equal(frozenString, result.Object.VirtualMethod());
            Assert.Equal(frozenString, result.Object.VirtualProperty);
            // Teardown
        }

        [Fact]
        public void MethodsWithParametersReturnValuesFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithMethod>();
            // Verify outcome
            Assert.Equal(frozenString, result.Method("hi"));
            // Teardown
        }

        [Fact]
        public void MethodsWithOutParametersReturnValuesFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenInt = fixture.Freeze<int>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithOutMethod>();
            // Verify outcome
            int outResult;
            result.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
            // Teardown
        }

        [Fact]
        public void SealedSettablePropertiesAreSetUsingFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<Mock<TypeWithSealedMembers>>();
            // Verify outcome
            Assert.Equal(frozenString, result.Object.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.Object.ImplicitlySealedProperty);
            // Teardown
        }

        [Fact]
        public void FieldsAreSetUsingFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<Mock<TypeWithPublicField>>();
            // Verify outcome
            Assert.Equal(frozenString, result.Object.Field);
            // Teardown
        }

        [Fact]
        public void SealedMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Mock<TypeWithSealedMembers> result = null;
            Assert.DoesNotThrow(() => result = fixture.Create<Mock<TypeWithSealedMembers>>());
            Assert.NotEqual(frozenString, result.Object.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.Object.ExplicitlySealedMethod());
        }

        [Fact]
        public void RefMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            IInterfaceWithRefMethod result = null;
            Assert.DoesNotThrow(() => result = fixture.Create<IInterfaceWithRefMethod>());

            string refResult = "";
            string returnValue = result.Method(ref refResult);
            Assert.NotEqual(frozenString, refResult);
            Assert.NotEqual(frozenString, returnValue);
        }

        [Fact]
        public void GenericMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            IInterfaceWithGenericMethod result = null;
            Assert.DoesNotThrow(() => result = fixture.Create<IInterfaceWithGenericMethod>());

            Assert.NotEqual(frozenString, result.GenericMethod<string>());
        }

        [Fact]
        public void StaticMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<Mock<TypeWithStaticMethod>>());
        }

        [Fact]
        public void StaticPropertiesAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<Mock<TypeWithStaticProperty>>());
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void PrivateFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Mock<TypeWithPrivateField> result = null;
            Assert.DoesNotThrow(() => result = fixture.Create<Mock<TypeWithPrivateField>>());

            Assert.NotEqual(frozenString, result.Object.GetPrivateField());
        }

        [Fact]
        public void ReadonlyFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Mock<TypeWithReadonlyField> result = null;
            Assert.DoesNotThrow(() => result = fixture.Create<Mock<TypeWithReadonlyField>>());

            Assert.NotEqual(frozenString, result.Object.ReadonlyField);
        }

        [Fact]
        public void LiteralFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<Mock<TypeWithConstField>>());
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void StaticFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<Mock<TypeWithStaticField>>());
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void CircularDependenciesAreAllowed()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<IComponent>());
        }

        public interface IComponent
        {
            IComponent Component { get; set; }
        }
    }
}
