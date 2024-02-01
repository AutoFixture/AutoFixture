using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.AutoFakeItEasy.UnitTest.TestTypes;
using FakeItEasy;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoFakesInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.IsAssignableFrom<IInterface>(result);
        }

        [Fact]
        public void FixtureAutoFakesAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.IsAssignableFrom<AbstractType>(result);
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureCanCreateFakeOfDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { GenerateDelegates = true });
            // Act
            var result = fixture.Create<Fake<Func<int, int>>>();
            // Assert
            Assert.IsAssignableFrom<Fake<Func<int, int>>>(result);
        }

        [Fact]
        public void FixtureCanCreateDelegateThatIsAFake()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { GenerateDelegates = true });
            // Act
            var result = fixture.Create<Func<int, int>>();
            // Assert
            Assert.IsAssignableFrom<Func<int, int>>(result);
            Assert.NotNull(Fake.GetFakeManager(result));
        }

        [Fact]
        public void FixtureCanFreezeFakeOfDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { GenerateDelegates = true });
            // Act
            var frozen = fixture.Freeze<Fake<Func<int, int>>>();
            var result = fixture.Create<Func<int, int>>();
            // Assert
            Assert.Same(frozen.FakedObject, result);
        }

        [Fact]
        public void FixtureWithDefaultCustomizationCanCreateNonFakedDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<Func<int, int>>();
            // Assert
            Assert.IsAssignableFrom<Func<int, int>>(result);

            var notAFakeException = Assert.Throws<ArgumentException>(() => Fake.GetFakeManager(result));
            Assert.Contains("not recognized as a fake", notAFakeException.Message);
        }

        [Fact]
        public void FixtureCanCreateFake()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<Fake<AbstractType>>();
            // Assert
            Assert.IsAssignableFrom<Fake<AbstractType>>(result);
        }

        [Fact]
        public void FixtureCanFreezeFake()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var dummy = new object();
            var fake = fixture.Freeze<Fake<IInterface>>();
            fake.CallsTo(x => x.MakeIt(dummy))
                .Returns(null);
            // Act
            var result = fixture.Create<IInterface>();
            result.MakeIt(dummy);
            // Assert
            A.CallTo(() => result.MakeIt(dummy)).MustHaveHappened();
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<IList<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.IsAssignableFrom<AbstractGenericType<object>>(result);
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Assert
            Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<Guid>();
            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public void WithConfigureMembers_ParameterlessMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithParameterlessMethod>();
            // Assert
            Assert.Same(frozenString, result.Method());
        }

        [Fact]
        public void WithConfigureMembers_MethodsFromBaseInterfacesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IDerivedInterface>();
            // Assert
            Assert.Same(frozenString, result.Method());
        }

        [Fact]
        public void WithConfigureMembers_PropertiesFromBaseInterfacesIsSetupLikeRealProperty()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var expected = fixture.Create<string>();
            // Act
            var result = fixture.Create<IDerivedInterfaceOfDerivedInterfaceWithProperty>();
            result.Property = expected;
            result.DerivedProperty = expected;
            // Assert
            Assert.Same(expected, result.Property);
            Assert.Same(expected, result.DerivedProperty);
        }

        [Fact]
        public void WithConfigureMembers_InterfaceNewMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Assert
            Assert.Same(frozenString, result.Method(0));
        }

        [Fact]
        public void WithConfigureMembers_InterfaceShadowedMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Assert
            Assert.Same(frozenString, ((IInterfaceWithShadowedMethod)result).Method(0));
        }

        [Fact]
        public void WithConfigureMembers_NewInterfaceMethodReturnsDifferentValueThanShadowedMethod()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            IInterfaceWithNewMethod fakeNewInterface = fixture.Create<IInterfaceWithNewMethod>();
            IInterfaceWithShadowedMethod fakeShadowedInterface = fakeNewInterface;
            // Act
            var newReturnValue = fakeNewInterface.Method(3);
            var shadowedReturnValue = fakeShadowedInterface.Method(3);
            // Assert
            Assert.NotEqual(newReturnValue, shadowedReturnValue);
        }

        [Fact]
        public void WithConfigureMembers_PropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Same(frozenString, result.Property);
        }

        [Fact]
        public void WithConfigureMembers_GetOnlyPropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithGetOnlyProperty>();
            // Assert
            Assert.Same(frozenString, result.GetOnlyProperty);
        }

        [Fact]
        public void WithConfigureMembers_IndexersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Assert
            Assert.Equal(frozenInt, result[42]);
        }

        [Fact]
        public void WithConfigureMembers_OverridableIndexersReturnSameValueWhenPassedSameArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Act
            var value1 = result[7];
            var value2 = result[7];
            // Assert
            Assert.Equal(value1, value2);
        }

        [Fact]
        public void WithConfigureMembers_OverridableIndexersReturnDifferentValueWhenPassedDifferentArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Act
            var value1 = result[7];
            var value2 = result[8];
            // Assert
            Assert.NotEqual(value1, value2);
        }

        [Fact]
        public void WithConfigureMembers_VirtualMembersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Fake<TypeWithVirtualMembers>>();
            // Assert
            Assert.Equal(frozenString, result.FakedObject.VirtualGetOnlyProperty);
            Assert.Equal(frozenString, result.FakedObject.VirtualMethod());
            Assert.Equal(frozenString, result.FakedObject.VirtualProperty);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithMethod>();
            // Assert
            Assert.Equal(frozenString, result.Method("hi"));
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithParametersReturnSameValuesWhenCalledWithSameArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithMethod>();
            // Act
            var returnValue1 = result.Method("an argument");
            var returnValue2 = result.Method("an argument");
            // Assert
            Assert.Equal(returnValue1, returnValue2);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithParametersReturnDifferentValuesWhenCalledWithDifferentArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithMethod>();
            // Act
            var returnValue1 = result.Method("an argument");
            var returnValue2 = result.Method("a different argument");
            // Assert
            Assert.NotEqual(returnValue1, returnValue2);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithRefParametersReturnSameValuesWhenCalledWithSameArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithRefMethod>();
            // Act
            int argument = 42;
            var returnValue1 = result.Method(ref argument);
            argument = 42;
            var returnValue2 = result.Method(ref argument);
            // Assert
            Assert.Equal(returnValue1, returnValue2);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithOutParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithOutMethod>();
            // Assert
            result.Method(out int outResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithParamsParameterReturnSameValuesWhenCalledWithSameArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithParamsMethod>();
            // Act
            var returnValue1 = result.Method(1, 2);
            var returnValue2 = result.Method(1, 2);
            // Assert
            Assert.Equal(returnValue1, returnValue2);
        }

        [Fact]
        public void WithConfigureMembers_IndexersWithParamsParameterReturnSameValuesWhenCalledWithSameIndexes()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithParamsMethod>();
            // Act
            var returnValue1 = result[1, 2];
            var returnValue2 = result[1, 2];
            // Assert
            Assert.Equal(returnValue1, returnValue2);
        }

        [Fact]
        public void WithConfigureMembers_IndexersWithParamsParameterReturnPreviouslySetValueWhenGetterCalledWithSameIndexes()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<IInterfaceWithParamsMethod>();
            // Act
            result[3, 9, 18] = 17;
            var returnValue = result[3, 9, 18];
            // Assert
            Assert.Equal(17, returnValue);
        }

        [Fact]
        public void WithConfigureMembers_SealedSettablePropertiesAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Fake<TypeWithSealedMembers>>();
            // Assert
            Assert.Equal(frozenString, result.FakedObject.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.FakedObject.ImplicitlySealedProperty);
        }

        [Fact]
        public void WithConfigureMembers_ExplicitlyImplementedPropertyReturnsDefaultValue()
        {
            // Members that explicitly implement interface members do not appear as public, so will not be
            // configured by ConfigureMembers, and FakeItEasy does not intercept them either, so the default
            // behavior will always be in place.

            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<Fake<TypeWithExplicitlyImplementedProperty>>();
            // Act
            IInterfaceWithProperty interfaceWithProperty = result.FakedObject;
            // Assert
            Assert.Equal(default(string), interfaceWithProperty.Property);
        }

        [Fact]
        public void WithConfigureMembers_OverridablePropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Equal(frozenString, result.Property);
        }

        [Fact]
        public void WithConfigureMembers_VirtualPropertyShouldNotBeMarkedAsCalled()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            A.CallToSet(() => result.Property).MustNotHaveHappened();
        }

        [Fact]
        public void WithConfigureMembers_OverridablePropertiesAreStubbed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            result.Property = "a string";
            Assert.Equal("a string", result.Property);
        }

        [Fact]
        public void WithConfigureMembers_FieldsAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Fake<TypeWithPublicField>>();
            // Assert
            Assert.Equal(frozenString, result.FakedObject.Field);
        }

        [Fact]
        public void WithConfigureMembers_SealedMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Fake<TypeWithSealedMembers>>();
            // Assert
            Assert.NotEqual(frozenString, result.FakedObject.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.FakedObject.ExplicitlySealedMethod());
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithRefParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithRefMethod>();
            // Assert
            string refResult = string.Empty;
            string returnValue = result.Method(ref refResult);
            Assert.Equal(frozenString, refResult);
            Assert.Equal(frozenString, returnValue);
        }

        [Fact]
        public void WithConfigureMembers_GenericMethodsReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithGenericMethod>();
            // Assert
            Assert.Equal(frozenString, result.GenericMethod<string>());
        }

        [Fact]
        public void WithConfigureMembers_StaticMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            fixture.Create<Fake<TypeWithStaticMethod>>();
            // Assert
            Assert.NotEqual(frozenString, TypeWithStaticMethod.StaticMethod());
        }

        [Fact]
        public void WithConfigureMembers_StaticPropertiesAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            fixture.Create<Fake<TypeWithStaticProperty>>();
            // Assert
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void WithConfigureMembers_PrivateFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Fake<TypeWithPrivateField>>();
            // Assert
            Assert.NotEqual(frozenString, result.FakedObject.GetPrivateField());
        }

        [Fact]
        public void WithConfigureMembers_ReadonlyFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Fake<TypeWithReadonlyField>>();
            // Assert
            Assert.NotEqual(frozenString, result.FakedObject.ReadonlyField);
        }

        [Fact]
        public void WithConfigureMembers_LiteralFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            fixture.Create<Fake<TypeWithConstField>>();
            // Assert
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void WithConfigureMembers_StaticFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            fixture.Create<Fake<TypeWithStaticField>>();
            // Assert
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void WithConfigureMembers_PropertiesWithCircularDependenciesAreAllowed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            // Act
            var result = fixture.Create<IInterfaceWithPropertyWithCircularDependency>();
            // Assert
            Assert.IsAssignableFrom<IInterfaceWithPropertyWithCircularDependency>(result);
        }

        [Fact]
        public void WithConfigureMembers_VirtualMemberConfiguredToCallBaseMethodReturnsValueFromBaseMethod()
        {
            // Arrange
            var expectedValue = new TypeWithVirtualMembers().VirtualMethod();
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
            var result = fixture.Create<Fake<TypeWithVirtualMembers>>();
            A.CallTo(() => result.FakedObject.VirtualMethod()).CallsBaseMethod();
            // Act
            // Assert
            var virtualMethodReturnValue = result.FakedObject.VirtualMethod();
            Assert.Equal(expectedValue, virtualMethodReturnValue);
        }

        [Fact]
        public void WithGenerateDelegatesAndConfigureMembers_ShouldReturnValueForRegularMethod()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenValue = fixture.Freeze<string>();
            // Act
            var fake = fixture.Create<RegularDelegate>();
            var callResult = fake.Invoke(42, 24);
            // Assert
            Assert.Equal(frozenValue, callResult);
        }

        [Fact]
        public void WithGenerateDelegatesAndConfigureMembers_ShouldReturnSameValueWhenCalledWithSameArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var fake = fixture.Create<RegularDelegate>();
            // Act
            var returnValue1 = fake.Invoke(42, 24);
            var returnValue2 = fake.Invoke(42, 24);
            // Assert
            Assert.Equal(returnValue1, returnValue2);
        }

        [Fact]
        public void WithGenerateDelegatesAndConfigureMembers_ShouldReturnDifferentValuesWhenCalledWithDifferentArguments()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var fake = fixture.Create<RegularDelegate>();
            // Act
            var returnValue1 = fake.Invoke(24, 42);
            var returnValue2 = fake.Invoke(42, 24);
            // Assert
            Assert.NotEqual(returnValue1, returnValue2);
        }

        [Fact]
        public void WithGenerateDelegatesAndConfigureMembers_ShouldReturnValueForMethodWithOut()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenString = fixture.Freeze<string>();
            var frozenInt = fixture.Freeze<int>();
            // Act
            var fake = fixture.Create<DelegateWithOut>();
            var callResult = fake.Invoke(out int outResult);
            // Assert
            Assert.Equal(frozenString, callResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void WithGenerateDelegateAndConfigureMembers_DelegatesWithRefParametersAreConfigured()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenInt = fixture.Freeze<int>();
            var frozenString = fixture.Freeze<string>();
            // Act
            var fake = fixture.Create<DelegateWithRef>();
            // Assert
            int refResult = 0;
            string returnValue = fake.Invoke(ref refResult);
            Assert.Equal(frozenInt, refResult);
            Assert.Equal(frozenString, returnValue);
        }

        [Fact]
        public void WithGenerateDelegateAndConfigureMembers_GenericDelegatesShouldBeConfigured()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenString = fixture.Freeze<string>();
            // Act
            var fake = fixture.Create<GenericDelegate<string>>();
            var callResult = fake.Invoke("42");
            // Assert
            Assert.Equal(frozenString, callResult);
        }

        public delegate string RegularDelegate(short s, byte b);
        public delegate string DelegateWithRef(ref int arg);
        public delegate string DelegateWithOut(out int arg);
        public delegate string GenericDelegate<T>(T arg);
    }
}