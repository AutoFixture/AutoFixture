using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;
using System.Collections.Generic;
using System.Collections;

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
            var request = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ConstrainedStringGenerator();
            var request = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(request), result);
            // Teardown
        }

        [Theory, ClassData(typeof(MaximumLengthTestCases))]
        public void CreateReturnsResultWithCorrectType(int maximumLength)
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(maximumLength);
            Type expectedType = typeof(string);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? contextValue : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsAssignableFrom(expectedType, result);
            // Teardown
        }

        [Theory, ClassData(typeof(MaximumLengthTestCases))]
        public void CreateReturnsStringReceivedFromContext(int maximumLength)
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(maximumLength);
            object expectedValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? expectedValue : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system and verify outcome
            var result = (string)sut.Create(request, context);
            Assert.True(expectedValue.ToString().Contains(result));
            // Teardown
        }

        [Theory, ClassData(typeof(MaximumLengthTestCases))]
        public void CreateReturnsStringWithCorrectLength(int expectedMaximumLength)
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(expectedMaximumLength);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? contextValue : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (string)sut.Create(request, context);
            // Verify outcome
            Assert.True(result.Length <= expectedMaximumLength);
            // Teardown
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(3, 30)]
        [InlineData(10, 9)]
        [InlineData(30, 8)]
        [InlineData(60, 7)]
        public void CreateReturnsStringWithCorrectLengthMultipleCall(int maximumLength, int loopCount)
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(maximumLength);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? contextValue : new NoSpecimen(r)
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (from s in Enumerable.Range(1, loopCount).Select(i => (string)sut.Create(request, context))
                          where (s.Length > request.MaximumLength)
                          select s);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        private sealed class MaximumLengthTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 3 };
                yield return new object[] { 10 };
                yield return new object[] { 20 };
                yield return new object[] { 30 };
                yield return new object[] { 60 };
                yield return new object[] { 90 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}