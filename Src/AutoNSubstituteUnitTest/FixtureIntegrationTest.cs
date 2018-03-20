using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using AutoFixture.Kernel;
using NSubstitute;
using NSubstitute.ClearExtensions;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoSubstitutesInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.IsAssignableFrom<IInterface>(result);
        }

        [Fact]
        public void FixtureAutoSubstitutesAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.IsAssignableFrom<AbstractType>(result);
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Assert
            Assert.NotEqual(0, result.Property1);
            Assert.NotEqual(0, result.Property2);
        }

        [Fact]
        public void FixtureCanFreezeSubstitute()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var dummy = new object();
            var substitute = fixture.Freeze<IInterface>();
            substitute.MakeIt(dummy).Returns(null);
            // Act
            var result = fixture.Create<IInterface>();
            result.MakeIt(dummy);
            // Assert
            result.Received().MakeIt(dummy);
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<IList<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.IsAssignableFrom<AbstractGenericType<object>>(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<Guid>();
            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Assert
            Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result);
        }

        [Theory]
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(ICollection<object>))]
        [InlineData(typeof(IList<object>))]
        public void FixtureDoesNotHijackCollectionInterfaces(Type collectionInterface)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var context = new SpecimenContext(fixture);
            // Act
            var result = context.Resolve(new SeededRequest(collectionInterface, null));
            // Assert
            Assert.NotEmpty((IEnumerable)result);
        }

        [Fact]
        public void WithConfigureMembers_ParameterlessMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithParameterlessMethod>();
            // Assert
            Assert.Same(frozenString, result.Method());
        }

        [Fact]
        public void WithConfigureMembers_PropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Same(frozenString, result.Property);
        }

        [Fact]
        public void WithConfigureMembers_IndexersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithIndexer>();
            // Assert
            Assert.Equal(frozenInt, result[2]);
        }

        [Fact]
        public void WithConfigureMembers_VirtualMembersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<TypeWithVirtualMembers>();
            // Assert
            Assert.Equal(frozenString, result.VirtualMethod());
            Assert.Equal(frozenString, result.VirtualProperty);
        }

        [Fact]
        public void WithConfigureMembers_MethodsWithParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
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
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            // Act
            var result = fixture.Create<IInterfaceWithOutMethod>();
            // Assert
            result.Method(out var outResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void WithConfigureMembers_SealedSettablePropertiesAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<TypeWithSealedMembers>();
            // Assert
            Assert.Equal(frozenString, result.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.ImplicitlySealedProperty);
        }

        [Fact]
        public void WithConfigureMembers_FieldsAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<TypeWithPublicField>();
            // Assert
            Assert.Equal(frozenString, result.Field);
        }

        [Fact]
        public void WithConfigureMembers_SealedMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<TypeWithSealedMembers>();

            Assert.NotEqual(frozenString, result.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.ExplicitlySealedMethod());
        }

        [Fact]
        public void WithConfigureMembers_RefMethodsReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<IInterfaceWithRefMethod>();

            string refResult = "";
            string returnValue = result.Method(ref refResult);

            Assert.Equal(frozenString, refResult);
            Assert.Equal(frozenString, returnValue);
        }

        [Fact]
        public void WithConfigureMembers_GenericMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act
            var obj = fixture.Create<IInterfaceWithGenericMethod>();
            var result = obj.GenericMethod<string>();

            // Assert
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void WithConfigureMembers_StaticMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticMethod>()));
            Assert.NotEqual(frozenString, TypeWithStaticMethod.StaticMethod());
        }

        [Fact]
        public void WithConfigureMembers_StaticPropertiesAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticProperty>()));
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void WithConfigureMembers_PrivateFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<TypeWithPrivateField>();

            Assert.NotEqual(frozenString, result.GetPrivateField());
        }

        [Fact]
        public void WithConfigureMembers_ReadonlyFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<TypeWithReadonlyField>();

            Assert.NotEqual(frozenString, result.ReadonlyField);
        }

        [Fact]
        public void WithConfigureMembers_LiteralFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithConstField>()));
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void WithConfigureMembers_StaticFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticField>()));
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void WithConfigureMembers_CircularDependenciesAreAllowed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<IInterfaceWithCircularDependency>()));
        }

        [Fact]
        public void WithConfigureMembers_SubsequentCallsWithSameParameterReturnSameValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();

            // Act
            var result1 = substitute.Method(anonymousString);
            var result2 = substitute.Method(anonymousString);

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void WithConfigureMembers_SubsequentCallsWithoutParametersReturnSameValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString1 = fixture.Create<string>();
            var anonymousString2 = fixture.Create<string>();

            // Act
            var result1 = substitute.Method(anonymousString1);
            var result2 = substitute.Method(anonymousString2);

            // Assert
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void WithConfigureMembers_SetupCallsReturnValueFromSubstituteSetup()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var expected = fixture.Create<string>();

            substitute.Method(anonymousString).Returns(expected);

            // Act
            var result = substitute.Method(anonymousString);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_SetupCallsOverrideValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var result1 = substitute.Method(anonymousString);
            var expected = fixture.Create<string>();

            substitute.Method(anonymousString).Returns(expected);

            // Act
            var result2 = substitute.Method(anonymousString);

            // Assert
            Assert.NotEqual(result1, result2);
            Assert.Equal(expected, result2);
        }

        [Fact]
        public void WithConfigureMembers_SetupCallsReturningAbstractTypesAreOverridable()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithCircularDependency>();
            var expected = fixture.Create<IInterfaceWithCircularDependency>();

            substitute.Component.Returns(expected);

            // Act
            var result = substitute.Component;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_VoidMethodsWithOutParameterIsFilled()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            var substitute = fixture.Create<IInterfaceWithOutVoidMethod>();

            // Act
            substitute.Method(out var result);

            // Assert
            Assert.Equal(frozenInt, result);
        }

        [Fact]
        public void WithConfigureMembers_VoidMethodsWithRefParameterIsFilled()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenInt = fixture.Freeze<int>();
            var substitute = fixture.Create<IInterfaceWithRefVoidMethod>();

            // Act
            int result = 0;
            substitute.Method(ref result);

            // Assert
            Assert.Equal(frozenInt, result);
        }

        [Fact]
        public void WithConfigureMembers_VoidOutMethodsReturnValuesSetup()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithOutVoidMethod>();
            var expected = fixture.Create<int>();
            int result;

            substitute
                .When(x => x.Method(out result))
                .Do(x => x[0] = expected);

            // Act
            substitute.Method(out result);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_VoidRefMethodsReturnValuesSetup()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var subsitute = fixture.Create<IInterfaceWithRefVoidMethod>();
            var expected = fixture.Create<int>();

            int origIntValue = -42;
            subsitute.When(x => x.Method(ref origIntValue)).Do(x => x[0] = expected);

            // Act
            int result = -42;
            subsitute.Method(ref result);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_ChainedSubstitutesAreVerifyable()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithCircularMethod>();
            var anonymousObject = new object();

            var anotherSubstitute = substitute.Method(anonymousObject);
            var anotherAnonymousObject = new object();

            // Act
            anotherSubstitute.Method(anotherAnonymousObject);

            // Assert
            anotherSubstitute.Received().Method(anotherAnonymousObject);
        }

        [Fact]
        public void WithConfigureMembers_ChainedSubstitutesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenObject = fixture.Freeze<object>();
            var substitute = fixture.Create<IInterfaceWithCircularMethod>();

            var anonymousObject = new object();
            IInterfaceWithCircularMethod anotherSubstitute = substitute.Method(anonymousObject);

            // Act
            var result = anotherSubstitute.AnotherMethod(new object());

            // Assert
            Assert.Equal(frozenObject, result);
        }

        [Fact]
        public void WithConfigureMembers_SubstitutesCreatedFromNSubstituteDoNotReturnValueFromFixture()
        {
            // Arrange
            var substitute = Substitute.For<IInterfaceWithMethod>();
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();

            // Act
            var result = substitute.Method(frozenString);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WithConfigureMembers_SubstitutesCountReceivedCallsCorrectly()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString1 = fixture.Create<string>();
            var anonymousString2 = fixture.Create<string>();

            // Act
            substitute.Method(anonymousString1);
            substitute.Method(anonymousString2);

            // Assert
            substitute.Received(1).Method(anonymousString1);
            substitute.Received(1).Method(anonymousString2);
            substitute.Received(2).Method(Arg.Any<string>());
        }

        [Fact]
        public void WithConfigureMembers_MethodsFromBaseInterfacesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            var substitute = fixture.Create<IDerivedInterface>();
            // Act
            var result = substitute.Method();
            // Assert
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void WithConfigureMembers_InterfaceNewMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithNewMethod>();
            // Act
            var result = substitute.Method(0);
            // Assert
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void WithConfigureMembers_InterfaceShadowedMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithNewMethod>();
            // Act
            var shadowedInterface = (IInterfaceWithShadowedMethod)substitute;
            var result = shadowedInterface.Method(0);
            // Assert
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void WithConfigureMembers_InterfacesImplementingIEnumerableReturnFiniteSequence()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var repeatCount = fixture.Create<int>();
            fixture.RepeatCount = repeatCount;

            var sut = fixture.Create<IDerivedFromEnumerableInterface<string>>();

            // Act
            var result = sut.Take(repeatCount + 1).Count();

            // Assert
            Assert.Equal(repeatCount, result);
        }

        [Fact]
        public void WithConfigureMembers_PropertiesOmittingSpecimenReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            fixture.Customizations.Add(
                new Omitter(new PropertySpecification(typeof(string), nameof(IInterfaceWithProperty.Property))));
            var sut = fixture.Create<IInterfaceWithProperty>();

            // Act
            var result = sut.Property;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WithConfigureMembers_MethodsOmittingSpecimenReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var anonymousString = fixture.Create<string>();
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof(string))));
            var sut = fixture.Create<IInterfaceWithMethod>();

            // Act
            var result = sut.Method(anonymousString);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WithConfigureMembers_RefMethodsOmittingSpecimenReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var frozenString = fixture.Freeze<string>();
            var intValue = fixture.Create<int>();
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof(int))));

            var sut = fixture.Create<IInterfaceWithRefIntMethod>();

            // Act
            var refResult = intValue;
            var result = sut.Method(ref refResult);

            // Assert
            Assert.Equal(frozenString, result);
            Assert.Equal(intValue, refResult);
        }

        [Fact]
        public void WithConfigureMembers_OutMethodsOmittingSpecimenReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var intValue = fixture.Create<int>();
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof(int))));
            var sut = fixture.Create<IInterfaceWithOutMethod>();

            // Act
            int refResult = intValue;
            sut.Method(out refResult);

            // Assert
            Assert.Equal(intValue, refResult);
        }

        [Fact]
        public void WithConfigureMembers_SetupCallsOverrideValueFromFixtureWithAnyArgumentMatcher()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var expected = fixture.Create<string>();

            substitute.Method(Arg.Any<string>()).Returns(expected);

            // Act
            var result = substitute.Method(anonymousString);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_SetupCallsOverrideValueFromFixtureWithIsArgumentMatcher()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var expected = fixture.Create<string>();

            substitute.Method(Arg.Is<string>(_ => true)).Returns(expected);

            // Act
            var result = substitute.Method(anonymousString);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_InterfaceImplementingAnotherReturnsValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var expected = fixture.Freeze<int>();
            var sut = fixture.Create<IDerivedInterfaceWithOwnMethod>();

            // Act
            var result = sut.IntMethod();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithConfigureMembers_GenericMethodsWithRefReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var expectedInt = fixture.Freeze<int>();
            var expectedStr = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericRefMethod>();

            // Act
            string refValue = "dummy";
            int retValue = substitute.GenericMethod<string>(ref refValue);

            // Assert
            Assert.Equal(expectedInt, retValue);
            Assert.Equal(expectedStr, refValue);
        }

        [Fact]
        public void WithConfigureMembers_GenericMethodsWithOutReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var expectedInt = fixture.Freeze<int>();
            var expectedStr = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericOutMethod>();

            // Act
            string outValue;
            int retValue = substitute.GenericMethod<string>(out outValue);

            // Assert
            Assert.Equal(expectedInt, retValue);
            Assert.Equal(expectedStr, outValue);
        }

        [Fact]
        public void WithConfigureMembers_VoidGenericMethodsWithRefReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var expected = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericRefVoidMethod>();

            // Act
            string refValue = "dummy";
            substitute.GenericMethod<string>(ref refValue);

            // Assert
            Assert.Equal(expected, refValue);
        }

        [Fact]
        public void WithConfigureMembers_VoidGenericMethodsWithOutReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var expected = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericOutVoidMethod>();

            // Act
            string outValue;
            substitute.GenericMethod<string>(out outValue);

            // Assert
            Assert.Equal(expected, outValue);
        }

        [Fact]
        public void WithConfigureMembers_ReturnValueIsSameForSameArgumentForGenerics()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();

            var inputValue = 42;

            // Act
            var result1 = substitute.GenericMethod<int>(inputValue);
            var result2 = substitute.GenericMethod<int>(inputValue);

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void WithConfigureMembers_ReturnValueIsDifferentForDifferentArgumentForGenerics()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();

            // Act
            var result1 = substitute.GenericMethod<int>(42);
            var result2 = substitute.GenericMethod<int>(24);

            // Assert
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void WithConfigureMembers_ResultIsDifferentForDifferentGenericInstantiations()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithGenericMethod>();

            // Act
            var intResult = substitute.GenericMethod<int>();
            var longResult = substitute.GenericMethod<long>();

            // Assert
            Assert.NotEqual(intResult, longResult);
        }

        [Fact]
        public void WithConfigureMembers_CachedResultIsMatchedByOtherInterfaceSubstituteForGenerics()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();
            var arg = fixture.Create<IInterfaceWithMethod>();

            // Act
            int result1 = substitute.GenericMethod<IInterfaceWithMethod>(arg);
            int result2 = substitute.GenericMethod<IInterfaceWithMethod>(arg);

            // Assert
            Assert.Equal(result1, result2);
        }

        /// <summary>
        /// Subsitute clear is used to reset manually configured user returns.
        /// The values configured by AutoFixture are not being manually-configured.
        ///
        /// If user needs that, it could easily override the auto-generated value using the
        /// substitute.Method(...).Returns(...);
        /// </summary>
        [Theory]
        [InlineData(ClearOptions.CallActions)]
        [InlineData(ClearOptions.ReceivedCalls)]
        [InlineData(ClearOptions.ReturnValues)]
        [InlineData(ClearOptions.All)]
        public void WithConfigureMembers_ShouldNotResetCachedValuesOnSubsituteClear(ClearOptions options)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethod>();
            string arg = "argValue";

            // Act
            var resultBefore = substitute.Method(arg);
            substitute.ClearSubstitute(options);
            var resultAfter = substitute.Method(arg);

            // Assert
            Assert.Equal(resultBefore, resultAfter);
        }

        /// <summary>
        /// Current implementation of NSubsitute doesn't call custom handlers for the Received.InOrder() scope (which
        /// we use for our integration with NSubstitute). That shouldn't cause any usability issues for users.
        ///
        /// Asserting that behavior via test to get a notification when that behavior changes, so we can make a decision
        /// whether we need to alter something in AF or not to respect that change.
        /// </summary>
        [Fact]
        public void WithConfigureMembers_IsNotExpectedToReturnValueInReceivedInOrderBlock()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var actualResult = substitute.Method();

            // Act
            IInterfaceWithMethod capturedResult = null;
            Received.InOrder(() => { capturedResult = substitute.Method(); });

            // Assert
            Assert.NotEqual(actualResult, capturedResult);
        }

        private class Issue630_TryingAlwaysSatisfyInlineTaskScheduler : TaskScheduler
        {
            private const int DELAY_MSEC = 100;
            private readonly object syncRoot = new object();
            private HashSet<Task> Tasks { get; } = new HashSet<Task>();

            protected override void QueueTask(Task task)
            {
                lock (this.syncRoot)
                {
                    this.Tasks.Add(task);
                }

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Thread.Sleep(DELAY_MSEC);

                    // If task cannot be dequeued - it was already executed.
                    if (this.TryDequeue(task))
                    {
                        this.TryExecuteTask(task);
                    }
                });
            }

            protected override bool TryDequeue(Task task)
            {
                lock (this.syncRoot)
                {
                    return this.Tasks.Remove(task);
                }
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                // If was queued, try to remove from queue before inlining. Ignore otherwise - it's already executed.
                if (taskWasPreviouslyQueued && !this.TryDequeue(task))
                {
                    return false;
                }

                this.TryExecuteTask(task);
                return true;
            }

            protected override IEnumerable<Task> GetScheduledTasks()
            {
                lock (this.syncRoot)
                {
                    // Create copy to ensure that it's not modified during enumeration
                    return this.Tasks.ToArray();
                }
            }
        }

        [Fact]
        public void Issue630_DontFailIfAllTasksAreInlinedInInlinePhase()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var interfaceSource = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var scheduler = new Issue630_TryingAlwaysSatisfyInlineTaskScheduler();

            /*
             * Simulate situation when tasks are always inlined on current thread.
             * To do that we implement our custom scheduler which put some delay before running task.
             * That gives a chance for task to be inlined.
             *
             * Schedulers are propagated to the nested tasks, so we are resolving IInterfaceWithMethod inside the task.
             * All the tasks created during that resolve will be inlined, if that is possible.
             */
            var task = new Task<IInterfaceWithMethod>(() => interfaceSource.Method());
            task.Start(scheduler);

            var instance = task.Result;

            //This test should not fail. Assertion is dummy and to specify that we use instance.
            Assert.NotNull(instance);
        }

        private class InlineOnQueueTaskScheduler : TaskScheduler
        {
            protected override void QueueTask(Task task)
            {
                this.TryExecuteTask(task);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                throw new NotSupportedException("This method should be never reached.");
            }

            protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();
        }

        [Fact]
        public void WithConfigureMembers_DontFailIfAllTasksInlinedOnQueueByCurrentScheduler()
        {
            var task = new Task(() =>
            {
                //arrange
                var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
                var interfaceSource = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

                //act & assert not throw
                var result = interfaceSource.Method();
            });
            task.Start(new InlineOnQueueTaskScheduler());

            task.Wait();
        }

        [Fact]
        public void Issue653_ResolvesPropertyValueViaPropertyRequest()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var subsitute = fixture.Create<IInterfaceWithProperty>();

            var expected = fixture.Create<string>();
            fixture.Customizations.Insert(0,
                new FilteringSpecimenBuilder(
                    new FixedBuilder(expected),
                    new PropertySpecification(typeof(string), nameof(IInterfaceWithProperty.Property))
                ));

            // Act
            var result = subsitute.Property;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Issue707_CanConfigureOutMethodParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithParameterAndOutMethod>();

            var parameter = fixture.Create<string>();
            var result = fixture.Create<int>();

            // Act
            substitute.Method(parameter, out int dummy).Returns(c =>
            {
                c[1] = result;
                return true;
            });

            int actualResult;
            substitute.Method(parameter, out actualResult);

            // Assert
            Assert.Equal(result, actualResult);
        }

        [Theory]
        [InlineData(32), InlineData(64), InlineData(128), InlineData(256)]
        public async Task Issue592_ShouldNotFailForConcurrency(int degreeOfParallelism)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var substitute = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var start = new SemaphoreSlim(0, degreeOfParallelism);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var tasks = Enumerable
                .Repeat(0, degreeOfParallelism)
                .Select(_ => Task.Run(() =>
                    {
                        start.Wait(cts.Token);
                        substitute.Method();
                    },
                    cts.Token));


            // Act
            start.Release(degreeOfParallelism);
            await Task.WhenAll(tasks).ConfigureAwait(false);

            // Assert
            substitute.Received(degreeOfParallelism).Method();
        }
    }
}
