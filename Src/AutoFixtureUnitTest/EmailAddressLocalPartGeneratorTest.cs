using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class EmailAddressLocalPartGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new EmailAddressLocalPartGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPartGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonEmailAddressLocalPartRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new EmailAddressLocalPartGenerator();
            var dummyRequest = new object();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(dummyRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWhenLocalPartReceivedFromContextIsNullReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(EmailAddressLocalPart);
            object expectedValue = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? expectedValue : new NoSpecimen(r)
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system and verify outcome
            var result = sut.Create(request, context);
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsEmailAddressLocalPartUsingLocalPartReceivedFromContext()
        {
            // Fixture setup
            var request = typeof(EmailAddressLocalPart);
            string expectedLocalPart = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(string).Equals(r))
                    {
                        return expectedLocalPart;
                    }

                    return new NoSpecimen(r);
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system
            var result = sut.Create(typeof(EmailAddressLocalPart), context) as EmailAddressLocalPart;
            // Verify outcome
            Assert.Equal(expectedLocalPart, result.LocalPart);
            // Teardown
        }

        [Fact]
        public void CreateReturnsEmailAddressLocalPartWithCorrectlyTruncatedLocalPartFromContext()
        {
            // Fixture setup
            var request = typeof(EmailAddressLocalPart);
            string contextLocalPart = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            string expectedLocalPart = contextLocalPart.Substring(0, 64);

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(string).Equals(r))
                    {
                        return contextLocalPart;
                    }

                    return new NoSpecimen(r);
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system
            var result = sut.Create(typeof(EmailAddressLocalPart), context) as EmailAddressLocalPart;
            // Verify outcome
            Assert.Equal(expectedLocalPart, result.LocalPart);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenContextCreatesInvalidLocalPartString()
        {
            // Fixture setup
            var request = typeof(EmailAddressLocalPart);
            var invalidLocalPart = "@@Invalid";

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(string).Equals(r))
                    {
                        return invalidLocalPart;
                    }

                    return new NoSpecimen(r);
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system
            var result = sut.Create(typeof(EmailAddressLocalPart), context);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

    }
}
