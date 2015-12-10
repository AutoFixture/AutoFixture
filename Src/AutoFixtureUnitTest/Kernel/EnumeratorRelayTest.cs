using System;
using System.Collections;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class EnumeratorRelayTest
    {
        [Fact]
        public void SutIsISpecimenBuilder()
        {
            var sut = new EnumeratorRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            var sut = new EnumeratorRelay();
            var dummyRequest = new object();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void CreateWithNoEnumeratorRequestReturnsCorrectResult(
            object request)
        {
            var sut = new EnumeratorRelay();
            var dummyContext = new DelegatingSpecimenContext();

            var result = sut.Create(request, dummyContext);
            
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [InlineData(typeof (IEnumerator<object>), typeof (object))]
        [InlineData(typeof (IEnumerator<string>), typeof (string))]
        [InlineData(typeof (IEnumerator<int>), typeof (int))]
        [InlineData(typeof (IEnumerator<Version>), typeof (Version))]
        public void CreateWithEnumeratorRequestReturnsCorrectResult(
            Type request,
            Type itemType)
        {
            var expectedRequest =
                typeof (IEnumerable<>).MakeGenericType(itemType);
            var enumerable = (IList) Activator.CreateInstance(
                typeof (List<>).MakeGenericType(itemType));
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRequest.Equals(r)
                    ? (object)enumerable
#pragma warning disable 618
                    : new NoSpecimen(r)
#pragma warning restore 618
            };
            var sut = new EnumeratorRelay();

            var result = sut.Create(request, context);

            var expectedResult = enumerable.GetEnumerator();
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(object[]))]
        public void CreateReturnsCorrectResultWhenContextReturnsNonEnumerableResult(
            object response)
        {
            var request = typeof(IEnumerator<object>);
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => response
            };
            var sut = new EnumeratorRelay();

            var result = sut.Create(request, context);
            
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}
