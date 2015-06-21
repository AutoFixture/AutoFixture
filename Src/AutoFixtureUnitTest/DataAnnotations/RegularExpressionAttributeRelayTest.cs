using System;
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
        public void SutInheritsCommonLogicFromAttributeRelayWhichIsTestedSeparately()
        {
            // Fixture setup
            // Exercise system
            var sut = new RegularExpressionAttributeRelay();
            // Verify outcome
            Assert.IsAssignableFrom<AttributeRelay<RegularExpressionAttribute>>(sut);
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
    }
}
