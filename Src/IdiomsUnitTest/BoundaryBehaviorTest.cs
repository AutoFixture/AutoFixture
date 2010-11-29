using System;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class BoundaryBehaviorTest
    {
        [Fact]
        public void ReflectionAssertWithNullActionWillThrow()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            // Exercise system
            Assert.Throws<ArgumentNullException>(() => 
                sut.ReflectionAssert(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ReflectionAssertWillAssertInvalidValueCorrectly()
        {
            // Fixture setup
            Action<object> expected = o => { throw new TargetInvocationException("Test", new ArgumentNullException());};

            var verified = false;
            var sut = new DelegatingBoundaryBehavior();
            sut.OnExercise = a => { verified = a == expected; a(new object()); };
            sut.OnIsSatisfiedBy = e => e is ArgumentNullException;
            // Exercise system
            sut.ReflectionAssert(expected);
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
            sut.ReflectionAssert(o => { throw new TargetInvocationException("Test", new ArgumentNullException()); });
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionDoesNotThrow()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            // Exercise system
            Assert.Throws<ValueGuardConventionException>(() =>
                sut.ReflectionAssert(g => { }));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionThrowsAnotherException()
        {
            // Fixture setup
            var sut = new DelegatingBoundaryBehavior();
            // Exercise system
            Assert.Throws(typeof(ValueGuardConventionException), () =>
                sut.ReflectionAssert(g => { throw new TargetInvocationException("Test", new InvalidOperationException()); }));
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
            behavior.OnExercise = a => { throw new TargetInvocationException(expectedException); };
            behavior.OnIsSatisfiedBy = e => verified = e == expectedException;
            // Exercise system
            behavior.ReflectionAssert(x => { });
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }
    }
}
