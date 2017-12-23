#if SYSTEM_NET_MAIL

using System;
using System.Net.Mail;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class MailAddressGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new MailAddressGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new MailAddressGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNonMailAddressRequestReturnsCorrectResult()
        {
            // Arrange
            var request = new object();
            var sut = new MailAddressGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var request = typeof(MailAddress);
            var sut = new MailAddressGenerator();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsCorrectResultUsingLocalPartAndDomainNameFromContext()
        {
            // Arrange
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
            // Act
            var result = (MailAddress)sut.Create(request, context);
            // Assert
            Assert.Equal(expectedLocalPart.LocalPart, result.User);
            Assert.Equal(expectedDomainName.Domain, result.Host);
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsNoSpecimenWhenContextReturnsNullLocalPart()
        {
            // Arrange
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
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithMailAddressRequestReturnsNoSpecimenWhenContextReturnsNullDomainName()
        {
            // Arrange
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
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenEmailAddressLocalPartIsInvalidForMailAddress()
        {
            // Arrange
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
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}

#endif
