using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Ploeh.AutoFixture.Kernel;

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
            ISpecimenBuilderComposer expectedComposer = new Fixture();
            var sut = new GuardClauseAssertion(expectedComposer);
            // Exercise system
            var result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrectFromGreedyConstructor()
        {
            // Fixture setup
            ISpecimenBuilderComposer expectedComposer = new Fixture();
            var dummyExpectation = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(expectedComposer, dummyExpectation);
            // Exercise system
            ISpecimenBuilderComposer result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
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
                    mockVerified = setterCmd.MemberInfo.Equals(property)
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
    }
}
