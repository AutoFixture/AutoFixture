using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ArrayRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ArrayRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new ArrayRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void CreateWithNoneArrayRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new ArrayRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object[]), typeof(object))]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(Version[]), typeof(Version))]
        public void CreateWithArrayRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Fixture setup
            var expectedRequest = new MultipleRequest(itemType);
            object expectedResult = Array.CreateInstance(itemType, 0);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? expectedResult : new NoSpecimen() };

            var sut = new ArrayRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateConvertsEnumerableToArray()
        {
            // Fixture setup
            var request = typeof(int[]);
            var expectedRequest = new MultipleRequest(typeof(int));
            var enumerable = Enumerable.Range(1, 3);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)enumerable : new NoSpecimen() };

            var sut = new ArrayRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var a = Assert.IsAssignableFrom<int[]>(result);
            Assert.True(enumerable.SequenceEqual(a));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(object[]))]
        public void CreateReturnsCorrectResultWhenContextReturnsNonEnumerableResult(object response)
        {
            // Fixture setup
            var request = typeof(object[]);
            var context = new DelegatingSpecimenContext { OnResolve = r => response };
            var sut = new ArrayRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
