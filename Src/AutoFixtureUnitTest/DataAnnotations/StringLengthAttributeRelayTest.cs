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
        public void SutInheritsCommonLogicFromAttributeRelayWhichIsTestedSeparately()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringLengthAttributeRelay();
            // Verify outcome
            Assert.IsAssignableFrom<AttributeRelay<StringLengthAttribute>>(sut);
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
    }
}
