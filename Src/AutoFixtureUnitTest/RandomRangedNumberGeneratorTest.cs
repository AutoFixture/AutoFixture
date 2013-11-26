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
            var sut = new RandomRangedNumberGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            var request = new RangedNumberRequest(operandType, minimum, maximum);

            var result = sut.Create(request, dummyContext);

            Assert.Equal(new NoSpecimen(request), result);
        }


        [Theory]
        [InlineData(typeof(int), 1, 2)]
        [InlineData(typeof(int), -5, -4)]
        [InlineData(typeof(long), 10000, 10005)]
        public void CreateWithMultipleValuesInSetCorrectlyReturnsDifferentValuesForDifferentRequests(Type operandType, object minimum, object maximum)
        {
            var sut = new RandomRangedNumberGenerator();
           
            var request1 = new RangedNumberRequest(operandType, minimum, maximum);
            var request2 = new RangedNumberRequest(operandType, minimum, maximum);

            var dummyContext = new DelegatingSpecimenContext();

            var value1 = sut.Create(request1, dummyContext);
            var value2 = sut.Create(request2, dummyContext);
            Assert.NotEqual(value1, value2);
        }

        [Fact]       
        public void CreateReturnsRandomDifferentValuesForRequestsOfSameTypeAndLimitsWithInterspersedRequest()
        {          
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);
            var request3 = new RangedNumberRequest(typeof(int), 0, 2);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            var value1 = (int)sut.Create(request1, dummyContext);
            var value2 = (int)sut.Create(request2, dummyContext);
            var value3 = (int)sut.Create(request3, dummyContext);
            Assert.NotEqual(value1, value3);  
        }

        [Fact]
        public void CreateReturnsRandomDifferentValuesForRequestsOfSameTypeAndOverlappingLimits()
        {
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

            Assert.True(Enumerable.Range(0, baseMaximum).All(a => results[request1].Contains(a)));
            Assert.True(Enumerable.Range(0, baseMaximum+1).All(a => results[request2].Contains(a)));
            Assert.True(Enumerable.Range(0, baseMaximum+2).All(a => results[request3].Contains(a)));
        }

        [Fact]
        public void CreateReturnsValuesFromDifferentSetsForSameTypeAndDifferentLimits()
        {
            var request1 = new RangedNumberRequest(typeof(int), 0, 2);
            var request2 = new RangedNumberRequest(typeof(int), 10, 20);
            var request3 = new RangedNumberRequest(typeof(int), 0, 2);

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            var value1 = (int)sut.Create(request1, dummyContext);
            var value2 = (int)sut.Create(request2, dummyContext);
            var value3 = (int)sut.Create(request3, dummyContext);

            Assert.InRange(value1, 0, 2);
            Assert.InRange(value2, 10, 20);
        }

        [Fact]
        public void CreateReturnsAllValuesInRangeBeforeRepeating()
        {
            int minimum = 0;
            int maximum = 2;

            var request = new RangedNumberRequest(typeof(int), minimum, maximum);                     

            var dummyContext = new DelegatingSpecimenContext();

            var sut = new RandomRangedNumberGenerator();

            var generatedValues = Enumerable.Range(0, 3).Select(i => sut.Create(request, dummyContext)).Cast<int>();
            var finalValue = (int)sut.Create(request, dummyContext);

            Assert.True(generatedValues.All(a => a >= minimum && a <= maximum));
            Assert.InRange(finalValue, minimum, maximum);

        }
     
    }
}
