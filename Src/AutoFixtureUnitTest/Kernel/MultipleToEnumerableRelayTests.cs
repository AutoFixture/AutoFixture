using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultipleToEnumerableRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new MultipleToEnumerableRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(object))]
        [InlineData(typeof(Type))]
        [InlineData(1)]
        [InlineData(9992)]
        [InlineData("")]
        public void CreateFromNonMultipleRequestReturnsCorrectResult(
            object request)
        {
            // Fixture setup
            var sut = new MultipleToEnumerableRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object), 1)]
        [InlineData(typeof(string), 42)]
        [InlineData(typeof(int), 1337)]
        public void CreateFromMultipleRequestReturnsCorrectResult(
            Type itemType,
            int arrayLength)
        {
            // Fixture setup
            var sut = new MultipleToEnumerableRelay();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(
                        typeof(IEnumerable<>).MakeGenericType(itemType),
                        r);
                    return Array.CreateInstance((Type)itemType, arrayLength);
                }
            };
            // Exercise system
            var request = new MultipleRequest(itemType);
            var actual = sut.Create(request, context);
            // Verify outcome
            Assert.IsAssignableFrom(
                typeof(IEnumerable<>).MakeGenericType(itemType),
                actual);
            var enumerable = 
                Assert.IsAssignableFrom<System.Collections.IEnumerable>(actual);
            Assert.Equal(arrayLength, enumerable.Cast<object>().Count());
            // Teardown
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("1")]
        [InlineData(false)]
        public void CreateFromMultipleRequestWithNonTypeRequestReturnsCorrectResult(
            object innerRequest)
        {
            // Fixture setup
            var sut = new MultipleToEnumerableRelay();
            var request = new MultipleRequest(innerRequest);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}
