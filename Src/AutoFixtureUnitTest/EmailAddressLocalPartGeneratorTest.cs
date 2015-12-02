using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

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
#pragma warning disable 618
            var expectedResult = new NoSpecimen(dummyRequest);
#pragma warning restore 618
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
                OnResolve = r =>
                {
                    Assert.Equal(typeof(string), r);
                    return expectedValue;
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system and verify outcome
            var result = sut.Create(request, context);
#pragma warning disable 618
            var expectedResult = new NoSpecimen(request);
#pragma warning restore 618
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
                        Assert.Equal(typeof(string), r);
                        return expectedLocalPart;
                    }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system
            var result = sut.Create(typeof(EmailAddressLocalPart), context) as EmailAddressLocalPart;
            // Verify outcome
            Assert.Equal(expectedLocalPart, result.LocalPart);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateReturnsNoSpecimenWhenContextCreatesInvalidLocalPartString(string invalidLocalPart)
        {
            // Fixture setup
            var request = typeof(EmailAddressLocalPart);            

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(typeof(string), r);
                    return invalidLocalPart;
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Exercise system
            var result = sut.Create(typeof(EmailAddressLocalPart), context);
            // Verify outcome
#pragma warning disable 618
            var expectedResult = new NoSpecimen(request);
#pragma warning restore 618
            Assert.Equal(expectedResult, result);
            // Teardown
        }

    }
}
