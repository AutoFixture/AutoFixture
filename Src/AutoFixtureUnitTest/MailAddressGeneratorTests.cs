using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MailAddressGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MailAddressGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MailAddressGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNonMailAddressRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var sut = new MailAddressGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var request = typeof(MailAddress);
            var sut = new MailAddressGenerator();            
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
            // Teardown
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsCorrectResultUsingLocalPartFromContext()
        {
            // Fixture setup
            var request = typeof(MailAddress);
            var expectedLocalPart = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
               {
                   Assert.Equal(typeof(EmailAddressLocalPart), r);
                   return expectedLocalPart;
               }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var result = (MailAddress)sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedLocalPart.LocalPart, result.User);
            Assert.True(Regex.IsMatch(result.Host, @"example\.(com|org|net)"));
            // Teardown
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsNoSpecimenWhenContextReturnsNullLocalPart()
        {
            // Fixture setup
            var request = typeof(MailAddress);         
            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
                {
                    Assert.Equal(typeof(EmailAddressLocalPart), r);
                    return null;                    
                }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var expectedResult = new NoSpecimen(request);
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenEmailAddressLocalPartIsInvalidForMailAddress()
        {
            // Fixture setup
            var localPart = new EmailAddressLocalPart("@Invalid@");
            var request = typeof(MailAddress);
            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
                {
                    Assert.Equal(typeof(EmailAddressLocalPart), r);
                    return localPart;
                }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var expectedResult = new NoSpecimen(request);
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
