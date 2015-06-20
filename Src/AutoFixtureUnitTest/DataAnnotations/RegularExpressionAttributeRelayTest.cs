using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RegularExpressionAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RegularExpressionAttributeRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutReusesLogicOfAttributeRelayWhichIsTestedSeparately()
        {
            //Fixture setup
            var sut = new TestableRegularExpressionAttributeRelay();
            // Verify outcome
            var relayBuilder = Assert.IsAssignableFrom<AttributeRelay<RegularExpressionAttribute>>(sut.RelayBuilder);
            Assert.Same(
                typeof(RegularExpressionAttributeRelay).GetMethod("CreateRelayedRequest", BindingFlags.Static | BindingFlags.NonPublic),
                relayBuilder.CreateRelayedRequestMethod);
            // Teardown
        }

        [Fact]
        public void CreateReturnsObjectCreatedByRelayBuilderBasedOnGivenRequestAndContext()
        {
            // Fixture setup
            object relayRequest = null;
            ISpecimenContext relayContext = null;
            var relayResult = new object();
            var relayBuilder = new DelegatingSpecimenBuilder();
            relayBuilder.OnCreate = (r, c) =>
            {
                relayRequest = r;
                relayContext = c;
                return relayResult;
            };
            var sut = new TestableRegularExpressionAttributeRelay { RelayBuilder = relayBuilder };
            // Exercise system
            var actualRequest = new object();
            var actualContext = new DelegatingSpecimenContext();
            object actualResult = sut.Create(actualRequest, actualContext);
            // Verify outcome
            Assert.Same(actualRequest, relayRequest);
            Assert.Same(actualContext, relayContext);
            Assert.Same(actualResult, relayResult);
            // Teardown            
        }

        [Theory]
        [InlineData("[0-9]")]
        [InlineData("[A-Z]")]
        [InlineData("[a-z]")]
        public void CreateWithRegularExpressionAttributeRequestReturnsCorrectResult(string pattern)
        {
            // Fixture setup
            var regularExpressionAttribute = new RegularExpressionAttribute(pattern);
            var providedAttribute = new ProvidedAttribute(regularExpressionAttribute, true);
            ICustomAttributeProvider request = new FakeCustomAttributeProvider(providedAttribute);
            var expectedRequest = new RegularExpressionRequest(regularExpressionAttribute.Pattern);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
            };
            var sut = new RegularExpressionAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private class TestableRegularExpressionAttributeRelay : RegularExpressionAttributeRelay
        {
            private static readonly FieldInfo relayBuilder = typeof(RegularExpressionAttributeRelay)
                .GetField("relayBuilder", BindingFlags.NonPublic | BindingFlags.Instance);

            public ISpecimenBuilder RelayBuilder
            {
                get { return (ISpecimenBuilder)relayBuilder.GetValue(this); }
                set { relayBuilder.SetValue(this, value); }
            }
        }
    }
}
