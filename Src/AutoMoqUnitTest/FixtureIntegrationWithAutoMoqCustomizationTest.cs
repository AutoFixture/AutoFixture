using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class FixtureIntegrationWithAutoMoqCustomizationTest
    {
        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureAutoMocksAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureCanCreateMock()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<Mock<AbstractType>>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureCanFreezeMock()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var expected = new object();

            fixture.Freeze<Mock<IInterface>>()
                .Setup(a => a.MakeIt(It.IsAny<object>()))
                .Returns(expected);
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            var dummy = new object();
            Assert.Equal(expected, result.MakeIt(dummy));
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<IList<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void FixtureCanCreateMockOfAction()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var actual = fixture.Create<Mock<Action<string>>>();
            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void FixtureCanCreateUsableMockOfFunc()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<Version>();
            var mockOfFunc = fixture.Create<Mock<Func<int, Version>>>();
            mockOfFunc.Setup(f => f(42)).Returns(expected);

            // Act
            var actual = mockOfFunc.Object(42);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FixtureCanFreezeUsableMockOfFunc()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<Uri>();
            var mockOfFunc = fixture.Freeze<Mock<Func<Guid, decimal, Uri>>>();
            mockOfFunc
                .Setup(f => f(It.IsAny<Guid>(), 1337m))
                .Returns(expected);

            // Act
            var actual = mockOfFunc.Object(Guid.NewGuid(), 1337m);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FixtureCanCreateUsableMockOfCustomDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<string>();
            var mockOfDelegate = fixture.Create<Mock<RegularDelegate>>();
            mockOfDelegate.Setup(f => f(13, 37)).Returns(expected);

            // Act
            var actual = mockOfDelegate.Object.Invoke(13, 37);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithConfigureMembers_ParameterlessMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Assert
            Assert.Same(frozenString, ((IInterfaceWithShadowedMethod)result).Method(0));
        }

        [Fact]
        public void WithConfigureMembers_PropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Assert
            Assert.Equal(frozenInt, result[42]);
        }

        [Fact]
        public void WithConfigureMembers_VirtualMembersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Mock<TypeWithVirtualMembers>>();
            // Assert
            Assert.Equal(frozenString, result.Object.VirtualGetOnlyProperty);
            Assert.Equal(frozenString, result.Object.VirtualMethod());
            Assert.Equal(frozenString, result.Object.VirtualProperty);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithMethod>();
            // Assert
            Assert.Equal(frozenString, result.Method("hi"));
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithOutParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithOutMethod>();
            // Assert
            result.Method(out int outResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void WithConfigureMembers_SealedSettablePropertiesAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Mock<TypeWithSealedMembers>>();
            // Assert
            Assert.Equal(frozenString, result.Object.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.Object.ImplicitlySealedProperty);
        }

        [Fact]
        public void WithConfigureMembers_OverridablePropertiesAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Equal(frozenString, result.Property);
        }

        [Fact]
        public void WithConfigureMembers_OverridablePropertiesAreStubbed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<Mock<TypeWithPublicField>>();
            // Assert
            Assert.Equal(frozenString, result.Object.Field);
        }

        [Fact]
        public void WithConfigureMembers_SealedMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Mock<TypeWithSealedMembers> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<Mock<TypeWithSealedMembers>>()));
            Assert.NotEqual(frozenString, result.Object.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.Object.ExplicitlySealedMethod());
        }

        [Fact]
        public void WithConfigureMembers_RefMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
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
        public void WithConfigureMembers_GenericMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            IInterfaceWithGenericMethod result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<IInterfaceWithGenericMethod>()));

            Assert.NotEqual(frozenString, result.GenericMethod<string>());
        }

        [Fact]
        public void WithConfigureMembers_StaticMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithStaticMethod>>()));
        }

        [Fact]
        public void WithConfigureMembers_StaticPropertiesAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithStaticProperty>>()));
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void WithConfigureMembers_PrivateFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Mock<TypeWithPrivateField> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<Mock<TypeWithPrivateField>>()));

            Assert.NotEqual(frozenString, result.Object.GetPrivateField());
        }

        [Fact]
        public void WithConfigureMembers_ReadonlyFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Mock<TypeWithReadonlyField> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<Mock<TypeWithReadonlyField>>()));

            Assert.NotEqual(frozenString, result.Object.ReadonlyField);
        }

        [Fact]
        public void WithConfigureMembers_LiteralFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithConstField>>()));
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void WithConfigureMembers_StaticFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<Mock<TypeWithStaticField>>()));
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void WithConfigureMembers_PropertiesWithCircularDependenciesAreNotAllowed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            // Act & Assert
            Assert.ThrowsAny<ObjectCreationException>(() => fixture.Create<IInterfaceWithPropertyWithCircularDependency>());
        }

        [Theory]
        [InlineData(typeof(Action))]
        [InlineData(typeof(Action<int>))]
        [InlineData(typeof(Func<int, string>))]
        [InlineData(typeof(RegularDelegate))]
        [InlineData(typeof(DelegateWithRef))]
        [InlineData(typeof(DelegateWithOut))]
        [InlineData(typeof(GenericDelegate<int>))]
        public void WithGenerateDelegates_ReturnsMockForDelegates(Type delegateType)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization { GenerateDelegates = true });
            var context = new SpecimenContext(fixture);
            // Act
            var result = fixture.Create(delegateType, context);
            // Assert
            Assert.Null(Record.Exception(() => AssertIsMock(result, delegateType)));
        }

        [Fact]
        public void WithGenerateDelegatesAndConfigureMembers_ShouldReturnValueForRegularMethod()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenValue = fixture.Freeze<string>();
            // Act
            var mock = fixture.Create<RegularDelegate>();
            var callResult = mock.Invoke(42, 24);
            // Assert
            Assert.Equal(frozenValue, callResult);
        }

