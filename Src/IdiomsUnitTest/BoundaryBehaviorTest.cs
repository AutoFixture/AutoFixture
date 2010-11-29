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
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<MyBoundaryBehavior>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () => 
                sut.ReflectionAssert((Action<object>) null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ReflectionAssertWillAssertInvalidValueCorrectly()
        {
            // Fixture setup
            var fixture = new Fixture();
            Action<object> expected = o => { throw new TargetInvocationException("Test", new ArgumentNullException());};

            var sut = fixture.CreateAnonymous<MyBoundaryBehavior>();
            // Exercise system
            sut.ReflectionAssert(expected);
            // Verify outcome
            var result = sut.AssertAction;
            Assert.Equal<Action<object>>(expected, result);
            // Teardown
        }

        [Fact]
        public void AssertWillNotThrowWhenActionThrowsCorrectException()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<MyBoundaryBehavior>();
            // Exercise system
            sut.ReflectionAssert(o => { throw new TargetInvocationException("Test", new ArgumentNullException()); });
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionDoesNotThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<MyBoundaryBehavior>();
            // Exercise system
            Assert.Throws(typeof(ValueGuardConventionException), () =>
                sut.ReflectionAssert(g => { }));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertWillThrowWhenActionThrowsAnotherException()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<MyBoundaryBehavior>();
            // Exercise system
            Assert.Throws(typeof(ValueGuardConventionException), () =>
                sut.ReflectionAssert(g => { throw new TargetInvocationException("Test", new InvalidOperationException()); }));
            // Verify outcome (expected exception)
            // Teardown
        }
        
        private class MyBoundaryBehavior : IBoundaryBehavior
        {
            private Action<object> assertAction;
            public Action<object> AssertAction {  get { return this.assertAction; } }

            #region Implementation of IBoundaryBehavior

            public void Exercise(Action<object> action)
            {
                this.assertAction = action;
                action(null);
            }

            public bool IsSatisfiedBy(Type exceptionType)
            {
                return exceptionType == typeof(ArgumentNullException);
            }

            public string Description
            {
                get { return "test invalid value"; }
            }

            #endregion
        }
    }
}
