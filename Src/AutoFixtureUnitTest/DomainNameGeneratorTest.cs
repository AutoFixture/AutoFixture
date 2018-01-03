using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
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
            var context = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(null, context);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrowsException()
        {
            // Arrange
            var sut = new DomainNameGenerator();
            var request = new object();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(request, context: null));
        }

        [Fact]
        public void CreateWithNonDomainNameRequestReturnsCorrectResult()
        {
            // Arrange
            var nonDomainNameRequest = typeof(object);
            var context = new DelegatingSpecimenContext();
            var sut = new DomainNameGenerator();
            // Act
            var result = sut.Create(nonDomainNameRequest, context);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateReturnsOneOfTheFictiousDomains()
        {
            // Arrange
            var sut = new DomainNameGenerator();
            var context = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(typeof(DomainName), context);
            // Assert
            var actualDomainName = Assert.IsAssignableFrom<DomainName>(result);
            Assert.Matches(@"example\.(com|org|net)", actualDomainName.Domain);
        }

        [Fact]
        public void CreateManyTimesReturnsAllConfiguredFictiousDomains()
        {
            // Arrange
            var sut = new DomainNameGenerator();
            var context = new DelegatingSpecimenContext();
            var expectedDomains = new[] { "example.com", "example.net", "example.org" }.Select(x => new DomainName(x));
            // Act
            var result = Enumerable.Range(0, 100).Select(x => sut.Create(typeof(DomainName), context)).ToList();
            // Assert
            foreach (var expectedDomain in expectedDomains)
            {
                Assert.Contains(expectedDomain, result);
            }
        }
    }
}