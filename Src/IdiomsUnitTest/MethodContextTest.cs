using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class MethodContextTest
    {
        [Fact]
        public void InitializeWithNullComposerAndDummyMethodInfoThrows()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodContext(null, dummyMethod));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodContext(dummyComposer, (MethodInfo)null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullComposerAndDummyConstructorInfoThrows()
        {
            // Fixture setup
            var dummyConstructor = typeof(object).GetConstructors().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodContext(null, dummyConstructor));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullConstructorInfoThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MethodContext(dummyComposer, (ConstructorInfo)null));
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var dummyMethod = typeof(object).GetMethods().First();
            var sut = new MethodContext(expectedComposer, dummyMethod);
            // Exercise system
            ISpecimenBuilderComposer result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void MethodBaseIsCorrect()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var expectedMethod = typeof(object).GetMethods().First();
            var sut = new MethodContext(dummyFixture, expectedMethod);
            // Exercise system
            MethodBase result = sut.MethodBase;
            // Verify outcome
            Assert.Equal(expectedMethod, result);
            // Teardown
        }

        [Fact]
        public void SutIsVerifiableBoundary()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var expectedMethod = typeof(object).GetMethods().First();
            // Exercise system
            var sut = new MethodContext(dummyFixture, expectedMethod);
            // Verify outcome
            Assert.IsAssignableFrom<IVerifiableBoundary>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesWithNullConventionThrows()
        {
            // Fixture setup
            var dummyFixture = new Fixture();
            var expectedMethod = typeof(object).GetMethods().First();
            var sut = new MethodContext(dummyFixture, expectedMethod);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.VerifyBoundaries(null));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void VerifyBoundariesInvokesBehaviorOncePerParameter(int index)
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = (from m in typeof(TypeWithOverloadedMembers).GetMethods()
                          where m.Name == "DoSomething"
                          orderby m.GetParameters().Length
                          select m).ElementAt(index);
            var sut = new MethodContext(fixture, method);

            var invocations = 0;
            var convention = new DelegatingBoundaryConvention
            {
                OnCreateBoundaryBehaviors = t => new[] { new DelegatingBoundaryBehavior
                {
                    OnAssert = a => invocations++
                }}
            };
            // Exercise system
            sut.VerifyBoundaries(convention);
            // Verify outcome
            Assert.Equal(method.GetParameters().Length, invocations);
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesInvokesEveryBehavior()
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var sut = new MethodContext(fixture, method);

            var invocations = new List<int>();
            var behaviors = Enumerable.Range(0, 3).Select(i => new DelegatingBoundaryBehavior { OnAssert = a => invocations.Add(i) }).ToArray();
            var convention = new DelegatingBoundaryConvention { OnCreateBoundaryBehaviors = t => behaviors };
            // Exercise system
            sut.VerifyBoundaries(convention);
            // Verify outcome
            Assert.True(Enumerable.Range(0, 3).SequenceEqual(invocations));
            // Teardown
        }

        [Theory]
        [InlineData("ConsumeString")]
        [InlineData("ConsumeInt32")]
        [InlineData("ConsumeGuid")]
        [InlineData("ConsumeStringAndInt32")]
        [InlineData("ConsumeStringAndGuid")]
        [InlineData("ConsumeInt32AndGuid")]
        [InlineData("ConsumeStringAndInt32AndGuid")]
        public void VerifyBoundariesCreatesBehaviorsBasedOnCorrectType(string methodName)
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = typeof(GuardedMethodHost).GetMethod(methodName);
            var sut = new MethodContext(fixture, method);

            var invokedParameterTypes = new List<Type>();
            var typedBehaviors = (from p in method.GetParameters()
                                  select new
                                  {
                                      Behavior = new DelegatingBoundaryBehavior { OnAssert = a => invokedParameterTypes.Add(p.ParameterType) },
                                      ParameterType = p.ParameterType
                                  }).ToDictionary(x => x.ParameterType);
            var convention = new DelegatingBoundaryConvention { OnCreateBoundaryBehaviors = t => new[] { typedBehaviors[t].Behavior } };
            // Exercise system
            sut.VerifyBoundaries(convention);
            // Verify outcome
            Assert.True(method.GetParameters().Select(p => p.ParameterType).SequenceEqual(invokedParameterTypes));
            // Teardown
        }

        [Theory]
        [InlineData("ConsumeString")]
        [InlineData("ConsumeInt32")]
        [InlineData("ConsumeGuid")]
        [InlineData("ConsumeStringAndInt32")]
        [InlineData("ConsumeStringAndGuid")]
        [InlineData("ConsumeInt32AndGuid")]
        [InlineData("ConsumeStringAndInt32AndGuid")]
        public void VerifyBoundariesOnGuardedMethodDoesNotThrow(string methodName)
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = typeof(GuardedMethodHost).GetMethod(methodName);
            var sut = new MethodContext(fixture, method);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.VerifyBoundaries());
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesOnMethodWithSingleUnguardedStringArgumentThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = typeof(UnguardedMethodHost).GetMethod("ConsumeUnguardedString");
            var sut = new MethodContext(fixture, method);
            // Exercise system and verify outcome
            Assert.Throws<BoundaryConventionException>(() =>
                sut.VerifyBoundaries());
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesOnMethodWithOneGuardedAndOneUnguardedParameterThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = typeof(UnguardedMethodHost).GetMethod("ConsumeGuardedGuidAndUnguardedString");
            var sut = new MethodContext(fixture, method);
            // Exercise system and verify outcome
            Assert.Throws<BoundaryConventionException>(() =>
                sut.VerifyBoundaries());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedConstructorHost<object>))]
        [InlineData(typeof(GuardedConstructorHost<OperatingSystem>))]
        [InlineData(typeof(GuardedConstructorHost<Version>))]
        public void VerifyBoundariesForGuardedConstructorSucceeds(Type type)
        {
            // Fixture setup
            var fixture = new Fixture();
            var ctor = type.GetConstructors().Single(c => c.GetParameters().Length == 1);
            var sut = new MethodContext(fixture, ctor);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.VerifyBoundaries());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(UnguardedConstructorHost<object>))]
        [InlineData(typeof(UnguardedConstructorHost<OperatingSystem>))]
        [InlineData(typeof(UnguardedConstructorHost<Version>))]
        public void VerifyBoundariesForUnguardedConstructorThrows(Type type)
        {
            // Fixture setup
            var fixture = new Fixture();
            var ctor = type.GetConstructors().Single(c => c.GetParameters().Length == 1);
            var sut = new MethodContext(fixture, ctor);
            // Exercise system and verify outcome
            Assert.Throws<BoundaryConventionException>(() =>
                sut.VerifyBoundaries());
            // Teardown
        }
    }
}
