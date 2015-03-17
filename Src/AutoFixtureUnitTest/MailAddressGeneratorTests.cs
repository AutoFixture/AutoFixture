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
        public void CreateWithMailAddressRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(MailAddress);
            var context = new DelegatingSpecimenContext();
            var sut = new MailAddressGenerator();
            // Exercise system
            var result = (MailAddress)sut.Create(request, context);
            // Verify outcome
            Assert.True(Regex.IsMatch(result.Host, @"example\.(com|org|net)"));
            // Teardown
        }
    }
}
