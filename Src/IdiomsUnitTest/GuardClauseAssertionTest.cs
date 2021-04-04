using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used via reflection.")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Required for testing.")]
    public partial class GuardClauseAssertionTest
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
                {
                    throw new ArgumentException(
                        "Guid.Empty not allowed.",
                        nameof(someGuid));
                }

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
                {
                    throw new ArgumentException(
                        "Guid.Empty not allowed.",
                        nameof(someGuid));
                }

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
        [InlineData(typeof(ClassWithEnumerableNonDeferredStringMissingGuard))]
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
            private class ReadOnlyCollection : ReadOnlyCollectionBase
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
                return new ReadOnlyCollection<string>(new[] { someString, someString, someString });
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

        private class ClassWithEnumerableNonDeferredStringMissingGuard
        {
            public string GetValues(string someString)
            {
                return someString;
            }
        }

        private interface IHaveNoImplementers
        {
        }

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
            var constructorInfo = typeof(NonProperlyGuardedClass).GetConstructors().Single();

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
        [InlineData(nameof(NonProperlyGuardedClass.DeferredMethodReturningGenericEnumerable), "deferred")]
        [InlineData(nameof(NonProperlyGuardedClass.DeferredMethodReturningGenericEnumerator), "deferred")]
        [InlineData(nameof(NonProperlyGuardedClass.DeferredMethodReturningNonGenericEnumerable), "deferred")]
        [InlineData(nameof(NonProperlyGuardedClass.DeferredMethodReturningNonGenericEnumerator), "deferred")]
        public void VerifyNonProperlyGuardedMethodThrowsException(string methodName, string expectedMessage)
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var methodInfo = typeof(NonProperlyGuardedClass).GetMethod(methodName);

            var exception = Assert.Throws<GuardClauseException>(() => sut.Verify(methodInfo));
            Assert.Contains(expectedMessage, exception.Message);
        }

        public class InternalProtectedConstructorTestType
        {
            protected internal InternalProtectedConstructorTestType()
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

        private class NonProperlyGuardedClass
        {
            public NonProperlyGuardedClass(object argument)
            {
                if (argument == null) throw new ArgumentNullException("invalid parameter name");
            }

            public object Property
            {
                get => null;
                set
                {
                    if (value == null) throw new ArgumentNullException("invalid parameter name");
                }
            }

            public void Method(object argument)
            {
                if (argument == null) throw new ArgumentNullException("invalid parameter name");
            }

            public IEnumerable<object> DeferredMethodReturningGenericEnumerable(object argument)
            {
                if (argument == null) throw new ArgumentNullException(nameof(argument));

                yield return argument;
            }

            public IEnumerator<object> DeferredMethodReturningGenericEnumerator(object argument)
            {
                if (argument == null) throw new ArgumentNullException(nameof(argument));

                yield return argument;
            }

            public IEnumerable DeferredMethodReturningNonGenericEnumerable(object argument)
            {
                if (argument == null) throw new ArgumentNullException(nameof(argument));

                yield return argument;
            }

            public IEnumerator DeferredMethodReturningNonGenericEnumerator(object argument)
            {
                if (argument == null) throw new ArgumentNullException(nameof(argument));

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

        [Fact]
        public void VerifyClassWithEmptyStringGuardDoesNotThrow()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new EmptyStringBehaviorExpectation());
            var constructors = typeof(ClassWithEmptyStringGuard)
                .GetConstructors();

            var exception = Record.Exception(() => sut.Verify(constructors));

            Assert.Null(exception);
        }

        [Fact]
        public void VerifyClassWithoutEmptyStringGuardThrows()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new EmptyStringBehaviorExpectation());
            var constructors = typeof(ClassWithoutEmptyStringGuard)
                .GetConstructors();

            Assert.Throws<GuardClauseException>(() => sut.Verify(constructors));
        }

        [Fact]
        public void VerifyClassWithoutEmptyStringGuardThrowsWithMessage()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new EmptyStringBehaviorExpectation());
            var constructors = typeof(ClassWithoutEmptyStringGuard)
                .GetConstructors();

            var actual = Record.Exception(() => sut.Verify(constructors));

            Assert.StartsWith(
                "An attempt was made to assign the value <empty string>",
                actual.Message);
        }

        [Fact]
        public void VerifyClassWithImproperEmptyStringGuardThrows()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new EmptyStringBehaviorExpectation());
            var constructors = typeof(ClassWithImproperEmptyStringGuard)
                .GetConstructors();

            Assert.Throws<GuardClauseException>(() => sut.Verify(constructors));
        }

        [Fact]
        public void VerifyClassWithImproperEmptyStringGuardThrowsWithExpectedMessage()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new EmptyStringBehaviorExpectation());
            var constructors = typeof(ClassWithImproperEmptyStringGuard)
                .GetConstructors();

            var actual = Record.Exception(() => sut.Verify(constructors));

            Assert.Contains(
                $"Expected parameter name: arg{Environment.NewLine}Actual parameter name: invalid parameter name",
                actual.Message);
        }

        private class ClassWithEmptyStringGuard
        {
            public ClassWithEmptyStringGuard(string arg)
            {
                if (arg is null)
                    throw new ArgumentNullException(nameof(arg));

                if (arg == string.Empty)
                    throw new ArgumentException("Value cannot be empty.", nameof(arg));
            }
        }

        private class ClassWithoutEmptyStringGuard
        {
            public ClassWithoutEmptyStringGuard(string arg)
            {
                if (arg is null)
                    throw new ArgumentNullException(nameof(arg));
            }
        }

        private class ClassWithImproperEmptyStringGuard
        {
            public ClassWithImproperEmptyStringGuard(string arg)
            {
                if (arg == string.Empty)
                    throw new ArgumentException("Value cannot be empty.", "invalid parameter name");
            }
        }

        [Fact]
        public void VerifyClassWithWhiteSpaceStringGuardDoesNotThrow()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new WhiteSpaceStringBehaviorExpectation());
            var constructors = typeof(ClassWithWhiteSpaceStringGuard)
                .GetConstructors();

            var exception = Record.Exception(() => sut.Verify(constructors));

            Assert.Null(exception);
        }

        [Fact]
        public void VerifyClassWithoutWhiteSpaceStringGuardThrows()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new WhiteSpaceStringBehaviorExpectation());
            var constructors = typeof(ClassWithoutWhiteSpaceStringGuard)
                .GetConstructors();

            Assert.Throws<GuardClauseException>(() => sut.Verify(constructors));
        }

        [Fact]
        public void VerifyClassWithoutWhiteSpaceStringGuardThrowsWithExpectedMessage()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new WhiteSpaceStringBehaviorExpectation());
            var constructors = typeof(ClassWithoutWhiteSpaceStringGuard)
                .GetConstructors();

            var actual = Record.Exception(() => sut.Verify(constructors));

            Assert.StartsWith("An attempt was made to assign the value <white space>", actual.Message);
        }

        [Fact]
        public void VerifyClassWithImproperWhiteSpaceStringGuardThrowsWithExpectedMessage()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new WhiteSpaceStringBehaviorExpectation());
            var constructors = typeof(ClassWithImproperWhiteSpaceStringGuard)
                .GetConstructors();

            var actual = Record.Exception(() => sut.Verify(constructors));

            Assert.Contains(
                $"Expected parameter name: arg{Environment.NewLine}Actual parameter name: invalid parameter name",
                actual.Message);
        }

        [Fact]
        public void VerifyClassWithImproperWhiteSpaceStringGuardThrows()
        {
            var sut = new GuardClauseAssertion(new Fixture(), new WhiteSpaceStringBehaviorExpectation());
            var constructors = typeof(ClassWithImproperWhiteSpaceStringGuard)
                .GetConstructors();

            Assert.Throws<GuardClauseException>(() => sut.Verify(constructors));
        }

        private class ClassWithWhiteSpaceStringGuard
        {
            public ClassWithWhiteSpaceStringGuard(string arg)
            {
                if (arg is null)
                    throw new ArgumentNullException(nameof(arg));

                if (arg == string.Empty)
                    throw new ArgumentException("Value cannot be empty.", nameof(arg));

                if (arg.All(x => x == ' '))
                    throw new ArgumentException("Value cannot be whitespace.", nameof(arg));
            }
        }

        private class ClassWithoutWhiteSpaceStringGuard
        {
            public ClassWithoutWhiteSpaceStringGuard(string arg)
            {
                if (arg is null)
                    throw new ArgumentNullException(nameof(arg));

                if (arg == string.Empty)
                    throw new ArgumentException("Value cannot be empty.", nameof(arg));
            }
        }

        private class ClassWithImproperWhiteSpaceStringGuard
        {
            public ClassWithImproperWhiteSpaceStringGuard(string arg)
            {
                if (arg.All(x => x == ' '))
                    throw new ArgumentException("Value cannot be whitespace.", "invalid parameter name");
            }
        }
    }
}
