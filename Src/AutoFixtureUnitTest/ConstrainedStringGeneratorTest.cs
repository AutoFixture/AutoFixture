using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ConstrainedStringGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ConstrainedStringGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ConstrainedStringGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new ConstrainedStringGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ConstrainedStringGenerator();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(dummyRequest), result);
            // Teardown
        }

        [Theory]
        [InlineData(2, 5)]
        [InlineData(5, 2)]
        [InlineData(9, 9)]
        public void CreateReturnsCorrectResultOnMultipleCall(int maximumLength, int loopCount)
        {
            // Fixture setup
            var dummyString = Guid.NewGuid().ToString();
            var request = new ConstrainedStringRequest(maximumLength);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(request.MaximumLength, maximumLength);
                    Assert.Equal(typeof(string), r);
                    return dummyString;
                }
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (from n in Enumerable.Range(1, loopCount)
                              .Select(i =>
                              {
                                  string s = (string)sut.Create(request, context);
                                  Assert.True(dummyString.Contains(s));
                                  return s.Length;
                              })
                          where (n > request.MaximumLength)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}