using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class EmailAddressLocalPartTest
    {
        [Fact]
        public void InitializeWithNullLocalPartThrows()
        {
            var sut = new EmailAddressLocalPart("good.localPart");

            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new EmailAddressLocalPart(null));
            // Teardown
        }

        [Theory]
        [InlineData(@"@CantStartWithAtSign")]
        [InlineData(@".StartsWithPeriod")]
        [InlineData(@"Has..TwoPeriods")]
        [InlineData(@"EndsWithPeriod.")]
        [InlineData(@".")]
        [InlineData(@"Ima Notquoted")]
        public void InitializeWithInvalidLocalPartThrows(string localPart)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(
                () => new EmailAddressLocalPart(localPart));
            // Teardown
        }

        [Theory]        
        [InlineData(@"""Quoted\\WithSlash""")]
        [InlineData("\"Quoted\\\rWithLineBreak\"")]
        [InlineData(@"""Both\""Quoted""")]
        [InlineData(@"service/department")]
        [InlineData(@"$StartWithDollar")]
        [InlineData(@"!def!xyz%abc")]
        [InlineData(@"_Good.Email")]
        [InlineData(@"~")]
        [InlineData(@"""Quoted@AtSign""")]
        [InlineData(@"Ima.InternalPeriod")]
        [InlineData(@"""Ima.Quoted""")]
        [InlineData(@"""Ima Quoted""")]
        public void InitializeWithValidLocalPartSetsLocalPart(string localPart)
        {
            // Fixture setup            
            var sut = new EmailAddressLocalPart(localPart);
            // Exercise system
            string result = sut.LocalPart;
            // Verify outcome
            Assert.Equal(localPart, result);
            // Teardown
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            string expected = "good.localPart";
            var sut = new EmailAddressLocalPart("good.localPart");
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new EmailAddressLocalPart("good.localPart");
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<EmailAddressLocalPart>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPart("good.localPart");
            object other = null;
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPart("good.localPart");
            UriScheme other = null;
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
            var sut = new EmailAddressLocalPart("good.localPart");
            var anonymousObject = new object();
            // Exercise system
            bool result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWhenSchemesDiffer()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPart("good.localPart");
            object other = new EmailAddressLocalPart("good2.localPart");
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWhenSchemesDiffer()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPart("good.localPart");
            var other = new EmailAddressLocalPart("good2.localPart");
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }      


        [Fact]
        public void GetHashCodeWhenSchemeIsNotDefaultReturnsCorrectResult()
        {
            // Fixture setup
            var localPart = "good.localPart";
            var sut = new UriScheme(localPart);
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            int expectedHashCode = localPart.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }
    }    
}
