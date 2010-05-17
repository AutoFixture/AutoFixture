using System;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class InvalidGuidValueTest
    {
        [Fact]
        public void SutIsIInvalidValue()
        {
            // Fixture setup
            // Exercise system
            var sut = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(InvalidGuidValue));
            // Verify outcome
            Assert.IsAssignableFrom<IInvalidValue>(sut);
            // Teardown
        }

        [Fact]
        public void ValueIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = Guid.Empty;

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            var result = sut.Value;
            // Verify outcome
            Assert.Equal<Guid>(expected, result);
            // Teardown
        }

        [Fact]
        public void AssertWithNullActionWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                sut.Assert((Action<object>)null));
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

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            sut.Assert(actionSpy);
            // Verify outcome
            Assert.Equal(Guid.Empty, capturedObject);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWithNullExceptionWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
              sut.IsSatisfiedBy((Type)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsTrueForArgumentException()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(ArgumentException));
            // Verify outcome 
            Assert.True(result, "IsSatisfiedBy");
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsFalseForOtherException()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            var result = sut.IsSatisfiedBy(typeof(InvalidOperationException));
            // Verify outcome 
            Assert.False(result, "IsSatisfiedBy");
            // Teardown
        }

        [Fact]
        public void DescriptionIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = "empty Guid";

            var sut = fixture.CreateAnonymous<InvalidGuidValue>();
            // Exercise system
            var result = sut.Description;
            // Verify outcome
            Assert.Equal<string>(expected, result);
            // Teardown
        }

    }
}
