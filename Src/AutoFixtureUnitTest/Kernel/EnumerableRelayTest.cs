using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class EnumerableRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new EnumerableRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new EnumerableRelay();
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
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        [InlineData(typeof(IList<object>))]
        [InlineData(typeof(IList<string>))]
        [InlineData(typeof(IList<int>))]
        [InlineData(typeof(IList<Version>))]
        public void CreateWithNoneEnumerableRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new EnumerableRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext { OnResolve = r => Enumerable.Empty<object>() };
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IEnumerable<object>), typeof(object))]
        [InlineData(typeof(IEnumerable<string>), typeof(string))]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [InlineData(typeof(IEnumerable<Version>), typeof(Version))]
        public void CreateWithEnumerableRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Fixture setup
            var expectedRequest = new MultipleRequest(itemType);
            object contextResult = Enumerable.Empty<object>();
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? contextResult : new NoSpecimen() };

            var sut = new EnumerableRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateConvertsEnumerableToCorrectGenericType()
        {
            // Fixture setup
            var request = typeof(IEnumerable<int>);
            var expectedRequest = new MultipleRequest(typeof(int));
            var enumerable = Enumerable.Range(1, 3).Cast<object>();
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? (object)enumerable : new NoSpecimen() };

            var sut = new EnumerableRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var e = Assert.IsAssignableFrom<IEnumerable<int>>(result);
            Assert.True(enumerable.Cast<int>().SequenceEqual(e));
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
            var request = typeof(IEnumerable<object>);
            var context = new DelegatingSpecimenContext { OnResolve = r => response };
            var sut = new EnumerableRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFiltersOmitSpecimenInstances()
        {
            // Fixture setup
            var request = typeof(IEnumerable<int>);
            var expectedRequest = new MultipleRequest(typeof(int));

            var enumerable = new object[]
            {
                1,
                new OmitSpecimen(),
                3
            };
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r) ? (object)enumerable : new NoSpecimen()
            };

            var sut = new EnumerableRelay();
            // Exercise system
            var actual = sut.Create(request, context);
            // Verify outcome
            var iter = Assert.IsAssignableFrom<IEnumerable<int>>(actual);
            Assert.True(
                enumerable.OfType<int>().SequenceEqual(iter),
                "Actual sequence is not equal to expected sequence.");
            // Teardown
        }
    }
}
