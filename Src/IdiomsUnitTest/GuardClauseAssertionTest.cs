using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuardClauseAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new GuardClauseAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrectFromModestConstructor()
        {
            // Fixture setup
            ISpecimenBuilder expectedBuilder = new Fixture();
            var sut = new GuardClauseAssertion(expectedBuilder);
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrectFromGreedyConstructor()
        {
            // Fixture setup
            ISpecimenBuilder expectedBuilder = new Fixture();
            var dummyExpectation = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(expectedBuilder, dummyExpectation);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void BehaviorExpectationIsCorrectFromGreedyConstructor()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            IBehaviorExpectation expected = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(dummyComposer, expected);
            // Exercise system
            IBehaviorExpectation result = sut.BehaviorExpectation;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void DefaultBehaviorExpectationIsCorrect()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GuardClauseAssertion(dummyComposer);
            // Exercise system
            var result = sut.BehaviorExpectation;
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeBehaviorExpectation>(result);
            Assert.True(composite.BehaviorExpectations.OfType<NullReferenceBehaviorExpectation>().Any());
            Assert.True(composite.BehaviorExpectations.OfType<EmptyGuidBehaviorExpectation>().Any());
            // Teardown
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GuardClauseAssertion(dummyComposer);
            // Exercise system and verify outcome
            var property = typeof(SingleParameterType<object>).GetProperty("Parameter");
            Assert.DoesNotThrow(() =>
                sut.Verify(property));
            // Teardown
        }

        [Fact]
        public void VerifyPropertyCorrectlyInvokesBehaviorExpectation()
        {
            // Fixture setup
            var fixture = new Fixture();
            var owner = fixture.Freeze<PropertyHolder<Version>>(c => c.OmitAutoProperties());
            var value = fixture.Freeze<Version>();

            var property = owner.GetType().GetProperty("Property");

            var mockVerified = false;
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var setterCmd = Assert.IsAssignableFrom<PropertySetCommand>(unwrapper.Command);
                    mockVerified = setterCmd.PropertyInfo.Equals(property)
                        && setterCmd.Owner.Equals(owner);
                }
            };
            var sut = new GuardClauseAssertion(fixture, expectation);
            // Exercise system
            sut.Verify(property);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
            // Teardown
        }

        [Theory, ClassData(typeof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectMethod(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var instanceMethod = Assert.IsAssignableFrom<InstanceMethod>(methodCmd.Method);
                    Assert.Equal(method, instanceMethod.Method);
                    Assert.IsAssignableFrom(ownerType, instanceMethod.Owner);
                    Assert.True(parameters.SequenceEqual(instanceMethod.Parameters));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory, ClassData(typeof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectReplacementIndices(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().Where(IsNotEqualsMethod).ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var observedIndices = new List<int>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    observedIndices.Add(replacement.ReplacementIndex);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            var expectedIndices = Enumerable.Range(0, parameters.Length);
            Assert.True(expectedIndices.SequenceEqual(observedIndices));
            // Teardown
        }

        [Theory, ClassData(typeof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectParametersForReplacement(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    Assert.True(replacement.Source.Select(x => x.GetType()).SequenceEqual(parameters.Select(p => p.ParameterType)));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory, ClassData(typeof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectParameterInfo(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().Where(IsNotEqualsMethod).ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var observedParameters = new List<ParameterInfo>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    observedParameters.Add(methodCmd.ParameterInfo);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            Assert.True(parameters.SequenceEqual(observedParameters));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object), typeof(object))]
        [InlineData(typeof(string), typeof(object))]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(Version), typeof(Version))]
        public void VerifyMethodIgnoresEquals(Type type, Type argumentType)
        {
            // Fixture setup
            var method = type.GetMethod("Equals", new[] { argumentType });

            var invoked = false;
            var expectation = new DelegatingBehaviorExpectation { OnVerify = c => invoked = true };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            Assert.False(invoked);
            // Teardown
        }

        [Theory, ClassData(typeof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectMethod(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var ctorMethod = Assert.IsAssignableFrom<ConstructorMethod>(methodCmd.Method);
                    Assert.Equal(ctor, ctorMethod.Constructor);
                    Assert.True(parameters.SequenceEqual(ctorMethod.Parameters));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory, ClassData(typeof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectReplacementIndices(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var observedIndices = new List<int>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    observedIndices.Add(replacement.ReplacementIndex);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome
            var expectedIndices = Enumerable.Range(0, parameters.Length);
            Assert.True(expectedIndices.SequenceEqual(observedIndices));
            // Teardown
        }

        [Theory, ClassData(typeof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectParametersForReplacement(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    Assert.True(replacement.Source.Select(x => x.GetType()).SequenceEqual(parameters.Select(p => p.ParameterType)));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory, ClassData(typeof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectParameterInfo(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var observedParameters = new List<ParameterInfo>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    observedParameters.Add(methodCmd.ParameterInfo);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome
            Assert.True(parameters.SequenceEqual(observedParameters));
            // Teardown
        }

        private static bool IsNotEqualsMethod(MethodInfo method)
        {
            return method.Name != "Equals";
        }

        private class MethodData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { typeof(GuardedMethodHost), 0 };
                yield return new object[] { typeof(GuardedMethodHost), 1 };
                yield return new object[] { typeof(GuardedMethodHost), 2 };
                yield return new object[] { typeof(GuardedMethodHost), 3 };
                yield return new object[] { typeof(GuardedMethodHost), 4 };
                yield return new object[] { typeof(GuardedMethodHost), 5 };
                yield return new object[] { typeof(GuardedMethodHost), 6 };
                yield return new object[] { typeof(GuardedMethodHost), 7 };
                yield return new object[] { typeof(GuardedMethodHost), 8 };
                yield return new object[] { typeof(Version), 0 };
                yield return new object[] { typeof(Version), 1 };
                yield return new object[] { typeof(Version), 2 };
                yield return new object[] { typeof(Version), 3 };
                yield return new object[] { typeof(Version), 4 };
                yield return new object[] { typeof(Version), 5 };
                yield return new object[] { typeof(Version), 6 };
                yield return new object[] { typeof(Version), 7 };
                yield return new object[] { typeof(Version), 8 };
                yield return new object[] { typeof(Version), 9 };
                yield return new object[] { typeof(Version), 10 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class ConstructorData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { typeof(GuardedConstructorHost<object>), 0 };
                yield return new object[] { typeof(GuardedConstructorHost<string>), 0 };
                yield return new object[] { typeof(GuardedConstructorHost<Version>), 0 };
                yield return new object[] { typeof(ConcreteType), 0 };
                yield return new object[] { typeof(ConcreteType), 1 };
                yield return new object[] { typeof(ConcreteType), 2 };
                yield return new object[] { typeof(ConcreteType), 3 };
                yield return new object[] { typeof(ConcreteType), 4 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
