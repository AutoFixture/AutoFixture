using System;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ExceptionBoundaryBehaviorTest
    {
        [Fact]
        public void AssertNullActionThrows()
        {
            // Fixture setup
            var dummyContext = string.Empty;
            var sut = new DelegatingExceptionBoundaryBehavior();
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                sut.Assert(null, dummyContext));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertNullContextThrows()
        {
            // Fixture setup
            Action<object> dummyAction = x => { };
            var sut = new DelegatingExceptionBoundaryBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Assert(dummyAction, null));
            // Teardown
        }

        [Fact]
        public void AssertWillAssertInvalidValueCorrectly()
        {
            // Fixture setup
            Action<object> expected = o => { throw new ArgumentNullException(); };
            var dummyContext = string.Empty;

            var verified = false;
            var sut = new DelegatingExceptionBoundaryBehavior();
            sut.OnExercise = a => { verified = a == expected; a(new object()); };
            sut.OnIsSatisfiedBy = e => e is ArgumentNullException;
            // Exercise system
            sut.Assert(expected, dummyContext);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void AssertWillNotThrowWhenActionThrowsCorrectException()
        {
            // Fixture setup
            var dummyContext = string.Empty;
            var sut = new DelegatingExceptionBoundaryBehavior();
            sut.OnIsSatisfiedBy = e => e is ArgumentNullException;
            // Exercise system
            sut.Assert(o => { throw new ArgumentNullException(); }, dummyContext);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionDoesNotThrow()
        {
            // Fixture setup
            var dummyContext = string.Empty;
            var sut = new DelegatingExceptionBoundaryBehavior();
            // Exercise system
            Assert.Throws<BoundaryConventionException>(() =>
                sut.Assert(g => { }, dummyContext));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillRethrowWhenActionThrowsAnotherException()
        {
            // Fixture setup
            var dummyContext = string.Empty;
            var expectedException = new TargetInvocationException("Test", new InvalidOperationException());
            var sut = new DelegatingExceptionBoundaryBehavior();
            // Exercise system
            var e = Assert.Throws(expectedException.GetType(), () =>
                sut.Assert(g => { throw expectedException; }, dummyContext));
            Assert.Equal(expectedException, e);
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillCorrectlyContinueWhenExceptionIsSatisfiedByBehavior()
        {
            // Fixture setup
            var dummyContext = string.Empty;
            var expectedException = new Exception();
            var verified = false;

            var behavior = new DelegatingExceptionBoundaryBehavior();
            behavior.OnExercise = a => { throw expectedException; };
            behavior.OnIsSatisfiedBy = e => verified = e == expectedException;
            // Exercise system
            behavior.Assert(x => { }, dummyContext);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }
    }
}
