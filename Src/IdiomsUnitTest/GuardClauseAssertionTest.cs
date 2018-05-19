using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class GuardClauseAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new GuardClauseAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrectFromModestConstructor()
        {
            // Arrange
            ISpecimenBuilder expectedBuilder = new Fixture();
            var sut = new GuardClauseAssertion(expectedBuilder);
            // Act
            var result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void ComposerIsCorrectFromGreedyConstructor()
        {
            // Arrange
            ISpecimenBuilder expectedBuilder = new Fixture();
            var dummyExpectation = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(expectedBuilder, dummyExpectation);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void BehaviorExpectationIsCorrectFromGreedyConstructor()
        {
            // Arrange
            var dummyComposer = new Fixture();
            IBehaviorExpectation expected = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(dummyComposer, expected);
            // Act
            IBehaviorExpectation result = sut.BehaviorExpectation;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DefaultBehaviorExpectationIsCorrect()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new GuardClauseAssertion(dummyComposer);
            // Act
            var result = sut.BehaviorExpectation;
            // Assert
            var composite = Assert.IsAssignableFrom<CompositeBehaviorExpectation>(result);
            Assert.True(composite.BehaviorExpectations.OfType<NullReferenceBehaviorExpectation>().Any());
            Assert.True(composite.BehaviorExpectations.OfType<EmptyGuidBehaviorExpectation>().Any());
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new GuardClauseAssertion(dummyComposer);
            // Act & Assert
            var property = typeof(SingleParameterType<object>).GetProperty("Parameter");
            Assert.Null(Record.Exception(() =>
                sut.Verify(property)));
        }

        [Fact]
        public void VerifyPropertyCorrectlyInvokesBehaviorExpectation()
        {
            // Arrange
            var fixture = new Fixture();
            var owner = fixture.Freeze<PropertyHolder<Version>>(c => c.OmitAutoProperties());
            fixture.Freeze<Version>();

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
            // Act
            sut.Verify(property);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
        }

        [Theory, MemberData(nameof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectMethod(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var method = methodRef.Member;
            var parameters = method.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var instanceMethod = Assert.IsAssignableFrom<InstanceMethod>(methodCmd.Method);
                    Assert.Equal(method, instanceMethod.Method);
                    Assert.IsAssignableFrom(method.DeclaringType, instanceMethod.Owner);
                    Assert.True(parameters.SequenceEqual(instanceMethod.Parameters));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Act
            sut.Verify(method);
            // Assert (done by mock)
        }

        [Theory, MemberData(nameof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectReplacementIndices(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var method = methodRef.Member;
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
            // Act
            sut.Verify(method);
            // Assert
            var expectedIndices = Enumerable.Range(0, parameters.Length).ToArray();
            Assert.Equal(expectedIndices, observedIndices);
        }

        [Theory, MemberData(nameof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectParametersForReplacement(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var method = methodRef.Member;
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
            // Act
            sut.Verify(method);
            // Assert (done by mock)
        }

        [Theory, MemberData(nameof(MethodData))]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectParameterInfo(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var method = methodRef.Member;
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
            // Act
            sut.Verify(method);
            // Assert
            Assert.Equal(parameters, observedParameters);
        }

        [Theory]
        [InlineData(typeof(object), typeof(object))]
        [InlineData(typeof(string), typeof(object))]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(Version), typeof(Version))]
        public void VerifyMethodIgnoresEquals(Type type, Type argumentType)
        {
            // Arrange
            var method = type.GetMethod("Equals", new[] { argumentType });

            var invoked = false;
            var expectation = new DelegatingBehaviorExpectation { OnVerify = c => invoked = true };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Act
            sut.Verify(method);
            // Assert
            Assert.False(invoked);
        }

        [Theory, MemberData(nameof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectMethod(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var ctor = ctorRef.Member;
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
            // Act
            sut.Verify(ctor);
            // Assert (done by mock)
        }

        [Theory, MemberData(nameof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectReplacementIndices(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var ctor = ctorRef.Member;
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
            // Act
            sut.Verify(ctor);
            // Assert
            var expectedIndices = Enumerable.Range(0, parameters.Length).ToArray();
            Assert.Equal(expectedIndices, observedIndices);
        }

        [Theory, MemberData(nameof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectParametersForReplacement(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var ctor = ctorRef.Member;
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
            // Act
            sut.Verify(ctor);
            // Assert (done by mock)
        }

        [Theory, MemberData(nameof(ConstructorData))]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectParameterInfo(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var ctor = ctorRef.Member;
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
            // Act
            sut.Verify(ctor);
            // Assert
            Assert.Equal(parameters, observedParameters);
        }

        public static TheoryData<MemberRef<MethodInfo>> MethodData => new TheoryData<MemberRef<MethodInfo>>
        {
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeString)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeInt32)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeGuid)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeStringAndInt32)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeStringAndGuid)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeInt32AndGuid)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ConsumeStringAndInt32AndGuid)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.ToString)),
            MemberRef.MethodByName(typeof(GuardedMethodHost), nameof(GuardedMethodHost.GetHashCode)),
            MemberRef.MethodByIndex(typeof(Version), 0),
            MemberRef.MethodByIndex(typeof(Version), 1),
            MemberRef.MethodByIndex(typeof(Version), 2),
            MemberRef.MethodByIndex(typeof(Version), 3),
            MemberRef.MethodByIndex(typeof(Version), 4),
            MemberRef.MethodByIndex(typeof(Version), 5),
            MemberRef.MethodByIndex(typeof(Version), 6),
            MemberRef.MethodByIndex(typeof(Version), 7),
            MemberRef.MethodByIndex(typeof(Version), 8),
            MemberRef.MethodByIndex(typeof(Version), 9),
            MemberRef.MethodByIndex(typeof(Version), 10),
        };

        public static TheoryData<MemberRef<ConstructorInfo>> ConstructorData => new TheoryData<MemberRef<ConstructorInfo>>
        {
            MemberRef.CtorByArgs(typeof(GuardedConstructorHost<object>), new[] { typeof(object) }),
            MemberRef.CtorByArgs(typeof(GuardedConstructorHost<string>), new[] { typeof(string) }),
            MemberRef.CtorByArgs(typeof(GuardedConstructorHost<Version>), new[] { typeof(Version) }),
            MemberRef.CtorByArgs(typeof(ConcreteType), Type.EmptyTypes),
            MemberRef.CtorByArgs(typeof(ConcreteType), new[] { typeof(object) }),
            MemberRef.CtorByArgs(typeof(ConcreteType), new[] { typeof(object), typeof(object) }),
            MemberRef.CtorByArgs(typeof(ConcreteType), new[] { typeof(object), typeof(object), typeof(object) }),
            MemberRef.CtorByArgs(typeof(ConcreteType), new[] { typeof(object), typeof(object), typeof(object), typeof(object) })
        };

        [Theory]
        [InlineData(typeof(ClassWithDeferredNullGuard))]
        [InlineData(typeof(ClassWithDeferredGuidGuard))]
        [InlineData(typeof(ClassWithDeferredGuidGuardReturningEnumerator))]
        public void VerifyMethodWithDeferredGuardThrowsExceptionWithExtraHelpfulMessage(
            Type type)
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var method = type.GetMethod("GetValues");
            // Act & Assert
            var e =
                Assert.Throws<GuardClauseException>(() => sut.Verify(method));
            Assert.Contains("deferred", e.Message);
        }

        private class ClassWithDeferredNullGuard
        {
            public IEnumerable<string> GetValues(string someString)
            {
                if (someString == null)
                    throw new ArgumentNullException(nameof(someString));

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
                        nameof(someGuid));

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
                        nameof(someGuid));

                yield return someGuid;
                yield return someGuid;
                yield return someGuid;
            }
        }

        [Theory]
        [InlineData(typeof(ClassWithEnumerableNonDeferredArraryMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericListMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericCollectionMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericDictionaryMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericReadOnlyCollectionMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericIListMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericICollectionMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredGenericIDictionaryMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredIListMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredICollectionMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredIDictionaryMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredArrayListMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredStackMissingGuard))]
        [InlineData(typeof(ClassWithEnumerableNonDeferredReadOnlyCollectionBaseMissingGuard))]
        public void VerifyMethodWithNonDeferredMissingGuardThrowsExceptionWithoutDeferredMessage(
            Type type)
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var method = type.GetMethod("GetValues");
            // Act & Assert
            var e =
                Assert.Throws<GuardClauseException>(() => sut.Verify(method));
            Assert.DoesNotContain("deferred", e.Message);
        }

        private class ClassWithEnumerableNonDeferredIListMissingGuard
        {
            public IList GetValues(string someString)
            {
                return new ArrayList { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredArrayListMissingGuard
        {
            public ArrayList GetValues(string someString)
            {
                return new ArrayList { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredStackMissingGuard
        {
            public Stack GetValues(string someString)
            {
                return new Stack(new[] { someString, someString, someString });
            }
        }

        private class ClassWithEnumerableNonDeferredReadOnlyCollectionBaseMissingGuard
        {
            class ReadOnlyCollection : ReadOnlyCollectionBase
            {
                public ReadOnlyCollection(params object[] items)
                {
                    this.InnerList.AddRange(items);
                }
            }

            public ReadOnlyCollectionBase GetValues(string someString)
            {
                return new ReadOnlyCollection(someString, someString, someString);
            }
        }

        private class ClassWithEnumerableNonDeferredICollectionMissingGuard
        {
            public ICollection GetValues(string someString)
            {
                return new Stack(new[] { someString, someString });
            }
        }

        private class ClassWithEnumerableNonDeferredIDictionaryMissingGuard
        {
            public IDictionary GetValues(string someString)
            {
                return new System.Collections.Specialized.HybridDictionary
                {
                    { "uniqueKey1", someString },
                    { "uniqueKey2", someString },
                    { "uniqueKey3", someString }
                };
            }
        }

        private class ClassWithEnumerableNonDeferredGenericICollectionMissingGuard
        {
            public ICollection<string> GetValues(string someString)
            {
                return new Collection<string> { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredGenericIDictionaryMissingGuard
        {
            public IDictionary<string, object> GetValues(string someString)
            {
                return new Dictionary<string, object>
                {
                    { "uniqueKey1", someString },
                    { "uniqueKey2", someString }
                };
            }
        }

        private class ClassWithEnumerableNonDeferredGenericIListMissingGuard
        {
            public IList<string> GetValues(string someString)
            {
                return new List<string> { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredArraryMissingGuard
        {
            public string[] GetValues(string someString)
            {
                return new[] { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredGenericListMissingGuard
        {
            public List<string> GetValues(string someString)
            {
                return new List<string> { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredGenericCollectionMissingGuard
        {
            public Collection<string> GetValues(string someString)
            {
                return new Collection<string> { someString, someString, someString };
            }
        }

        private class ClassWithEnumerableNonDeferredGenericReadOnlyCollectionMissingGuard
        {
            public ReadOnlyCollection<string> GetValues(string someString)
            {
                return new ReadOnlyCollection<string>(new[] {someString, someString, someString });
            }
        }

        private class ClassWithEnumerableNonDeferredGenericDictionaryMissingGuard
        {
            public Dictionary<string, string> GetValues(string someString)
            {
                return new Dictionary<string, string> 
                {
                    { "uniqueKey1", someString },
                    { "uniqueKey2", someString }
                };
            }
        }

        private interface IHaveNoImplementers { }

        [Fact]
        public void VerifyNullConstructorThrows()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Verify((ConstructorInfo)null));
        }

        [Fact]
        public void VerifyMethodWithUnknownParameterTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.ThrowsAny<GuardClauseException>(
                    () => sut.Verify(typeof(TypeWithMethodWithParameterWithoutImplementers)));
            Assert.Contains("parameter", e.Message, StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains(nameof(TypeWithMethodWithParameterWithoutImplementers), e.Message);
            Assert.Contains(nameof(TypeWithMethodWithParameterWithoutImplementers.MethodWithParameterWithoutImplementers), e.Message);
            Assert.IsAssignableFrom<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyTypeWithPropertyOfUnknownTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.ThrowsAny<GuardClauseException>(
                    () => sut.Verify(typeof(TypeWithPropertyOfTypeWithoutImplementers)));
            Assert.Contains(nameof(TypeWithPropertyOfTypeWithoutImplementers), e.Message);
            Assert.IsAssignableFrom<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyMethodOnTypeWithPropertyOfUnknownTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.ThrowsAny<GuardClauseException>(
                    () =>
                    sut.Verify(
                        typeof(TypeWithPropertyOfTypeWithoutImplementersAndMethod)
                        .GetMethod(nameof(TypeWithPropertyOfTypeWithoutImplementersAndMethod.Method))));
            Assert.Contains(nameof(TypeWithPropertyOfTypeWithoutImplementersAndMethod), e.Message);
            Assert.IsAssignableFrom<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyPropertyOnTypeWithPropertyOfUnknownTypeThrowsHelpfulException()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            var e =
                Assert.ThrowsAny<GuardClauseException>(
                    () =>
                    sut.Verify(
                        typeof(TypeWithPropertyOfTypeWithoutImplementersAndMethod)
                        .GetProperty(nameof(TypeWithPropertyOfTypeWithoutImplementersAndMethod.Property))));
            Assert.Contains(nameof(TypeWithPropertyOfTypeWithoutImplementersAndMethod), e.Message);
            Assert.IsAssignableFrom<ObjectCreationException>(e.InnerException);
        }

        [Fact]
        public void VerifyConstructorOnTypeWithPropertyOfUnknownTypeDoesNotThrow()
        {
            var sut = new GuardClauseAssertion(new Fixture());

            Assert.Null(
                Record.Exception(() =>
                sut.Verify(
                    typeof(TypeWithPropertyOfTypeWithoutImplementersAndMethod)
                    .GetConstructors().First())));
        }

        [Fact]
        public void VerifyPropertyOfUnknownTypeDoesNotThrowWhenAutoPropertiesAreOmitted()
        {
            var fixture = new Fixture();
            fixture.Customize<TypeWithPropertyOfTypeWithoutImplementers>(
                x => x.OmitAutoProperties());
            var sut = new GuardClauseAssertion(fixture);

            Assert.Null(
                Record.Exception(() =>
                sut.Verify(
                    typeof(TypeWithPropertyOfTypeWithoutImplementers)
                    .GetProperty(nameof(TypeWithPropertyOfTypeWithoutImplementers.PropertyOfTypeWithoutImplementers)))));
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
            private IHaveNoImplementers propertyOfTypeWithoutImplementers;
            public IHaveNoImplementers PropertyOfTypeWithoutImplementers
            {
                get => this.propertyOfTypeWithoutImplementers;
                set => this.propertyOfTypeWithoutImplementers = value ?? throw new ArgumentNullException(nameof(value));
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
        public void VerifyUnguardedStaticPropertyOnNonStaticClassThrowsHelpfulException()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var staticProperty = typeof(StaticPropertyHolder<object>).GetProperty(nameof(StaticPropertyHolder<object>.Property));
            Assert.NotNull(staticProperty);
            // Act & Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticProperty));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyUnguardedStaticPropertyOnStaticTypeThrowsHelpfulException()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var staticProperty = typeof(UnguardedStaticPropertyOnStaticTypeHost).GetProperty(nameof(UnguardedStaticPropertyOnStaticTypeHost.Property));
            Assert.NotNull(staticProperty);
            // Act & Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticProperty));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyUnguardedStaticMethodOnNonStaticTypeThrowsHelpfulException()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var staticMethod = typeof(StaticPropertyHolder<object>).GetProperty(nameof(StaticPropertyHolder<object>.Property)).GetSetMethod();
            Assert.NotNull(staticMethod);
            // Act & Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticMethod));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyUnguardedStaticMethodOnStaticTypeThrowsHelpfulException()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var staticMethod = typeof(UnguardedStaticMethodOnStaticTypeHost).GetMethod(nameof(UnguardedStaticMethodOnStaticTypeHost.Method));
            Assert.NotNull(staticMethod);
            // Act & Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(staticMethod));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyGuardedStaticMethodOnStaticTypeDoesNotThrow()
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var staticMethods = typeof(GuardedStaticMethodOnStaticTypeHost).GetMethods();
            Assert.NotEmpty(staticMethods);
            Assert.Null(Record.Exception(() => sut.Verify(staticMethods)));
        }

        [Theory, MemberData(nameof(ConstructorsOnGuardedOpenGenericTypes))]
        public void VerifyConstructorOnGuardedGenericDoesNotThrow(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var constructor = ctorRef.Member;
            var sut = new GuardClauseAssertion(new Fixture());
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(constructor)));
        }

        [Theory, MemberData(nameof(PropertiesOnGuardedOpenGenericTypes))]
        public void VerifyPropertyOnGuardedGenericDoesNotThrow(MemberRef<PropertyInfo> propertyRef)
        {
            // Arrange
            var propertyInfo = propertyRef.Member;
            var sut = new GuardClauseAssertion(new Fixture());
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(propertyInfo)));
        }

        [Theory, MemberData(nameof(MethodsOnGuardedOpenGenericTypes))]
        public void VerifyMethodOnGuardedGenericDoesNotThrow(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var methodInfo = methodRef.Member;
            var sut = new GuardClauseAssertion(new Fixture());
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(methodInfo)));
        }

        [Theory, MemberData(nameof(ConstructorsOnUnguardedOpenGenericTypes))]
        public void VerifyConstructorOnUnguardedGenericThrows(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var constructorInfo = ctorRef.Member;
            var sut = new GuardClauseAssertion(new Fixture { OmitAutoProperties = true });
            // Act
            // Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(constructorInfo));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Theory, MemberData(nameof(PropertiesOnUnguardedOpenGenericTypes))]
        public void VerifyPropertyOnUnguardedGenericThrows(MemberRef<PropertyInfo> propertyRef)
        {
            // Arrange
            var propertyInfo = propertyRef.Member;
            var sut = new GuardClauseAssertion(new Fixture { OmitAutoProperties = true });
            // Act
            // Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(propertyInfo));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Theory, MemberData(nameof(MethodsOnUnguardedOpenGenericTypes))]
        public void VerifyMethodOnUnguardedGenericThrows(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var methodInfo = methodRef.Member;
            var sut = new GuardClauseAssertion(new Fixture { OmitAutoProperties = true });
            // Act
            // Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(methodInfo));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        [Fact]
        public void VerifyMethodOnGenericManyTimeLoadsOnlyUniqueAssemblies()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            MethodInfo methodInfo = typeof(NoContraint<>).GetMethod(nameof(NoContraint<object>.Method));

            var assembliesBefore = AppDomain.CurrentDomain.GetAssemblies();

            // Act
            sut.Verify(methodInfo);
            sut.Verify(methodInfo);
            sut.Verify(methodInfo);
            // Assert

            var assembliesAfter = AppDomain.CurrentDomain.GetAssemblies();

            Assert.Equal(assembliesBefore, assembliesAfter);
        }

        [Fact]
        public void DynamicDummyTypeIfVoidMethodIsCalledDoesNotThrows()
        {
            // Arrange
            bool mockVerification = false;
            var behaviorExpectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var dynamicInstance = (IDynamicInstanceTestType)GetParameters(c).ElementAt(0);
                    Assert.Null(Record.Exception(() => dynamicInstance.VoidMethod(null, 123)));
                    Assert.Null(Record.Exception(() => { dynamicInstance.Property = new object(); }));
                    mockVerification = true;
                }
            };
            var sut = new GuardClauseAssertion(new Fixture(), behaviorExpectation);
            var methodInfo = typeof(DynamicInstanceTestConstraint<>).GetMethod("Method");
            // Act
            sut.Verify(methodInfo);
            // Assert
            Assert.True(mockVerification, "mock verification.");
        }

        private static IEnumerable<object> GetParameters(IGuardClauseCommand commmand)
        {
            var methodInvokeCommand = (MethodInvokeCommand)((ReflectionExceptionUnwrappingCommand)commmand).Command;
            var indexedReplacement = (IndexedReplacement<object>)methodInvokeCommand.Expansion;
            return indexedReplacement.Source;
        }

        [Fact]
        public void DynamicDummyTypeIfReturnMethodIsCalledReturnsAnonymousValue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var objectValue = fixture.Freeze<object>();
            var intValue = fixture.Freeze<int>();

            bool mockVerification = false;
            var behaviorExpectation = new DelegatingBehaviorExpectation()
            {
                OnVerify = c =>
                {
                    var dynamicInstance = (IDynamicInstanceTestType)GetParameters(c).ElementAt(0);
                    Assert.Equal(objectValue, dynamicInstance.Property);
                    Assert.Equal(intValue, dynamicInstance.ReturnMethod(null, 123));
                    mockVerification = true;
                }
            };
            
            var sut = new GuardClauseAssertion(fixture, behaviorExpectation);
            var methodInfo = typeof(DynamicInstanceTestConstraint<>).GetMethod("Method");
            // Act
            sut.Verify(methodInfo);
            // Assert
            Assert.True(mockVerification, "mock verification.");
        }

        [Fact]
        public void VerifyUnguardedConstructorOnGenericHavingNoAccessibleConstructorGenericArgumentThrows()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var constructorInfo = typeof(NoAccessibleConstructorTestConstraint<>).GetConstructors().Single();
            // Act
            // Assert
            var e = Assert.Throws<ArgumentException>(() => sut.Verify(constructorInfo));
            Assert.Equal(
                "Cannot create a dummy type because the base type " +
                "'AutoFixture.IdiomsUnitTest.GuardClauseAssertionTest+NoAccessibleConstructorTestType' " +
                "does not have any accessible constructor.",
                e.Message);
        }

        [Fact]
        public void VerifyIgnoresGuardAssertionForOutParameter()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var methodInfo = typeof(OutParameterTestType).GetMethod("Method");
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(methodInfo)));
        }

        [Fact]
        public void VerifyOnTaskDeferredGuardThrowsExceptionWithExtraHelpfulMessage()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());

            var theMethod = from m in new Albedo.Methods<AsyncHost>()
                            select m.TaskWithInnerGuardClause(null);
            // Act
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(theMethod));
            // Assert
            Assert.Contains("Task", e.Message);
            Assert.Contains("async", e.Message);
        }

        [Fact]
        public void VerifyOnTaskOfTDeferredGuardThrowsExceptionWithExtraHelpfulMessage()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());

            var theMethod = from m in new Albedo.Methods<AsyncHost>()
                            select m.TaskOfTWithInnerGuardClause(null);

            // Act
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(theMethod));
            // Assert
            Assert.Contains("Task", e.Message);
            Assert.Contains("async", e.Message);
        }

        [Fact]
        public void VerifyOnTaskWithCorrectGuardClauseDoesNotThrow()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());

            var theMethod = from m in new Albedo.Methods<AsyncHost>()
                            select m.TaskWithCorrectGuardClause(null);

            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(theMethod)));
        }

        [Fact]
        public void VerifyOnTaskOfTWithCorrectGuardClauseDoesNotThrow()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());

            var theMethod = from m in new Albedo.Methods<AsyncHost>()
                            select m.TaskOfTWithCorrectGuardClause(null);

            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(theMethod)));
        }

        private class AsyncHost
        {
            public Task<string> TaskOfTWithCorrectGuardClause(object obj)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));

                return Task.Run(() => obj.ToString());
            }
            
            public Task TaskWithCorrectGuardClause(object obj)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));

                return Task.Run(() => obj.ToString());
            }

            public Task<string> TaskOfTWithInnerGuardClause(object obj)
            {
                return Task.Run(() =>
                {
                    if (obj == null) throw new ArgumentNullException(nameof(obj));

                    return obj.ToString();
                });
            }

            public Task TaskWithInnerGuardClause(object obj)
            {
                return Task.Run(() =>
                {
                    if (obj == null)
                        throw new ArgumentNullException(nameof(obj));

                    return obj.ToString();
                });
            }
        }

        [Fact]
        public void VerifyNonProperlyGuardedConstructorThrowsException()
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var constructorInfo = typeof (NonProperlyGuardedClass).GetConstructors().Single();

            var exception = Assert.Throws<GuardClauseException>(() => sut.Verify(constructorInfo));
            Assert.Contains("Guard Clause prevented it, however", exception.Message);
        }

        [Fact]
        public void VerifyNonProperlyGuardedPropertyThrowsException()
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var propertyInfo = typeof(NonProperlyGuardedClass).GetProperty(nameof(NonProperlyGuardedClass.Property));

            var exception = Assert.Throws<GuardClauseException>(() => sut.Verify(propertyInfo));
            Assert.Contains("Guard Clause prevented it, however", exception.Message);
        }

        [Theory]
        [InlineData(nameof(NonProperlyGuardedClass.Method), "Guard Clause prevented it, however")]
        [InlineData(nameof(NonProperlyGuardedClass.DeferredMethod), "deferred")]
        [InlineData(nameof(NonProperlyGuardedClass.AnotherDeferredMethod), "deferred")]
        public void VerifyNonProperlyGuardedMethodThrowsException(string methodName, string expectedMessage)
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var methodInfo = typeof(NonProperlyGuardedClass).GetMethod(methodName);

            var exception = Assert.Throws<GuardClauseException>(() => sut.Verify(methodInfo));
            Assert.Contains(expectedMessage, exception.Message);
        }

        private static Type[] GuardedOpenGenericTypes => new[]
        {
            typeof(NoContraint<>),
            typeof(InterfacesContraint<>),
            typeof(StructureAndInterfacesContraint<>),
            typeof(ParameterizedConstructorTestConstraint<>),
            typeof(UnclosedGenericMethodTestType<>),
            typeof(NestedGenericParameterTestType<,>)

        };

        public static TheoryData<MemberRef<ConstructorInfo>> ConstructorsOnGuardedOpenGenericTypes =>
            MakeTheoryData(
                GuardedOpenGenericTypes
                    .SelectMany(t => t.GetConstructors())
                    .Select(c => new MemberRef<ConstructorInfo>(c)));

        public static TheoryData<MemberRef<MethodInfo>> MethodsOnGuardedOpenGenericTypes =>
            MakeTheoryData(
                GuardedOpenGenericTypes
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    // Skip getters and setters
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
                    .Select(m => new MemberRef<MethodInfo>(m)));

        public static TheoryData<MemberRef<PropertyInfo>> PropertiesOnGuardedOpenGenericTypes =>
            MakeTheoryData(
                GuardedOpenGenericTypes
                    .SelectMany(t => t.GetProperties())
                    .Select(c => new MemberRef<PropertyInfo>(c)));

        private static Type[] UnguardedOpenGenericTypes => new[]
        {
            typeof(ClassContraint<>),
            typeof(CertainClassContraint<>),
            typeof(CertainClassAndInterfacesContraint<>),
            typeof(MultipleGenericArguments<,>),
            typeof(AbstractTypeAndInterfacesContraint<>),
            typeof(OpenGenericTestType<>).BaseType,
            typeof(ConstructedGenericTestType<>).BaseType,
            typeof(InternalProtectedConstructorTestConstraint<>),
            typeof(ModestConstructorTestConstraint<>),
            typeof(ConstructorMatchTestType<,>),
            typeof(MethodMatchTestType<,>),
            typeof(ByRefTestType<>)
        };

        public static TheoryData<MemberRef<ConstructorInfo>> ConstructorsOnUnguardedOpenGenericTypes =>
            MakeTheoryData(
                UnguardedOpenGenericTypes
                    .SelectMany(t => t.GetConstructors())
                    .Select(c => new MemberRef<ConstructorInfo>(c)));

        public static TheoryData<MemberRef<MethodInfo>> MethodsOnUnguardedOpenGenericTypes =>
            MakeTheoryData(
                UnguardedOpenGenericTypes
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    // Skip getters and setters
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
                    .Select(m => new MemberRef<MethodInfo>(m)));

        public static TheoryData<MemberRef<PropertyInfo>> PropertiesOnUnguardedOpenGenericTypes =>
            MakeTheoryData(
                UnguardedOpenGenericTypes
                    .SelectMany(t => t.GetProperties())
                    .Select(c => new MemberRef<PropertyInfo>(c)));

        private class NoContraint<T>
        {
            public NoContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class InterfacesContraint<T> where T : IInterfaceTestType, IEnumerable<object>
        {
            public InterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class StructureAndInterfacesContraint<T> where T : struct, IInterfaceTestType, IEnumerable<object>
        {
            public StructureAndInterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        public interface IInterfaceTestType
        {
            event EventHandler TestEvent;

            object Property { get; set; }

            void Method(object argument);
        }

        private class ClassContraint<T> where T : class
        {
            public ClassContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class CertainClassContraint<T> where T : ConcreteType
        {
            public CertainClassContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class CertainClassAndInterfacesContraint<T>
            where T : ConcreteType, IInterfaceTestType, IEnumerable<object>
        {
            public CertainClassAndInterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class MultipleGenericArguments<T1, T2> where T1 : class
        {
            public MultipleGenericArguments(T1 argument1, T2 argument2)
            {
            }

            public T1 Property { get; set; }

            public void Method(T1 argument1, T2 argument2)
            {
            }
        }

        private class AbstractTypeAndInterfacesContraint<T>
            where T : AbstractTestType, IInterfaceTestType, IEnumerable<object>
        {
            public AbstractTypeAndInterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        public abstract class AbstractTestType
        {
            public abstract event EventHandler TestEvent;
            protected abstract event EventHandler ProtectedTestEvent;

            public abstract object Property { get; set; }

            protected abstract object ProtectedProperty { get; set; }

            public abstract void Method(object argument);
            protected abstract void ProtectedMethod(object argument);
        }

        private class OpenGenericTestType<T> : OpenGenericTestTypeBase<T> where T : class
        {
            public OpenGenericTestType(T argument) : base(argument)
            {
            }
        }

        private class OpenGenericTestTypeBase<T>
        {
            public OpenGenericTestTypeBase(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class ConstructedGenericTestType<T> : ConstructedGenericTestTypeBase<string, T> where T : class
        {
            public ConstructedGenericTestType(string argument1, T argument2) : base(argument1, argument2)
            {
            }
        }

        private class ConstructedGenericTestTypeBase<T1, T2>
        {
            public ConstructedGenericTestTypeBase(T1 argument1, T2 argument2)
            {
            }

            public T1 Property1 { get; set; }

            public T2 Property2 { get; set; }

            public void Method(T1 argument1, T2 argument2)
            {
            }
        }

        private class ParameterizedConstructorTestConstraint<T> where T : ParameterizedConstructorTestType, new()
        {
            public void Method(T argument, object test)
            {
                if (argument == null) throw new ArgumentNullException(nameof(argument));
                if (argument.Argument1 == null || argument.Argument2 == null)
                {
                    throw new ArgumentException(
                        "The constructor of the base type should be called with anonymous values.");
                }
                if (test == null)
                {
                    throw new ArgumentNullException(nameof(test));
                }
            }
        }

        public class ParameterizedConstructorTestType
        {
            // to test duplicating with the specimenBuilder field of a dummy type.
            public static ISpecimenBuilder specimenBuilder = null;

            public ParameterizedConstructorTestType(object argument1, string argument2)
            {
                this.Argument1 = argument1;
                this.Argument2 = argument2;
            }

            public object Argument1 { get; }

            public string Argument2 { get; }
        }

        private class InternalProtectedConstructorTestConstraint<T> where T : InternalProtectedConstructorTestType
        {
            public InternalProtectedConstructorTestConstraint(T argument)
            {
            }
        }

        public class InternalProtectedConstructorTestType
        {
            protected internal InternalProtectedConstructorTestType()
            {
            }
        }

        private class ModestConstructorTestConstraint<T> where T : ModestConstructorTestType
        {
            public ModestConstructorTestConstraint(T argument)
            {
            }
        }

        public abstract class ModestConstructorTestType
        {
            protected ModestConstructorTestType(object argument1, int argument2)
            {
                throw new InvalidOperationException("Should use the modest constructor.");
            }

            protected ModestConstructorTestType(object argument1, string argument2, int argument3)
            {
                throw new InvalidOperationException("Should use the modest constructor.");
            }

            protected ModestConstructorTestType(object argument)
            {
            }
        }

        private class NoAccessibleConstructorTestConstraint<T> where T : NoAccessibleConstructorTestType
        {
            public NoAccessibleConstructorTestConstraint(T argument)
            {
            }
        }

        public class NoAccessibleConstructorTestType
        {
            private NoAccessibleConstructorTestType()
            {
            }
        }

        public class DynamicInstanceTestConstraint<T> where T : IDynamicInstanceTestType
        {
            public void Method(T argument)
            {
            }
        }

        public interface IDynamicInstanceTestType
        {
            object Property { get; set; }

            int VoidMethod(object argument1, int argument2);

            int ReturnMethod(object argument1, int argument2);
        }

        private class UnclosedGenericMethodTestType<T1> where T1 : class
        {
            public void Method<T2, T3, T4>(T1 argument1, int argument2, T2 argument3, T3 argument4, T4 argument5)
                where T2 : class where T4 : class
            {
                if (argument1 == null)
                {
                    throw new ArgumentNullException(nameof(argument1));
                }
                if (argument3 == null)
                {
                    throw new ArgumentNullException(nameof(argument3));
                }
                if (argument5 == null)
                {
                    throw new ArgumentNullException(nameof(argument5));
                }
            }
        }

        private class ConstructorMatchTestType<T1, T2> where T1 : class
        {
            public ConstructorMatchTestType(T1 argument)
            {
            }

            public ConstructorMatchTestType(T1 argument1, T2 argument2)
            {
            }

            public ConstructorMatchTestType(T1 argument1, T1 argument2)
            {
            }

            public ConstructorMatchTestType(T2 argument1, object argument2)
            {
            }
        }

        private class MethodMatchTestType<T1, T2> where T1 : class
        {
            public MethodMatchTestType(T1 argument)
            {
            }

            public void Method(T1 argument)
            {
            }

            public void Method(T1 argument1, T2 argument2)
            {
            }

            public void Method(T2 argument1, object argument2)
            {
            }

            public void Method<T3>(T1 argument1, object argument2)
                where T3 : class
            {
            }

            public void Method<T3>(int argument1, T3 argument2)
                where T3 : class
            {
            }

            public void Method<T3>(T1 argument1, T3 argument2)
                where T3 : class
            {
            }
        }

        private class ByRefTestType<T1> where T1 : class
        {
            public ByRefTestType(T1 argument)
            {
            }

            public void Method(ref T1 argument)
            {
            }

            public void Method<T2>(ref T2 argument) where T2 : class
            {
            }
            
            public void Method(ref T1 argument1, int argument2)
            {
            }

            public void Method(T1 argument1, int argument2)
            {
            }
        }

        private class OutParameterTestType
        {
            public void Method(string argument1, out object argument2, int argument3)
            {
                if (argument1 == null)
                {
                    throw new ArgumentNullException(nameof(argument1));
                }

                argument2 = null;
            }
        }

        private class NestedGenericParameterTestType<T1, T2>
        {
            public NestedGenericParameterTestType(IEnumerable<T1> arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(IEnumerable<IEnumerable<T2>> arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1 arg1, Func<T1, IEnumerable<T2>> arg2)
            {
                if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            }

            public NestedGenericParameterTestType(T1[] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1[,] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1[][] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1[,][][] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(Func<T1, IEnumerable<IEnumerable<T2>>, T1[][]> arg1, T2 arg2)
            {
                if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            }
        }

        private class NonProperlyGuardedClass
        {
            private const string InvalidParamName = "invalidParamName";

            public NonProperlyGuardedClass(object argument)
            {
                if (argument == null) throw new ArgumentNullException(InvalidParamName);
            }

            public object Property
            {
                get => null;
                set
                {
                    if (value == null) throw new ArgumentNullException(InvalidParamName);
                }
            }

            public void Method(object argument)
            {
                if (argument == null) throw new ArgumentNullException(InvalidParamName);
            }

            public IEnumerable<object> DeferredMethod(object argument)
            {
                if (argument == null) throw new ArgumentNullException(InvalidParamName);

                yield return argument;
            }

            public IEnumerator<object> AnotherDeferredMethod(object argument)
            {
                if (argument == null) throw new ArgumentNullException(InvalidParamName);

                yield return argument;
            }
        }

        [Fact]
        public void VerifyOnAbstractMethodDoesNotThrow()
        {
            var method = typeof(AbstractTypeWithAbstractMethod)
                .GetMethod(nameof(AbstractTypeWithAbstractMethod.Method));
            var sut = new GuardClauseAssertion(new Fixture());
            sut.Verify(method);
        }

        private abstract class AbstractTypeWithAbstractMethod
        {
            public abstract void Method(object arg);
        }

        /// <summary>
        /// Wrapper around member to produce nice theory name
        /// </summary>
        public class MemberRef<T> where T : MemberInfo
        {
            public MemberRef(T member)
            {
                this.Member = member;
            }

            public T Member { get; }

            public override string ToString()
            {
                var str = new StringBuilder();
                str.Append(GetNonMangledTypeName(this.Member.DeclaringType));
                str.Append('.');

                str.Append(this.Member.Name);

                var methodBase = this.Member as MethodBase;
                if (methodBase != null)
                {
                    str.Append('(');
                    str.Append(string.Join(", ",
                        methodBase.GetParameters().Select(p => GetNonMangledTypeName(p.ParameterType))));
                    str.Append(')');
                }

                return str.ToString();
            }

            private static string GetNonMangledTypeName(Type type)
            {
                var typeName = type.Name;
                if (!type.GetTypeInfo().IsGenericType)
                    return typeName;

                typeName = typeName.Substring(0, typeName.IndexOf('`'));
                var genericArgTypes = type.GetGenericArguments().Select(GetNonMangledTypeName);
                return string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", typeName,
                    string.Join(", ", genericArgTypes));
            }
        }

        public static class MemberRef
        {
            public static MemberRef<MethodInfo> MethodByName(Type type, string methodName)
            {
                return new MemberRef<MethodInfo>(type.GetMethod(methodName));
            }

            public static MemberRef<MethodInfo> MethodByIndex(Type type, int index)
            {
                return new MemberRef<MethodInfo>(type.GetMethods().Where(IsNotEqualsMethod).ElementAt(index));
            }

            public static MemberRef<ConstructorInfo> CtorByArgs(Type type, Type[] ctorParams)
            {
                return new MemberRef<ConstructorInfo>(type.GetConstructor(ctorParams));
            }

            private static bool IsNotEqualsMethod(MethodInfo method)
            {
                return method.Name != "Equals";
            }
        }

        private static TheoryData<T> MakeTheoryData<T>(IEnumerable<T> entries)
        {
            var result = new TheoryData<T>();

            foreach (var entry in entries)
            {
                result.Add(entry);
            }

            return result;
        }
    }
}
