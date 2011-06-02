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
    }
}
