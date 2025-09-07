using System;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
#pragma warning disable SA1601 // This is a test class, so it's OK to have unannotated public members
    public partial class GuardClauseAssertionTest
#pragma warning restore SA1601
    {
        [Fact]
        public void VerifyThrowsForMissingGuardClauseOnNonNullableReferenceType()
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var method = typeof(NonNullableReferenceParameterClass)
                .GetMethod(nameof(NonNullableReferenceParameterClass.Method));

            Assert.Throws<GuardClauseException>(() => sut.Verify(method));
        }

        private class NonNullableReferenceParameterClass
        {
            public void Method(string foo) { /* no guard clause */ }
        }

        [Fact]
        public void VerifyDoesNotThrowForMissingGuardClauseOnNullableReferenceType()
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var method = typeof(NullableReferenceParameterClass)
                .GetMethod(nameof(NullableReferenceParameterClass.Method));

            var ex = Record.Exception(() => sut.Verify(method));

            Assert.Null(ex);
        }

        private class NullableReferenceParameterClass
        {
#nullable enable
            public void Method(string? foo)
            {
                if (foo == null) throw new ArgumentNullException(nameof(foo));
            }
        }

        [Fact]
        public void VerifyDoesNotThrowForMissingGuardClauseOnNullableValueType()
        {
            var sut = new GuardClauseAssertion(new Fixture());
            var method = typeof(NullableReferenceParameterClass).GetMethod(nameof(NullableReferenceParameterClass.Method));

            var ex = Record.Exception(() => sut.Verify(method));

            Assert.Null(ex);
        }

        private class NullableValueParameterClass
        {
            public void Method(int? foo)
            {
                if (foo == null) throw new ArgumentNullException(nameof(foo));
            }
        }
    }
}
