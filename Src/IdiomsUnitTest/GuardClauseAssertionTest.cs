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

        [Theory]
        [InlineData(typeof(ClassWithDeferredNullGuard))]
        [InlineData(typeof(ClassWithDeferredGuidGuard))]
        [InlineData(typeof(ClassWithDeferredGuidGuardReturningEnumerator))]
        public void VerifyMethodWithDeferredGuardThrowsExceptionWithExtraHelpfulMessage(
            Type type)
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            var method = type.GetMethod("GetValues");
            // Exercise system and verify outcome
            var e =
                Assert.Throws<GuardClauseException>(() => sut.Verify(method));
            Assert.Contains("deferred", e.Message);
            // Teardown
        }

        private class ClassWithDeferredNullGuard
        {
            public IEnumerable<string> GetValues(string someString)
            {
                if (someString == null)
                    throw new ArgumentNullException("someString");

                yield return someString;
                yield return someString;
                yield return someString;
            }
        }

        private class ClassWithDeferredGuidGuard
        {
            public IEnumerable<Guid> GetValues(Guid someGuid)
            {
                if (someGuid == null)
                    throw new ArgumentException(
                        "Guid.Empty not allowed.",
                        "someGuid");

                yield return someGuid;
                yield return someGuid;
                yield return someGuid;
            }
        }

        private class ClassWithDeferredGuidGuardReturningEnumerator
        {
            public IEnumerator<Guid> GetValues(Guid someGuid)
            {
                if (someGuid == null)
                    throw new ArgumentException(
                        "Guid.Empty not allowed.",
                        "someGuid");

                yield return someGuid;
                yield return someGuid;
                yield return someGuid;
            }
        }

        [Fact]
        public void VerifyGenericConstructorThrowsHelpfulException()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            // Exercise system and verify outcome
            var e = Assert.Throws<GuardClauseException>(
                () => sut.Verify(
                    typeof(GenericTypeWithNonTrivialConstraint<>).GetConstructors().First()));
            Assert.Contains(
                "generic",
                e.Message,
                StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains("GenericTypeWithNonTrivialConstraint`1", e.Message);
            // Teardown
        }

        [Fact]
        public void VerifyGenericTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () =>
                    sut.Verify(typeof(GenericTypeWithNonTrivialConstraint<>)));

            Assert.Contains(
                "generic type definition", e.Message, StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains("GenericTypeWithNonTrivialConstraint`1", e.Message);
        }

        [Fact]
        public void VerifyMethodOnGenericTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () =>
                    sut.Verify(
                        typeof(GenericTypeWithNonTrivialConstraint<>).GetMethod(
                            "MethodWithParameter")));

            Assert.Contains(
                "generic type definition", e.Message, StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains("GenericTypeWithNonTrivialConstraint`1", e.Message);
        }

        [Fact]
        public void VerifyPropertyOnGenericTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () =>
                    sut.Verify(
                        typeof(GenericTypeWithNonTrivialConstraint<>).GetProperty(
                            "Property")));

            Assert.Contains(
                "generic type definition", e.Message, StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains("GenericTypeWithNonTrivialConstraint`1", e.Message);
        }

        private class GenericTypeWithNonTrivialConstraint<T>
            where T : IHaveNoImplementers
        {
            public GenericTypeWithNonTrivialConstraint(T item)
            {
            }

            public void MethodWithParameter(string s)
            {
            }

            public string Property { get; set; }
        }

        private interface IHaveNoImplementers { }

        [Fact]
        public void VerifyNullConstructorThrows()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Verify((ConstructorInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyMethodWithUnknownParameterTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () => sut.Verify(typeof(TypeWithMethodWithParameterWithoutImplementers)));
            Assert.Contains("parameter", e.Message, StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains("TypeWithMethodWithParameterWithoutImplementers", e.Message);
            Assert.Contains("MethodWithParameterWithoutImplementers", e.Message);
            Assert.IsType<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyTypeWithPropertyOfUnknownTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () => sut.Verify(typeof(TypeWithPropertyOfTypeWithoutImplementers)));
            Assert.Contains("TypeWithPropertyOfTypeWithoutImplementers", e.Message);
            Assert.IsType<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyMethodOnTypeWithPropertyOfUnknownTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () =>
                    sut.Verify(
                        typeof(
                        TypeWithPropertyOfTypeWithoutImplementersAndMethod)
                        .GetMethod("Method")));
            Assert.Contains(
                "TypeWithPropertyOfTypeWithoutImplementersAndMethod", e.Message);
            Assert.IsType<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyPropertyOnTypeWithPropertyOfUnknownTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.Throws<GuardClauseException>(
                    () =>
                    sut.Verify(
                        typeof(
                        TypeWithPropertyOfTypeWithoutImplementersAndMethod)
                        .GetProperty("Property")));
            Assert.Contains(
                "TypeWithPropertyOfTypeWithoutImplementersAndMethod", e.Message);
            Assert.IsType<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyConstructorOnTypeWithPropertyOfUnknownTypeDoesNotThrow()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            Assert.DoesNotThrow(
                () =>
                sut.Verify(
                    typeof(TypeWithPropertyOfTypeWithoutImplementersAndMethod)
                    .GetConstructors().First()));
        }

        [Fact]
        public void VerifyPropertyOfUnknownTypeDoesNotThrowWhenAutoPropertiesAreOmitted()
        {
            var fixture = new Fixture();
            fixture.Customize<TypeWithPropertyOfTypeWithoutImplementers>(
                x => x.OmitAutoProperties());
            var sut = new GuardClauseAssertion(fixture);

            Assert.DoesNotThrow(
                () =>
                sut.Verify(
                    typeof(TypeWithPropertyOfTypeWithoutImplementers)
                    .GetProperty("PropertyOfTypeWithoutImplementers")));
        }

        private class TypeWithMethodWithParameterWithoutImplementers
        {
            public void MethodWithParameterWithoutImplementers(
                IHaveNoImplementers parameter, string other)
            {
            }
        }

        private class TypeWithPropertyOfTypeWithoutImplementers
        {
            private IHaveNoImplementers _propertyOfTypeWithoutImplementers;
            public IHaveNoImplementers PropertyOfTypeWithoutImplementers
            {
                get { return this._propertyOfTypeWithoutImplementers; }
                set
                {
                    if(value == null)
                        throw new ArgumentNullException("value");
                    this._propertyOfTypeWithoutImplementers = value;
                }
            }
        }

        private class TypeWithPropertyOfTypeWithoutImplementersAndMethod
        {
            public void Method()
            {
            }

            public IHaveNoImplementers PropertyOfTypeWithoutImplementers { get; set; }
            public int Property { get; set; }
        }

        [Fact]
        public void VerifyStaticPropertyOnNonStaticClassThrowsHelpfulException()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            var staticProperty = typeof(StaticPropertyHolder<object>).GetProperty("Property");
            Assert.NotNull(staticProperty);
            // Exercise system & Verify outcome
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticProperty));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyStaticPropertyOnStaticTypeThrowsHelpfulException()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            var staticProperty = typeof(UngardedStaticPropertyOnStaticTypeHost).GetProperty("Property");
            Assert.NotNull(staticProperty);
            // Exercise system & Verify outcome
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticProperty));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyStaticMethodOnNonStaticTypeThrowsHelpfulException()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            var staticMethod = typeof(StaticPropertyHolder<object>).GetProperty("Property").GetSetMethod();
            Assert.NotNull(staticMethod);
            // Exercise system & Verify outcome
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticMethod));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyStaticMethodOnStaticTypeThrowsHelpfulException()
        {
            // Fixture setup
            var sut = new GuardClauseAssertion(new Fixture());
            var staticMethod = typeof(UngardedStaticMethodOnStaticTypeHost).GetMethod("Method");
            Assert.NotNull(staticMethod);
            // Exercise system & Verify outcome
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticMethod));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }
    }
}
