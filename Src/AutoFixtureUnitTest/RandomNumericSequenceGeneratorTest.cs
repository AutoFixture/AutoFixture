using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomNumericSequenceGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomNumericSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            IEnumerable<int> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullEnumerable));
            // Teardown
        }

        [Fact]
        public void BoundariesMatchListParameter()
        {
            // Fixture setup
            IEnumerable<int> expectedBoundaries = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue 
            }.AsEnumerable();
            var sut = new RandomNumericSequenceGenerator(expectedBoundaries);
            // Exercise system
            IEnumerable<int> result = sut.Boundaries;
            // Verify outcome
            Assert.True(expectedBoundaries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            int[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new RandomNumericSequenceGenerator(nullArray));
            // Teardown
        }

        [Fact]
        public void BoundariesMatchParamsArray()
        {
            // Fixture setup
            int[] expectedBoundaries = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue
            };
            var sut = new RandomNumericSequenceGenerator(expectedBoundaries);
            // Exercise system
            IEnumerable<int> result = sut.Boundaries;
            // Verify outcome
            Assert.True(expectedBoundaries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void BoundariesDoesNotMatchParamsArray()
        {
            // Fixture setup
            int[] expectedBoundaries = new[]
            { 
                Byte.MaxValue, 
                Int16.MaxValue, 
                Int32.MaxValue
            };
            var sut = new RandomNumericSequenceGenerator(
                expectedBoundaries[0],
                expectedBoundaries[2],
                expectedBoundaries[1]
                );
            // Exercise system
            IEnumerable<int> result = sut.Boundaries;
            // Verify outcome
            Assert.False(expectedBoundaries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsNoSpecimen()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            object result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.DoesNotThrow(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(default(bool))]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new RandomNumericSequenceGenerator();
            var expectedResult = new NoSpecimen(request);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            object result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}