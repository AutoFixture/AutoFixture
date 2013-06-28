using Ploeh.TestTypeFoundation;
using System;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class MemberComparerTest
    {
        [Fact]
        public void SutIsMemberComparer()
        {
            // Fixture setup
            var dummyEqualityComparer = new DelegatingEqualityComparer();
            var sut = new MemberComparer(dummyEqualityComparer);
            // Exercise system and verify outcome
            Assert.IsAssignableFrom<IMemberComparer>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEqualityComparerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MemberComparer(null));
            // Teardown
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Fixture setup
            var dummyEqualityComparer = new DelegatingEqualityComparer();
            var expected = new MemberComparer(dummyEqualityComparer);
            var sut = new MemberComparer(expected);
            // Exercise system
            var result = sut.Comparer;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var dummyEqualityComparer = new DelegatingEqualityComparer();
            var sut = new MemberComparer(dummyEqualityComparer);
            var dummyProperty = typeof(PropertyHolder<int>).GetProperty("Property");
            // Exercise system
            var result = sut.IsSatisfiedBy(dummyProperty);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByFieldReturnsCorrectResult()
        {
            // Fixture setup
            var dummyEqualityComparer = new DelegatingEqualityComparer();
            var sut = new MemberComparer(dummyEqualityComparer);
            var dummyField = typeof(FieldHolder<int>).GetField("Field");
            // Exercise system
            var result = sut.IsSatisfiedBy(dummyField);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void EqualsUsesInjectedComparer()
        {
            // Fixture setup
            var verified = false;
            var comparer = new DelegatingEqualityComparer
            {
                OnEquals = (x, y) => verified = true
            };
            var sut = new MemberComparer(comparer);
            // Exercise system
            sut.Equals("dummy", "dummy");
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Theory]
        [InlineData(123, 123, true)]
        [InlineData(123, 321, false)]
        public void EqualsForwardsCorrectCallToComparer(
            object a, 
            object b,
            bool expected)
        {
            // Fixture setup
            var comparer = new DelegatingEqualityComparer
            {
                OnEquals = (x, y) => x.Equals(y)
            };
            var sut = new MemberComparer(comparer);
            // Exercise system
            var result = sut.Equals(a, b);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData(123, 2)]
        public void GetHashCodeForwardsCorrectCallToComparer(
            object obj,
            int expected)
        {
            // Fixture setup
            var comparer = new DelegatingEqualityComparer
            {
                OnGetHashCode = x => x == obj ? expected : 0
            };
            var sut = new MemberComparer(comparer);
            // Exercise system
            var result = sut.GetHashCode(obj);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
