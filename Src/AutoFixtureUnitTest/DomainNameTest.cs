using Ploeh.AutoFixture;
using System;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DomainNameTest
    {
        [Fact]
        public void InitializeWithNullDomainNameThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new DomainName(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithEmptyDomainNameThrows()
        {
            // Fixture setup
            Assert.Throws<ArgumentException>(
                () => new DomainName(string.Empty));
            // Exercise system and verify outcome
            // Teardown
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            var expected = Guid.NewGuid().ToString();
            var sut = new DomainName(expected);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new DomainName(Guid.NewGuid().ToString());
            object other = null;
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new DomainName(Guid.NewGuid().ToString());
            var anonymousObject = new object();
            // Exercise system
            bool result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenDomainNamesDiffer()
        {
            // Fixture setup
            var sut = new DomainName(Guid.NewGuid().ToString());
            object other = new DomainName(Guid.NewGuid().ToString());
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenDomainNamesAreEqual()
        {
            // Fixture setup
            var domainName = Guid.NewGuid().ToString();

            var sut = new DomainName(domainName);
            var other = new DomainName(domainName);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Fixture setup
            var domainName = Guid.NewGuid().ToString();
            var sut = new DomainName(domainName);
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            int expectedHashCode = domainName.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }
}