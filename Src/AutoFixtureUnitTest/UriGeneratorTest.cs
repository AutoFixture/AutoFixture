using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UriGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new UriGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new UriGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new UriGenerator();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithNonUriRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new UriGenerator();
            var dummyRequest = new object();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWhenUriSchemeReceivedFromContextIsNullReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(Uri);
            object expectedValue = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(UriScheme).Equals(r) ? expectedValue : new NoSpecimen()
            };
            var sut = new UriGenerator();
            // Act & assert
            var result = sut.Create(request, context);
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWhenStringReceivedFromContextIsNullReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(Uri);
            object expectedValue = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(UriScheme).Equals(r))
                    {
                        return new UriScheme();
                    }

                    if (typeof(string).Equals(r))
                    {
                        return expectedValue;
                    }

                    return new NoSpecimen();
                }
            };
            var sut = new UriGenerator();
            // Act & assert
            var result = sut.Create(request, context);
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateReturnsUriWithSchemeReceivedFromContext()
        {
            // Arrange
            var request = typeof(Uri);
            string expectedScheme = "https";
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(UriScheme).Equals(r))
                    {
                        return new UriScheme(expectedScheme);
                    }

                    if (typeof(string).Equals(r))
                    {
                        return Guid.NewGuid().ToString();
                    }

                    return new NoSpecimen();
                }
            };
            var sut = new UriGenerator();
            // Act
            var result = (Uri)sut.Create(request, context);
            // Assert
            Assert.Equal(expectedScheme, result.Scheme);
        }

        [Fact]
        public void CreateReturnsUriWithAuthorityReceivedFromContext()
        {
            // Arrange
            var request = typeof(Uri);
            object expectedAuthority = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(UriScheme).Equals(r))
                    {
                        return new UriScheme();
                    }

                    if (typeof(string).Equals(r))
                    {
                        return expectedAuthority;
                    }

                    return new NoSpecimen();
                }
            };
            var sut = new UriGenerator();
            // Act
            var result = (Uri)sut.Create(request, context);
            // Assert
            Assert.Equal(expectedAuthority, result.Authority);
        }

        [Fact]
        public void CreateWithUriRequestReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(Uri);
            object expectedUriScheme = new UriScheme("ftp");
            object expectedAuthority = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    if (typeof(UriScheme).Equals(r))
                    {
                        return expectedUriScheme;
                    }

                    if (typeof(string).Equals(r))
                    {
                        return expectedAuthority;
                    }

                    return new NoSpecimen();
                }
            };
            var sut = new UriGenerator();
            // Act
            var result = (Uri)sut.Create(request, context);
            // Assert
            var expectedUri = new Uri(expectedUriScheme + "://" + expectedAuthority);
            Assert.Equal(expectedUri, result);
        }
    }
}
