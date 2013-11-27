using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RandomRangedNumberGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RandomRangedNumberGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RandomRangedNumberGenerator();
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
            var sut = new RandomRangedNumberGenerator();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RandomRangedNumberGenerator();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(dummyRequest, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(dummyRequest), result);
            // Teardown
        }


        [Theory]
        [InlineData(typeof(int), "a", "b")]
        [InlineData(typeof(long), 'd', 'e')]
        public void CreateWithNonnumericLimitsReturnsNoSpecimen(Type operandType, object minimum, object maximum)            
        {
            // Fixture setup
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            var request = new RangedNumberRequest(operandType, minimum, maximum);
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify
            Assert.Equal(new NoSpecimen(request), result);
        }
        
      
        [Fact]
        public void CreateReturnsAllValuesInSetBeforeRepeating()
        {
            // Fixture setup
            int minimum = 0;
            int maximum = 2;

            var request = new RangedNumberRequest(typeof(int), minimum, maximum);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            var generatedValues = Enumerable.Range(0, 3).Select(i => sut.Create(request, dummyContext)).Cast<int>();
            var shouldBeRepeatedValue = (int)sut.Create(request, dummyContext);

            // Verify
            Assert.True(generatedValues.All(a => a >= minimum && a <= maximum));
            Assert.InRange(shouldBeRepeatedValue, minimum, maximum);

        }

        [Fact]
        public void CreateReturnsRandomDifferentValuesForRequestsOfSameTypeAndDifferentMaximums()
        {
            // Fixture setup
            int minimum = 0;
            int baseMaximum = 2;
            int totalValuesToRequest = baseMaximum*3+3;
            var request1 = new RangedNumberRequest(typeof(int), minimum, baseMaximum);
            var request2 = new RangedNumberRequest(typeof(int), minimum, baseMaximum+1);
            var request3 = new RangedNumberRequest(typeof(int), minimum, baseMaximum+2);

            IDictionary<RangedNumberRequest, IList<int>> results = new Dictionary<RangedNumberRequest, IList<int>>()
            {
                { request1, new List<int>() }, { request2, new List<int>() }, { request3, new List<int>() }
            };

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();
            
            // Exercise system
            results[request1].Add((int)sut.Create(request1, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request1].Add((int)sut.Create(request1, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request1].Add((int)sut.Create(request1, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));

            // Verify
            Assert.True(Enumerable.Range(0, baseMaximum+1).All(a => results[request1].Contains(a)));
            Assert.True(Enumerable.Range(0, baseMaximum+2).All(a => results[request2].Contains(a)));
            Assert.True(Enumerable.Range(0, baseMaximum+3).All(a => results[request3].Contains(a)));
        }

        [Fact]
        public void CreateReturnsRandomDifferentValuesForRequestsOfSameTypeAndDifferentMinimums()
        {
            // Fixture setup
            int baseMinimum = 0;
            int maximum = 2;
            int totalValuesToRequest = maximum * 3 + 3;
            var request1 = new RangedNumberRequest(typeof(int), baseMinimum, maximum);
            var request2 = new RangedNumberRequest(typeof(int), baseMinimum-1, maximum);
            var request3 = new RangedNumberRequest(typeof(int), baseMinimum-2, maximum);

            IDictionary<RangedNumberRequest, IList<int>> results = new Dictionary<RangedNumberRequest, IList<int>>()
            {
                { request1, new List<int>() }, { request2, new List<int>() }, { request3, new List<int>() }
            };

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            results[request1].Add((int)sut.Create(request1, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request1].Add((int)sut.Create(request1, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request1].Add((int)sut.Create(request1, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));
            results[request3].Add((int)sut.Create(request3, dummyContext));
            results[request2].Add((int)sut.Create(request2, dummyContext));

            // Verify
            Assert.True(Enumerable.Range(baseMinimum, maximum+1).All(a => results[request1].Contains(a)));
            Assert.True(Enumerable.Range(baseMinimum-1, maximum - (baseMinimum-1)+1).All(a => results[request2].Contains(a)));
            Assert.True(Enumerable.Range(baseMinimum-2, maximum - (baseMinimum - 2) + 1).All(a => results[request3].Contains(a)));
        }


        [Fact]
        public void CreateReturnsValuesFromCorrectSetForTwoRequestsOfSameTypeAndDifferentLimits()
        {
            // Fixture setup
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);
          
            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            var value1 = (int)sut.Create(request1, dummyContext);
            var value2 = (int)sut.Create(request2, dummyContext);
            var value3 = (int)sut.Create(request1, dummyContext);

            // Verify
            Assert.InRange(value1, 0, 2);
            Assert.InRange(value2, 10, 20);
            Assert.InRange(value3, 0, 2);           
        }

        [Fact]
        public void CreateReturnsValuesFromCorrectSetForMultipleRequestsWithInterspersedDifferentRequestOfSameType()
        {
            // Fixture setup
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);
            var request3 = new RangedNumberRequest(typeof(int), 0, 2);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            // Exercise system
            var value1 = (int)sut.Create(request1, dummyContext);
            var value2 = (int)sut.Create(request2, dummyContext);
            var value3 = (int)sut.Create(request3, dummyContext);

            // Verify
            Assert.InRange(value1, 0, 2);
            Assert.InRange(value2, 10, 20);
            Assert.InRange(value3, 0, 2);
            Assert.NotEqual(value1, value3);
        }

       
        [Fact]
        public void CreateReturnsValuesFromCorrectSetForRequestsWithDifferentTypesAndSameLimits()
        {
            // Fixture setup
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(long), 0, 2);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            var request1Results = new List<int>();
           
            // Exercise system
            request1Results.Add((int)sut.Create(request1, dummyContext));
            request1Results.Add((int)sut.Create(request1, dummyContext));
            var longResult = ((long)sut.Create(request2, dummyContext));
            request1Results.Add((int)sut.Create(request1, dummyContext));
            
            // Verify
            Assert.True(request1Results.Distinct().Count()==3);
            Assert.True(request1Results.TrueForAll(a => Enumerable.Range(0, 3).Contains(a)));
            Assert.InRange(longResult, 0, 2);

        }
    }
}
