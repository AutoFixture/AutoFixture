using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DomainNameGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange

            // Act
            var sut = new DomainNameGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new DomainNameGenerator();
            // Act
            var result = sut.Create(null, null);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNonDomainNameRequestReturnsCorrectResult()
        {
            // Arrange
            var nonDomainNameRequest = typeof(object);
            var sut = new DomainNameGenerator();
            // Act
            var result = sut.Create(nonDomainNameRequest, null);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateReturnsOneOfTheFictiousDomains()
        {
            // Arrange
            var sut = new DomainNameGenerator();
            // Act
            var result = sut.Create(typeof(DomainName), null);
            // Assert
            var actualDomainName = Assert.IsAssignableFrom<DomainName>(result);
            Assert.Matches(@"example\.(com|org|net)", actualDomainName.Domain);
        }

        [Fact]
        public void CreateManyTimesReturnsAllConfiguredFictiousDomains()
        {
            // Arrange
            var sut = new DomainNameGenerator();
            var expectedDomains = new[] { "example.com", "example.net", "example.org" }.Select(x => new DomainName(x)).ToList();
            // Act
            var result = Enumerable.Range(0, 100).Select(x => sut.Create(typeof(DomainName), null)).ToList();
            // Assert
            foreach (var expectedDomain in expectedDomains)
            {
                Assert.Contains(expectedDomain, result);
            }
        }
    }
}