using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UriGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new UriGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new UriGenerator();
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
            var sut = new UriGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonUriRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new UriGenerator();
            var dummyRequest = new object();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWhenUriSchemeReceivedFromContextIsNullReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(Uri);
            object expectedValue = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(UriScheme).Equals(r) ? expectedValue : new NoSpecimen()
            };
            var sut = new UriGenerator();
            // Exercise system and verify outcome
            var result = sut.Create(request, context);
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWhenStringReceivedFromContextIsNullReturnsCorrectResult()
        {
            // Fixture setup
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
            // Exercise system and verify outcome
            var result = sut.Create(request, context);
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsUriWithSchemeReceivedFromContext()
        {
            // Fixture setup
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
            // Exercise system
            var result = (Uri)sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedScheme, result.Scheme);
            // Teardown
        }

        [Fact]
        public void CreateReturnsUriWithAuthorityReceivedFromContext()
        {
            // Fixture setup
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
            // Exercise system
            var result = (Uri)sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedAuthority, result.Authority);
            // Teardown
        }

        [Fact]
        public void CreateWithUriRequestReturnsCorrectResult()
        {
            // Fixture setup
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
            // Exercise system
            var result = (Uri)sut.Create(request, context);
            // Verify outcome
            var expectedUri = new Uri(expectedUriScheme + "://" + expectedAuthority);
            Assert.Equal(expectedUri, result);
            // Teardown
        }
    }
}
