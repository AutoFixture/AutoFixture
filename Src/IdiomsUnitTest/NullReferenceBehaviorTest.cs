using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class NullReferenceBehaviorTest
    {
        [Fact]
        public void SutIsBoundaryBehavior()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullReferenceBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ExceptionBoundaryBehavior>(sut);
            // Teardown
        }


        [Fact]
        public void AssertWithNullActionWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<NullReferenceBehavior>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.Exercise((Action<object>)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void AssertInvokesActionWithCorrectValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            var capturedObject = new object();
            Action<object> actionSpy = o => capturedObject = o;

            var sut = fixture.CreateAnonymous<NullReferenceBehavior>();
            // Exercise system
            sut.Exercise(actionSpy);
            // Verify outcome
            Assert.Null(capturedObject);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullExceptionWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<NullReferenceBehavior>();
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                sut.IsSatisfiedBy(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForArgumentNullException()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<NullReferenceBehavior>();
            // Exercise system
            var result = sut.IsSatisfiedBy(new ArgumentNullException());
            // Verify outcome 
            Assert.True(result, "IsSatisfiedBy");
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForOtherException()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<NullReferenceBehavior>();
            // Exercise system
            var result = sut.IsSatisfiedBy(new InvalidOperationException());
            // Verify outcome 
            Assert.False(result, "IsSatisfiedBy");
            // Teardown
        }

        [Fact]
        public void DescriptionIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = "null";

            var sut = fixture.CreateAnonymous<NullReferenceBehavior>();
            // Exercise system
            var result = sut.Description;
            // Verify outcome
            Assert.Equal<string>(expected, result);
            // Teardown
        }
    }
}
