using System;
using AutoFixture;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class EmailAddressLocalPartTest
    {
        [Fact]
        public void InitializeWithNullLocalPartThrows()
        {          
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new EmailAddressLocalPart(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithEmptyLocalPartThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(
                () => new EmailAddressLocalPart(string.Empty));
            // Teardown
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            string expected = Guid.NewGuid().ToString();
            var sut = new EmailAddressLocalPart(expected);
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
            var sut = new EmailAddressLocalPart(Guid.NewGuid().ToString());
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
            var sut = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            var anonymousObject = new object();
            // Exercise system
            bool result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenLocalPartsDiffer()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            object other = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenLocalPartsAreEqual()
        {
            // Fixture setup
            var localPart = Guid.NewGuid().ToString();

            var sut = new EmailAddressLocalPart(localPart);
            var other = new EmailAddressLocalPart(localPart);
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
            var localPart = Guid.NewGuid().ToString();
            var sut = new EmailAddressLocalPart(localPart);
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            int expectedHashCode = localPart.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }    
}