#if FIXED_DELEGATE_OUT
        [Fact]
        public void WithGenerateDelegatesAndConfigureMembers_ShouldReturnValueForMethodWithOut()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenString = fixture.Freeze<string>();
            var frozenInt = fixture.Freeze<int>();
            // Act
            var mock = fixture.Create<DelegateWithOut>();
            var callResult = mock.Invoke(out int outResult);
            // Assert
            Assert.Equal(frozenString, callResult);
            Assert.Equal(frozenInt, outResult);
        }
#endif

        [Fact]
        public void WithGenerateDelegateAndConfigureMembers_DelegatesWithRefMethodsAreNotConfigured()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenInt = fixture.Freeze<int>();
            var frozenString = fixture.Freeze<string>();
            // Act
            var mock = fixture.Create<DelegateWithRef>();
            // Assert
            int refResult = 0;
            string returnValue = mock.Invoke(ref refResult);
            Assert.NotEqual(frozenInt, refResult);
            Assert.NotEqual(frozenString, returnValue);
        }

        [Fact]
        public void WithGenerateDelegateAndConfigureMembers_GenericDelegatesShouldBeConfigured()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });
            var frozenString = fixture.Freeze<string>();
            // Act
            var mock = fixture.Create<GenericDelegate<string>>();
            var callResult = mock.Invoke("42");
            // Assert
            Assert.Equal(frozenString, callResult);
        }

        public delegate string RegularDelegate(short s, byte b);
        public delegate string DelegateWithRef(ref int arg);
        public delegate string DelegateWithOut(out int arg);
        public delegate string GenericDelegate<T>(T arg);

        private static void AssertIsMock(object mock, Type mockType)
        {
            typeof(Mock).GetTypeInfo()
                .GetMethod(nameof(Mock.Get))
                .MakeGenericMethod(mockType)
                .Invoke(null, new[] { mock });
        }
    }
}
