using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class SortedListRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new SortedListRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new SortedListRelay();
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
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(IEnumerable<Version>))]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<Version>))]
        [InlineData(typeof(IDictionary<int,string>))]
        public void CreateWithNonSortedListRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new SortedListRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
#pragma warning disable 618
            var expectedResult = new NoSpecimen(request);
#pragma warning restore 618
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SortedList<object, object>), typeof(object), typeof(object))]
        [InlineData(typeof(SortedList<int, string>), typeof(int), typeof(string))]
        [InlineData(typeof(SortedList<string, int>), typeof(string), typeof(int))]
        [InlineData(typeof(SortedList<Version, OperatingSystem>), typeof(Version), typeof(OperatingSystem))]
        public void CreateWithSortedListRequestReturnsCorrectResult(Type request, Type keyType, Type itemType)
        {
            // Fixture setup
            var expectedResult = typeof(SortedList<,>).MakeGenericType(keyType, itemType);
            object contextResult = typeof(Dictionary<,>).MakeGenericType(keyType, itemType).GetConstructor(Type.EmptyTypes)?.Invoke(new object[0]);
            var context = new DelegatingSpecimenContext { OnResolve = r => contextResult };
            var sut = new SortedListRelay();

            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result.GetType());
            // Teardown
        }
    }
}
