using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using AutoFixture.Kernel;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
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
        public void GenericMethodsReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            var obj = fixture.Create<IInterfaceWithGenericMethod>();
            var result = obj.GenericMethod<string>();

            // Verify outcome
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void StaticMethodsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticMethod>()));
            Assert.NotEqual(frozenString, TypeWithStaticMethod.StaticMethod());
        }

        [Fact]
        public void StaticPropertiesAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticProperty>()));
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
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithConstField>()));
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void StaticFieldsAreIgnored()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticField>()));
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void CircularDependenciesAreAllowed()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => fixture.Create<IInterfaceWithCircularDependency>()));
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
        public void VoidMethodsWithOutParameterIsFilled()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenInt = fixture.Freeze<int>();
            var substitute = fixture.Create<IInterfaceWithOutVoidMethod>();

            int result;
            substitute.Method(out result);

            Assert.Equal(frozenInt, result);
        }

        [Fact]
        public void VoidMethodsWithRefParameterIsFilled()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenInt = fixture.Freeze<int>();
            var substitute = fixture.Create<IInterfaceWithRefVoidMethod>();

            // Exercise system
            int result = 0;
            substitute.Method(ref result);

            // Verify outcome
            Assert.Equal(frozenInt, result);

            // Teardown
        }

        [Fact]
        public void VoidOutMethodsReturnValuesSetup()
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
        public void VoidRefMethodsReturnValuesSetup()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var subsitute = fixture.Create<IInterfaceWithRefVoidMethod>();
            var expected = fixture.Create<int>();

            int origIntValue = -10;
            subsitute.When(x => x.Method(ref origIntValue)).Do(x => x[0] = expected);

            // Exercise system
            int result = -10;
            subsitute.Method(ref result);

            // Verify outcome
            Assert.Equal(expected, result);

            // Teardown
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

        [Fact]
        public void InterfacesImplementingIEnumerableReturnFiniteSequence()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoConfiguredNSubstituteCustomization());
            var repeatCount = fixture.Create<int>();
            fixture.RepeatCount = repeatCount;
            var sut = fixture.Create<IMyList<string>>();

            var result = sut.Take(repeatCount + 1).Count();

            Assert.Equal(repeatCount, result);
        }

        [Fact]
        public void PropertiesOmittingSpecimenReturnsCorrectResult()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            fixture.Customizations.Add(new Omitter(new PropertySpecification(typeof(string), nameof(IInterfaceWithProperty.Property))));
            var sut = fixture.Create<IInterfaceWithProperty>();

            var result = sut.Property;

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MethodsOmittingSpecimenReturnsCorrectResult()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var anonymousString = fixture.Create<string>();
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof(string))));
            var sut = fixture.Create<IInterfaceWithMethod>();

            var result = sut.Method(anonymousString);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void RefMethodsOmittingSpecimenReturnsCorrectResult()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            var intValue = fixture.Create<int>();
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof(int))));
            var sut = fixture.Create<IInterfaceWithRefIntMethod>();

            var refResult = intValue;
            var result = sut.Method(ref refResult);

            Assert.Equal(frozenString, result);
            Assert.Equal(intValue, refResult);
        }

        [Fact]
        public void OutMethodsOmittingSpecimenReturnsCorrectResult()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var intValue = fixture.Create<int>();
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof(int))));
            var sut = fixture.Create<IInterfaceWithOutMethod>();

            var refResult = intValue;
            sut.Method(out refResult);

            Assert.Equal(intValue, refResult);
        }

        [Fact]
        public void SetupCallsOverrideValueFromFixtureWithAnyArgumentMatcher()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var expected = fixture.Create<string>();
            substitute.Method(Arg.Any<string>()).Returns(expected);

            var result = substitute.Method(anonymousString);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetupCallsOverrideValueFromFixtureWithIsArgumentMatcher()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            var anonymousString = fixture.Create<string>();
            var expected = fixture.Create<string>();
            substitute.Method(Arg.Is<string>(_ => true)).Returns(expected);

            var result = substitute.Method(anonymousString);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void InterfaceImplementingAnotherReturnsValueFromFixture()
        {
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expected = fixture.Freeze<int>();
            var sut = fixture.Create<IInterfaceImplementingAnother>();

            var result = sut.Method();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenericMethodsWithRefReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expectedInt = fixture.Freeze<int>();
            var expectedStr = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericRefMethod>();

            // Exercise system
            string refValue = "dummy";
            int retValue = substitute.GenericMethod<string>(ref refValue);

            // Verify outcome
            Assert.Equal(expectedInt, retValue);
            Assert.Equal(expectedStr, refValue);

            // Teardown
        }

        [Fact]
        public void GenericMethodsWithOutReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expectedInt = fixture.Freeze<int>();
            var expectedStr = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericOutMethod>();

            // Exercise system
            string outvalue;
            int retValue = substitute.GenericMethod<string>(out outvalue);

            // Verify outcome
            Assert.Equal(expectedInt, retValue);
            Assert.Equal(expectedStr, outvalue);

            // Teardown
        }

        [Fact]
        public void VoidGenericMethodsWithRefReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expected = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericRefVoidMethod>();

            // Exercise system
            string refValue = "dummy";
            substitute.GenericMethod<string>(ref refValue);

            // Verify outcome
            Assert.Equal(expected, refValue);
            // Teardown
        }

        [Fact]
        public void VoidGenericMethodsWithOutReturnValueFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expected = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericOutVoidMethod>();

            // Exercise system
            string outValue;
            substitute.GenericMethod<string>(out outValue);

            // Verify outcome
            Assert.Equal(expected, outValue);
            // Teardown
        }

        [Fact]
        public void ReturnValueIsSameForSameArgumentForGenerics()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();

            var inputValue = 42;

            // Exercise system
            var result1 = substitute.GenericMethod<int>(inputValue);
            var result2 = substitute.GenericMethod<int>(inputValue);

            // Verify outcome
            Assert.Equal(result1, result2);

            // Teardown
        }

        [Fact]
        public void ReturnValueIsDifferentForDifferentArgumentForGenerics()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();

            // Exercise system
            var result1 = substitute.GenericMethod<int>(42);
            var result2 = substitute.GenericMethod<int>(10);

            // Verify outcome
            Assert.NotEqual(result1, result2);

            // Teardown
        }

        [Fact]
        public void ResultIsDifferentForDifferentGeneticInstantiations()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericMethod>();

            // Exercise system
            var intResult = substitute.GenericMethod<int>();
            var longResult = substitute.GenericMethod<long>();

            // Verify outcome
            Assert.NotEqual(intResult, longResult);

            // Teardown
        }

        [Fact]
        public void CachedResultIsMatchedByOtherInterfaceSubstituteForGenerics()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();
            var arg = fixture.Create<IInterfaceWithMethod>();

            // Exercise system
            int result1 = substitute.GenericMethod<IInterfaceWithMethod>(arg);
            int result2 = substitute.GenericMethod<IInterfaceWithMethod>(arg);

            // Verify outcome
            Assert.Equal(result1, result2);

            // Teardown
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
        public void ShouldNotResetCachedValuesOnSubsituteClear(ClearOptions options)
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethod>();
            string arg = "argValue";

            // Exercise system
            var resultBefore = substitute.Method(arg);
            substitute.ClearSubstitute(options);
            var resultAfter = substitute.Method(arg);

            // Verify outcome
            Assert.Equal(resultBefore, resultAfter);

            // Teardown
        }

        /// <summary>
        /// Current implementation of NSubsitute doesn't call custom handlers for the Received.InOrder() scope (which
        /// we use for our integration with NSubstitute). That shouldsn't cause any usability issues for users.
        ///  
        /// Asserting that behavior via test to get a notification when that behavior changes, so we can make a decision
        /// whether we need to alter something in AF or not to respect that change.
        /// </summary>
        [Fact]
        public void IsNotExpectedToReturnValueInReceivedInOrderBlock()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var actualResult = substitute.Method();

            // Exercise system
            IInterfaceWithMethod capturedResult = null;
            Received.InOrder(() =>
            {
                capturedResult = substitute.Method();
            });

            // Verify outcome
            Assert.NotEqual(actualResult, capturedResult);

            // Teardown
        }

        [Fact]
        public void Issue630_DontFailIfAllTasksAreInlinedInInlinePhase()
        {
            //Test for the following issue fix: https://github.com/AutoFixture/AutoFixture/issues/630

            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var interfaceSource = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var scheduler = new TryingAlwaysSatisfyInlineTaskScheduler();

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

        [Fact]
        public void DontFailIfAllTasksInlinedOnQueueByCurrentScheduler()
        {
            var task = new Task(() =>
            {
                //arrange
                var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var subsitute = fixture.Create<IInterfaceWithProperty>();

            var expected = fixture.Create<string>();
            fixture.Customizations.Insert(0, 
                new FilteringSpecimenBuilder(
                    new FixedBuilder(expected),
                    new PropertySpecification(typeof(string), nameof(IInterfaceWithProperty.Property))
                ));

            // Exercise system
            var result = subsitute.Property;

            // Verify outcome
            Assert.Equal(expected, result);

            // Teardown
        }

        [Fact]
        public void Issue707_CanConfigureOutMethodParameters()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithParameterAndOutMethod>();

            var parameter = fixture.Create<string>();
            var result = fixture.Create<int>();

            // Exercise system
            substitute.Method(parameter, out int dummy).Returns(c => { c[1] = result; return true; });

            int actualResult;
            substitute.Method(parameter, out actualResult);

            // Verify outcome
            Assert.Equal(result, actualResult);

            // Teardown
        }

        [Theory]
        [InlineData(32), InlineData(64), InlineData(128), InlineData(256)]
        public async Task Issue592_ShouldNotFailForConcurrency(int degreeOfParallelism)
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var start = new SemaphoreSlim(0, degreeOfParallelism);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            var tasks = Enumerable
                .Range(0, degreeOfParallelism)
                .Select(_ => Task.Run
                (async () =>
                    {
                        await start.WaitAsync(cts.Token).ConfigureAwait(false);
                        substitute.Method();
                    },
                    cts.Token));


            // Exercise system
            start.Release(degreeOfParallelism);
            await Task.WhenAll(tasks).ConfigureAwait(false);

            // Verify outcome
            substitute.Received(degreeOfParallelism).Method();

            // Teardown
        }

        public interface IMyList<out T> : IEnumerable<T>
        {
        }

        public interface IInterface
        {
        }

        public interface IInterfaceImplementingAnother : IInterface
        {
            int Method();
        }

        private class TryingAlwaysSatisfyInlineTaskScheduler : TaskScheduler
        {
            private const int DELAY_MSEC = 100;
            private readonly object _syncRoot = new object();
            private HashSet<Task> Tasks { get; } = new HashSet<Task>();

            protected override void QueueTask(Task task)
            {
                lock (this._syncRoot)
                {
                    this.Tasks.Add(task);
                }

                ThreadPool.QueueUserWorkItem(delegate
                {
                    Thread.Sleep(DELAY_MSEC);

                    //If task cannot be dequeued - it was already executed.
                    if (this.TryDequeue(task))
                    {
                        base.TryExecuteTask(task);
                    }
                });
            }

            protected override bool TryDequeue(Task task)
            {
                lock (this._syncRoot)
                {
                    return this.Tasks.Remove(task);
                }
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {

                //If was queued, try to remove from queue before inlining. Ignore otherwise - it's already executed.
                if (taskWasPreviouslyQueued && !this.TryDequeue(task))
                {
                    return false;
                }

                base.TryExecuteTask(task);
                return true;
            }

            protected override IEnumerable<Task> GetScheduledTasks()
            {
                lock (this._syncRoot)
                {
                    //Create copy to ensure that it's not modified during enumeration
                    return this.Tasks.ToArray();
                }
            }
        }

        private class InlineOnQueueTaskScheduler : TaskScheduler
        {
            protected override void QueueTask(Task task)
            {
                base.TryExecuteTask(task);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                throw new NotSupportedException("This method should be never reached.");
            }

            protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        }


    }
}