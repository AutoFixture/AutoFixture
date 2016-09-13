using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class QueryableRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new QueryableRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            var sut = new QueryableRelay();
            var dummyRequest = new object();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithNonQueryableRequestReturnsCorrectResult()
        {
            var request = new object();
            var dummyContext = new DelegatingSpecimenContext();
            var sut = new QueryableRelay();

            var actual = sut.Create(request, dummyContext);

            Assert.IsType<NoSpecimen>(actual);
        }

        [Fact]
        public void CreateWithIQueryableRequestReturnsNoSpecimenIfNoIEnumerableResolvedFromContext()
        {
            var request = typeof(IQueryable<object>);
            var context = new DelegatingSpecimenContext();
            var sut = new QueryableRelay();

            var result = sut.Create(request, context);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithIQueryableRequestReturnsCorrectResult1()
        {
            var request = typeof(IQueryable<object>);
            var expectedRequest = typeof(IEnumerable<>).MakeGenericType(typeof(object));
            object contextResult = new object[0].AsQueryable();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r)
                    ? contextResult
                    : new NoSpecimen()
            };
            var sut = new QueryableRelay();

            var result = sut.Create(request, context);

            Assert.Equal(contextResult, result);
        }
    }
}
