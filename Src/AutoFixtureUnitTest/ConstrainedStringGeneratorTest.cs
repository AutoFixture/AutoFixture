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

        [Theory]
        [MemberData(nameof(MinimumLengthMaximumLengthTestCases))]
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

        [Theory]
        [MemberData(nameof(MinimumLengthMaximumLengthTestCases))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters - the minLength is needed to access the maxLenght.
        public void CreateReturnsStringWithCorrectLengthMultipleCall(int minimumLength, int maximumLength)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
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

        public static TheoryData<int, int> MinimumLengthMaximumLengthTestCases =>
            new TheoryData<int, int>
            {
                { 0, 3 },
                { 0, 10 },
                { 0, 20 },
                { 0, 30 },
                { 0, 60 },
                { 0, 90 },
                { 3, 90 },
                { 10, 100 },
                { 20, 120 },
                { 30, 130 },
                { 60, 160 },
                { 90, 190 },
                { 100, 200 },
                { 120, 210 },
                { 130, 220 },
                { 160, 230 },
                { 190, 240 }
            };
    }
}