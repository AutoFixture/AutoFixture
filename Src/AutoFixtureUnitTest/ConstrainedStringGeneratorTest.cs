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

        [Fact]
        public void CreateReturnsResultWithCorrectType()
        {
            // Fixture setup
            int maximumLength = new Random().Next(1, 10);
            var expectedRequest = new ConstrainedStringRequest(maximumLength);
            Type expectedType = typeof(string);
            object expectedResult = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? expectedResult : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = sut.Create(expectedRequest, context);
            // Verify outcome
            Assert.IsAssignableFrom(expectedType, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsStringReceivedFromContext()
        {
            // Fixture setup
            int maximumLength = new Random().Next(1, 10);
            var expectedRequest = new ConstrainedStringRequest(maximumLength);
            Type expectedType = typeof(string);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? contextValue : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system and verify outcome
            var result = (string)sut.Create(expectedRequest, context);
            Assert.True(contextValue.ToString().Contains(result));
            // Teardown
        }

        [Fact]
        public void CreateReturnsStringWithCorrectLength()
        {
            // Fixture setup
            int expectedMaximumLength = new Random().Next(1, 10);
            var expectedRequest = new ConstrainedStringRequest(expectedMaximumLength);
            Type expectedType = typeof(string);
            object expectedResult = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? expectedResult : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (string)sut.Create(expectedRequest, context);
            // Verify outcome
            Assert.Equal(expectedMaximumLength, result.Length);
            // Teardown
        }

        [Theory]
        [InlineData(5)]
        [InlineData(2)]
        [InlineData(9)]
        public void CreateReturnsStringWithCorrectLengthMultipleCall(int loopCount)
        {
            // Fixture setup
            int maximumLength = new Random().Next(1, 10);
            var expectedRequest = new ConstrainedStringRequest(maximumLength);
            Type expectedType = typeof(string);
            object expectedResult = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? expectedResult : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (from s in Enumerable.Range(1, loopCount).Select(i => (string)sut.Create(expectedRequest, context))
                          where (s.Length > expectedRequest.MaximumLength)
                          select s);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}