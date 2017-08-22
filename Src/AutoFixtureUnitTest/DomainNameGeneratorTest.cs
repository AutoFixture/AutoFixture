using System.Linq;
using System.Text.RegularExpressions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DomainNameGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup

            // Exercise system
            var sut = new DomainNameGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new DomainNameGenerator();
            // Exercise system
            var result = sut.Create(null, null);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNonDomainNameRequestReturnsCorrectResult()
        {
            // Fixture setup
            var nonDomainNameRequest = typeof(object);
            var sut = new DomainNameGenerator();
            // Exercise system
            var result = sut.Create(nonDomainNameRequest, null);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsOneOfTheFictiousDomains()
        {
            // Fixture setup
            var sut = new DomainNameGenerator();
            // Exercise system
            var result = sut.Create(typeof(DomainName), null);
            // Verify outcome
            var actualDomainName = Assert.IsAssignableFrom<DomainName>(result);
            Assert.Matches(@"example\.(com|org|net)", actualDomainName.Domain);
            // Teardown
        }

        [Fact]
        public void CreateManyTimesReturnsAllConfiguredFictiousDomains()
        {
            // Fixture setup
            var sut = new DomainNameGenerator();
            var expectedDomains = new[] {"example.com", "example.net", "example.org"}.Select(x => new DomainName(x)).ToList();
            // Exercise system
            var result = Enumerable.Range(0, 100).Select(x => sut.Create(typeof(DomainName), null)).ToList();
            // Verify outcome
            foreach (var expectedDomain in expectedDomains)
            {
                Assert.Contains(expectedDomain, result);
            }
            // Teardown
        }
    }
}