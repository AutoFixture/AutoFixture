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
    [Obsolete]
    public class AutoConfiguredFixtureIntegrationTest
    {
        [Fact]
        public void ParameterlessMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithParameterlessMethod>();
            // Assert
            Assert.Same(frozenString, result.Method());
        }

        [Fact]
        public void PropertiesReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithProperty>();
            // Assert
            Assert.Same(frozenString, result.Property);
        }

        [Fact]
        public void IndexersReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<TypeWithVirtualMembers>();
            // Assert
            Assert.Equal(frozenString, result.VirtualMethod());
            Assert.Equal(frozenString, result.VirtualProperty);
        }

        [Fact]
        public void MethodsWithParametersReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<TypeWithSealedMembers>();
            // Assert
            Assert.Equal(frozenString, result.ExplicitlySealedProperty);
            Assert.Equal(frozenString, result.ImplicitlySealedProperty);
        }

        [Fact]
        public void FieldsAreSetUsingFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<TypeWithPublicField>();
            // Assert
            Assert.Equal(frozenString, result.Field);
        }

        [Fact]
        public void SealedMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<TypeWithSealedMembers>();

            Assert.NotEqual(frozenString, result.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, result.ExplicitlySealedMethod());
        }

        [Fact]
        public void RefMethodsReturnValuesFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<IInterfaceWithRefMethod>();

            string refResult = "";
            string returnValue = result.Method(ref refResult);

            Assert.Equal(frozenString, refResult);
            Assert.Equal(frozenString, returnValue);
        }

        [Fact]
        public void GenericMethodsReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var obj = fixture.Create<IInterfaceWithGenericMethod>();
            var result = obj.GenericMethod<string>();

            // Assert
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void StaticMethodsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticMethod>()));
            Assert.NotEqual(frozenString, TypeWithStaticMethod.StaticMethod());
        }

        [Fact]
        public void StaticPropertiesAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticProperty>()));
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void PrivateFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<TypeWithPrivateField>();

            Assert.NotEqual(frozenString, result.GetPrivateField());
        }

        [Fact]
        public void ReadonlyFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            var result = fixture.Create<TypeWithReadonlyField>();

            Assert.NotEqual(frozenString, result.ReadonlyField);
        }

        [Fact]
        public void LiteralFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithConstField>()));
            Assert.NotEqual(frozenString, TypeWithConstField.ConstField);
        }

        [Fact]
        public void StaticFieldsAreIgnored()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act & Assert
            Assert.Null(Record.Exception(() => fixture.Create<TypeWithStaticField>()));
            Assert.NotEqual(frozenString, TypeWithStaticField.StaticField);
        }

        [Fact]
        public void CircularDependenciesAreAllowed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            // Act & Assert
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
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenInt = fixture.Freeze<int>();
            var substitute = fixture.Create<IInterfaceWithRefVoidMethod>();

            // Act
            int result = 0;
            substitute.Method(ref result);

            // Assert
            Assert.Equal(frozenInt, result);
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
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var subsitute = fixture.Create<IInterfaceWithRefVoidMethod>();
            var expected = fixture.Create<int>();

            int origIntValue = -10;
            subsitute.When(x => x.Method(ref origIntValue)).Do(x => x[0] = expected);

            // Act
            int result = -10;
            subsitute.Method(ref result);

            // Assert
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
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var frozenString = fixture.Freeze<string>();
            // Act
            var result = fixture.Create<IInterfaceWithNewMethod>();
            // Assert
            Assert.Same(frozenString, (result as IInterfaceWithShadowedMethod).Method(0));
        }

        [Fact]
        public void InterfacesImplementingIEnumerableReturnFiniteSequence()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoConfiguredNSubstituteCustomization());
            var repeatCount = fixture.Create<int>();
            fixture.RepeatCount = repeatCount;
            var sut = fixture.Create<IDerivedFromEnumerableInterface<string>>();

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
            var sut = fixture.Create<IDerivedInterfaceWithOwnMethod>();

            var result = sut.IntMethod();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenericMethodsWithRefReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
        public void GenericMethodsWithOutReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expectedInt = fixture.Freeze<int>();
            var expectedStr = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericOutMethod>();

            // Act
            string outvalue;
            int retValue = substitute.GenericMethod<string>(out outvalue);

            // Assert
            Assert.Equal(expectedInt, retValue);
            Assert.Equal(expectedStr, outvalue);
        }

        [Fact]
        public void VoidGenericMethodsWithRefReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expected = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericRefVoidMethod>();

            // Act
            string refValue = "dummy";
            substitute.GenericMethod<string>(ref refValue);

            // Assert
            Assert.Equal(expected, refValue);
        }

        [Fact]
        public void VoidGenericMethodsWithOutReturnValueFromFixture()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var expected = fixture.Freeze<string>();
            var substitute = fixture.Create<IInterfaceWithGenericOutVoidMethod>();

            // Act
            string outValue;
            substitute.GenericMethod<string>(out outValue);

            // Assert
            Assert.Equal(expected, outValue);
        }

        [Fact]
        public void ReturnValueIsSameForSameArgumentForGenerics()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();

            var inputValue = 42;

            // Act
            var result1 = substitute.GenericMethod<int>(inputValue);
            var result2 = substitute.GenericMethod<int>(inputValue);

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void ReturnValueIsDifferentForDifferentArgumentForGenerics()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericParameterMethod>();

            // Act
            var result1 = substitute.GenericMethod<int>(42);
            var result2 = substitute.GenericMethod<int>(10);

            // Assert
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void ResultIsDifferentForDifferentGeneticInstantiations()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithGenericMethod>();

            // Act
            var intResult = substitute.GenericMethod<int>();
            var longResult = substitute.GenericMethod<long>();

            // Assert
            Assert.NotEqual(intResult, longResult);
        }

        [Fact]
        public void CachedResultIsMatchedByOtherInterfaceSubstituteForGenerics()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
        public void ShouldNotResetCachedValuesOnSubsituteClear(ClearOptions options)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
        /// we use for our integration with NSubstitute). That shouldsn't cause any usability issues for users.
        ///  
        /// Asserting that behavior via test to get a notification when that behavior changes, so we can make a decision
        /// whether we need to alter something in AF or not to respect that change.
        /// </summary>
        [Fact]
        public void IsNotExpectedToReturnValueInReceivedInOrderBlock()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithMethodReturningOtherInterface>();

            var actualResult = substitute.Method();

            // Act
            IInterfaceWithMethod capturedResult = null;
            Received.InOrder(() =>
            {
                capturedResult = substitute.Method();
            });

            // Assert
            Assert.NotEqual(actualResult, capturedResult);
        }

        private class Issue630_TryingAlwaysSatisfyInlineTaskScheduler : TaskScheduler
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

        [Fact]
        public void Issue630_DontFailIfAllTasksAreInlinedInInlinePhase()
        {
            //Test for the following issue fix: https://github.com/AutoFixture/AutoFixture/issues/630

            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
                base.TryExecuteTask(task);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                throw new NotSupportedException("This method should be never reached.");
            }

            protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

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
            // Arrange
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
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
            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var substitute = fixture.Create<IInterfaceWithParameterAndOutMethod>();

            var parameter = fixture.Create<string>();
            var result = fixture.Create<int>();

            // Act
            substitute.Method(parameter, out int dummy).Returns(c => { c[1] = result; return true; });

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


            // Act
            start.Release(degreeOfParallelism);
            await Task.WhenAll(tasks).ConfigureAwait(false);

            // Assert
            substitute.Received(degreeOfParallelism).Method();
        }
    }
}