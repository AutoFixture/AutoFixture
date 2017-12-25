using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class ConstrainedStringGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new ConstrainedStringGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new ConstrainedStringGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new ConstrainedStringGenerator();
            var request = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new ConstrainedStringGenerator();
            var request = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var result = sut.Create(request, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateReturnsResultWithCorrectType()
        {
            // Arrange
            var request = new ConstrainedStringRequest(1, 10);
            Type expectedType = typeof(string);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedType.Equals(r) ? contextValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsAssignableFrom(expectedType, result);
        }

        [Fact]
        public void CreateReturnsStringReceivedFromContext()
        {
            // Arrange
            var request = new ConstrainedStringRequest(1, 10);
            object expectedValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? expectedValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Act & assert
            var result = (string)sut.Create(request, context);
            Assert.Contains(result, expectedValue.ToString());
        }

        [Theory]
        [MemberData(nameof(MinimumLengthMaximumLengthTestCases))]
        public void CreateReturnsStringWithCorrectLength(int expectedMinimumLength, int expectedMaximumLength)
        {
            // Arrange
            var request = new ConstrainedStringRequest(expectedMinimumLength, expectedMaximumLength);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? contextValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Act
            var result = (string)sut.Create(request, context);
            // Assert
            Assert.True(expectedMinimumLength < result.Length && expectedMaximumLength >= result.Length);
        }

        [Theory]
        [MemberData(nameof(MinimumLengthMaximumLengthTestCases))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters - the minLength is needed to access the maxLenght.
        public void CreateReturnsStringWithCorrectLengthMultipleCall(int minimumLength, int maximumLength)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            // Arrange
            var request = new ConstrainedStringRequest(maximumLength);
            object contextValue = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => typeof(string).Equals(r) ? contextValue : new NoSpecimen()
            };
            var sut = new ConstrainedStringGenerator();
            // Act
            var result = (from s in Enumerable.Range(1, 30).Select(i => (string)sut.Create(request, context))
                          where (s.Length <= request.MinimumLength || s.Length > request.MaximumLength)
                          select s);
            // Assert
            Assert.False(result.Any());
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