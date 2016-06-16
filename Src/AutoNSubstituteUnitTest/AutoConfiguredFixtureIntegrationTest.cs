using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
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
            fixture.Customizations.Add(new Omitter(new ExactTypeSpecification(typeof (string))));
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
        public void Issue630_DontFailIfAllTasksAreInlined()
        {
            //Test for the following issue fix: https://github.com/AutoFixture/AutoFixture/issues/630

            var fixture = new Fixture().Customize(new AutoConfiguredNSubstituteCustomization());
            var interfaceSource = fixture.Create<IInterfaceWithMethodSource>();

            var scheduler = new AggressiveInliningTaskScheduler();

            /*
             * Simulate situation when tasks are always inlined on current thread.
             * To do that we implement our custom scheduler which put some delay before running task.
             * That gives a chance for task to be inlined.
             * 
             * Schedulers are propagated to the nested tasks, so we are resolving IInterfaceWithMethod inside the task.
             * All the tasks created during that resolve will be inlined, if that is possible.
             */
            var task = new Task<IInterfaceWithMethod>(() => interfaceSource.Get());
            task.Start(scheduler);

            var instance = task.Result;

            //This test should not fail. Assertion is dummy and to specify that we use instance.
            Assert.NotNull(instance);
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

        public interface IInterfaceWithMethodSource
        {
            IInterfaceWithMethod Get();
        }

        public class AggressiveInliningTaskScheduler : TaskScheduler
        {
            private const int DELAY_MSEC = 200;
            private readonly object _syncRoot = new object();
            private HashSet<Task> Tasks { get; } = new HashSet<Task>();

            protected override void QueueTask(Task task)
            {
                lock (_syncRoot)
                {
                    Tasks.Add(task);
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
                lock (_syncRoot)
                {
                    return Tasks.Remove(task);
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
                lock (_syncRoot)
                {
                    //Create copy to ensure that it's not modified during enumeration
                    return Tasks.ToArray();
                }
            }
        }
    }
}