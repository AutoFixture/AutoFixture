using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringLengthAttributeRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutReusesLogicOfAttributeRelayWhichIsTestedSeparately()
        {
            //Fixture setup
            var sut = new TestableStringLengthAttributeRelay();
            // Exercise system
            // Verify outcome
            var relayBuilder = Assert.IsAssignableFrom<AttributeRelay<StringLengthAttribute>>(sut.RelayBuilder);
            Assert.Same(
                typeof(StringLengthAttributeRelay).GetMethod("CreateRelayedRequest", BindingFlags.Static | BindingFlags.NonPublic),
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
            var sut = new TestableStringLengthAttributeRelay { RelayBuilder = relayBuilder };
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateWithConstrainedStringRequestReturnsCorrectResult(int maximum)
        {
            // Fixture setup
            var stringLengthAttribute = new StringLengthAttribute(maximum);
            var providedAttribute = new ProvidedAttribute(stringLengthAttribute, true);
            ICustomAttributeProvider request = new FakeCustomAttributeProvider(providedAttribute);
            var expectedRequest = new ConstrainedStringRequest(stringLengthAttribute.MaximumLength);
            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen(r)
            };
            var sut = new StringLengthAttributeRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private class TestableStringLengthAttributeRelay : StringLengthAttributeRelay
        {
            private static readonly FieldInfo relayBuilder = typeof(StringLengthAttributeRelay)
                .GetField("relayBuilder", BindingFlags.NonPublic | BindingFlags.Instance);

            public ISpecimenBuilder RelayBuilder
            {
                get { return (ISpecimenBuilder)relayBuilder.GetValue(this); }
                set { relayBuilder.SetValue(this, value); }
            }
        }
    }
}
