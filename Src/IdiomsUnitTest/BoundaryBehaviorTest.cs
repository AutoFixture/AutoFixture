using System;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class BoundaryBehaviorTest
    {
        [Fact]
        public void AssertNullBehaviorThrows()
        {
            // Fixture setup
            Action<object> dummyAction = x => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                BoundaryBehavior.Assert(null, dummyAction));
            // Teardown
        }

        [Fact]
        public void AssertNullActionThrows()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            // Exercise system
            Assert.Throws<ArgumentNullException>(() => 
                sut.Assert(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillAssertInvalidValueCorrectly()
        {
            // Fixture setup
            Action<object> expected = o => { throw new ArgumentNullException();};

            var verified = false;
            var sut = new DelegatingBoundaryBehavior();
            sut.OnExercise = a => { verified = a == expected; a(new object()); };
            sut.OnIsSatisfiedBy = e => e is ArgumentNullException;
            // Exercise system
            sut.Assert(expected);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void AssertWillNotThrowWhenActionThrowsCorrectException()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            sut.OnIsSatisfiedBy = e => e is ArgumentNullException;
            // Exercise system
            sut.Assert(o => { throw new ArgumentNullException(); });
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionDoesNotThrow()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            // Exercise system
            Assert.Throws<BoundaryConventionException>(() =>
                sut.Assert(g => { }));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionThrowsAnotherException()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            // Exercise system
            Assert.Throws(typeof(BoundaryConventionException), () =>
                sut.Assert(g => { throw new TargetInvocationException("Test", new InvalidOperationException()); }));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillCorrectlyContinueWhenExceptionIsSatisfiedByBehavior()
        {
            // Fixture setup
            var expectedException = new Exception();
            var verified = false;

            var behavior = new DelegatingBoundaryBehavior();
            behavior.OnExercise = a => { throw expectedException; };
            behavior.OnIsSatisfiedBy = e => verified = e == expectedException;
            // Exercise system
            behavior.Assert(x => { });
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }
    }
}
