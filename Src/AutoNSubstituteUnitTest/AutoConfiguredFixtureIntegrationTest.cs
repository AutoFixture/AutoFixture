using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class AutoConfiguredFixtureIntegrationTest
    {
        [Fact]
        public void ParameterlessMethodsReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithParameterlessMethod>();
            // Verify outcome
            Assert.Same(frozenString, result.Method());
            // Teardown
        }

        [Fact]
        public void PropertiesReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<TypeWithVirtualMembers>();
            // Verify outcome
            Assert.Equal(frozenString, result.VirtualMethod());
            Assert.Equal(frozenString, result.VirtualProperty);
            // Teardown
        }

        [Fact]
        public void MethodsWithParametersReturnValuesFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<TypeWithSealedMembers>();
            // Verify outcome
            Assert.Equal(frozenString, result.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.ImplicitlySealedProperty);
            // Teardown
        }

        [Fact]
        public void FieldsAreSetUsingFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<TypeWithPublicField>();
            // Verify outcome
            Assert.Equal(frozenString, result.Field);
            // Teardown
        }

        [Fact]
        public void SealedMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            var result = fixture.Create<TypeWithSealedMembers>();

            Assert.NotEqual(frozenString, result.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.ExplicitlySealedMethod());
        }

        [Fact]
        public void RefMethodsReturnValuesFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            var result = fixture.Create<IInterfaceWithRefMethod>();

            string refResult = "";
            string returnValue = result.Method(ref refResult);

            Assert.Equal(frozenString, refResult);
            Assert.Equal(frozenString, returnValue);
        }

        [Fact]
        public void GenericMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            var result = fixture.Create<IInterfaceWithGenericMethod>();

            Assert.NotEqual(frozenString, result.GenericMethod<string>());
        }

        [Fact]
        public void StaticMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<TypeWithStaticMethod>());
            Assert.NotEqual(frozenString, TypeWithStaticMethod.StaticMethod());
        }

        [Fact]
        public void StaticPropertiesAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<TypeWithStaticProperty>());
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void PrivateFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            var result = fixture.Create<TypeWithPrivateField>();

            Assert.NotEqual(frozenString, result.GetPrivateField());
        }

        [Fact]
        public void ReadonlyFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            var result = fixture.Create<TypeWithReadonlyField>();

            Assert.NotEqual(frozenString, result.ReadonlyField);
        }

        [Fact]
        public void LiteralFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<TypeWithConstField>());
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void StaticFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<TypeWithStaticField>());
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void CircularDependenciesAreAllowed()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => fixture.Create<IInterfaceWithCircularDependency>());
        }

        [Fact]
        public void SubsequentCallsWithSameParameterReturnSameValueFromFixture()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();

            var result1 = substitute.Method(anonymousString);
            var result2 = substitute.Method(anonymousString);

            Assert.Equal(result1, result2);
        }

        [Fact]
        public void SubsequentCallsReturnValuesFromFixture()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString1 = fixture.Create<string>();
            var anonymousString2 = fixture.Create<string>();

            var result1 = substitute.Method(anonymousString1);
            var result2 = substitute.Method(anonymousString2);

            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void SetupCallsReturnValueFromSubstituteSetup()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var expected = fixture.Create<string>();
            substitute.Method(anonymousString).Returns(expected);

            var result = substitute.Method(anonymousString);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetupCallsOverrideValueFromFixture()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var result1 = substitute.Method(anonymousString);
            var expected = fixture.Create<string>();
            substitute.Method(anonymousString).Returns(expected);

            var result2 = substitute.Method(anonymousString);

            Assert.NotEqual(result1, result2);
            Assert.Equal(expected, result2);
        }

        [Fact]
        public void SetupCallsReturningAbstractTypesAreOverridable()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithCircularDependency>();
            var expected = fixture.Create<IInterfaceWithCircularDependency>();
            substitute.Component.Returns(expected);

            var result = substitute.Component;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void VoidMethodsAreIgnored()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenInt = fixture.Freeze<int>();
            var substitute = fixture.Create<IInterfaceWithOutVoidMethod>();

            int result;
            substitute.Method(out result);

            Assert.NotEqual(frozenInt, result);
        }

        [Fact]
        public void VoidMethodsReturnValuesSetup()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithOutVoidMethod>();
            var expected = fixture.Create<int>();
            int result;
            substitute.When(x => x.Method(out result)).Do(x => x[0] = expected);

            substitute.Method(out result);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ChainedSubstitutesAreVerifyable()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithCircularMethod>();
            var anonymousObject = new object();
            var anotherSubstitute = substitute.Method(anonymousObject);
            var anotherAnonymousObject = new object();

            anotherSubstitute.Method(anotherAnonymousObject);

            anotherSubstitute.Received().Method(anotherAnonymousObject);
        }

        [Fact]
        public void ChainedSubstitutesReturnValueFromFixture()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenObject = fixture.Freeze<object>();
            var substitute = fixture.Create<IInterfaceWithCircularMethod>();
            var anonymousObject = new object();
            var anotherSubstitute = substitute.Method(anonymousObject);

            var result = anotherSubstitute.AnotherMethod(new object());

            Assert.Equal(frozenObject, result);
        }

        [Fact]
        public void SubstitutesCreatedFromNSubstituteDoNotReturnValueFromFixture()
        {
            var substitute = Substitute.For<IInterfaceWithMethod>();
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();

            var result = substitute.Method(frozenString);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void SubstitutesCountReceivedCallsCorrectly()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString1 = fixture.Create<string>();
            var anonymousString2 = fixture.Create<string>();

            substitute.Method(anonymousString1);
            substitute.Method(anonymousString2);

            substitute.Received(1).Method(anonymousString1);
            substitute.Received(1).Method(anonymousString2);
            substitute.ReceivedWithAnyArgs(2).Method(null);
        }

        [Fact]
        public void MethodsFromBaseInterfacesReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Verify outcome
            Assert.Same(frozenString, result.Method(0));
            // Teardown
        }

        [Fact]
        public void InterfaceShadowedMethodsReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Verify outcome
            Assert.Same(frozenString, (result as IInterfaceWithShadowedMethod).Method(0));
            // Teardown
        }

    }
}