using System;
using System.Collections;
using System.Collections.Generic;
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
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsResultWithCorrectType()
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(1, 10);
            Type expectedType = typeof(string);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? contextValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsAssignableFrom(expectedType, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsStringReceivedFromContext()
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(1, 10);
            object expectedValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? expectedValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system and verify outcome
            var result = (string)sut.Create(request, context);
            Assert.Contains(result, expectedValue.ToString());
            // Teardown
        }

        [Theory, ClassData(typeof(MinimumLengthMaximumLengthTestCases))]
        public void CreateReturnsStringWithCorrectLength(int expectedMinimumLength, int expectedMaximumLength)
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(expectedMinimumLength, expectedMaximumLength);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? contextValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (string)sut.Create(request, context);
            // Verify outcome
            Assert.True(expectedMinimumLength < result.Length && expectedMaximumLength >= result.Length);
            // Teardown
        }

        [Theory, ClassData(typeof(MinimumLengthMaximumLengthTestCases))]
        public void CreateReturnsStringWithCorrectLengthMultipleCall(int minimumLength, int maximumLength)
        {
            // Fixture setup
            var request = new ConstrainedStringRequest(maximumLength);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? contextValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Exercise system
            var result = (from s in Enumerable.Range(1, 30).Select(i => (string)sut.Create(request, context))
                          where (s.Length <= request.MinimumLength || s.Length > request.MaximumLength)
                          select s);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        private sealed class MinimumLengthMaximumLengthTestCases : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {   0,   3 };
                yield return new object[] {   0,  10 };
                yield return new object[] {   0,  20 };
                yield return new object[] {   0,  30 };
                yield return new object[] {   0,  60 };
                yield return new object[] {   0,  90 };
                yield return new object[] {   3,  90 };
                yield return new object[] {  10, 100 };
                yield return new object[] {  20, 120 };
                yield return new object[] {  30, 130 };
                yield return new object[] {  60, 160 };
                yield return new object[] {  90, 190 };
                yield return new object[] { 100, 200 };
                yield return new object[] { 120, 210 };
                yield return new object[] { 130, 220 };
                yield return new object[] { 160, 230 };
                yield return new object[] { 190, 240 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}