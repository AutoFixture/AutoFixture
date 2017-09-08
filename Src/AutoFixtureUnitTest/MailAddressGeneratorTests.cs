#if SYSTEM_NET_MAIL

using System;
using System.Net.Mail;
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
            var expectedResult = new NoSpecimen();
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
        public void CreateWithMailAddressRequestReturnsCorrectResultUsingLocalPartAndDomainNameFromContext()
        {
            // Fixture setup
            var request = typeof(MailAddress);
            var expectedLocalPart = new EmailAddressLocalPart(Guid.NewGuid().ToString());
            var expectedDomainName = new DomainName(Guid.NewGuid().ToString());
            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
               {
                   Assert.True(typeof(EmailAddressLocalPart).Equals(r) || typeof(DomainName).Equals(r));
                   if (typeof(EmailAddressLocalPart).Equals(r))
                   {
                       return expectedLocalPart;
                   }
                   else
                   {
                       return expectedDomainName;
                   }
               }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var result = (MailAddress)sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedLocalPart.LocalPart, result.User);
            Assert.Equal(expectedDomainName.Domain, result.Host);
            // Teardown
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsNoSpecimenWhenContextReturnsNullLocalPart()
        {
            // Fixture setup
            var request = typeof(MailAddress);
            var anonymousDomainName = new DomainName(Guid.NewGuid().ToString());

            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
                {
                    Assert.True(typeof(EmailAddressLocalPart).Equals(r) || typeof(DomainName).Equals(r));
                    return typeof(DomainName).Equals(r) ? anonymousDomainName : null;
                }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsNoSpecimenWhenContextReturnsNullDomainName()
        {
            // Fixture setup
            var request = typeof(MailAddress);
            var anonymousLocalPart = new EmailAddressLocalPart(Guid.NewGuid().ToString());

            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
                {
                    Assert.True(typeof(EmailAddressLocalPart).Equals(r) || typeof(DomainName).Equals(r));
                    return typeof(EmailAddressLocalPart).Equals(r) ? anonymousLocalPart : null;
                }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenEmailAddressLocalPartIsInvalidForMailAddress()
        {
            // Fixture setup
            var localPart = new EmailAddressLocalPart("@Invalid@");
            var anonymousDomainName = new DomainName(Guid.NewGuid().ToString());
            var request = typeof(MailAddress);
            var context = new DelegatingSpecimenContext()
            {
                OnResolve = r =>
                {
                    Assert.True(typeof(EmailAddressLocalPart).Equals(r) || typeof(DomainName).Equals(r));
                    if (typeof(EmailAddressLocalPart).Equals(r))
                    {
                        return localPart;
                    }
                    else
                    {
                        return anonymousDomainName;
                    }
                }
            };
            var sut = new MailAddressGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}

#endif
