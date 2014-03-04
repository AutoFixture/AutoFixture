﻿using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ReadOnlyCollectionRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ReadOnlyCollectionRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new ReadOnlyCollectionRelay();
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
        public void CreateWithNonListRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new ReadOnlyCollectionRelay();
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IReadOnlyCollection<object>), typeof(object))]
        [InlineData(typeof(IReadOnlyCollection<string>), typeof(string))]
        [InlineData(typeof(IReadOnlyCollection<int>), typeof(int))]
        [InlineData(typeof(IReadOnlyCollection<Version>), typeof(Version))]
        public void CreateWithListRequestReturnsCorrectResult(Type request, Type itemType)
        {
            // Fixture setup
            var expectedRequest = typeof(List<>).MakeGenericType(itemType);
            object contextResult = typeof(List<>).MakeGenericType(itemType).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? contextResult : new NoSpecimen(r) };

            var sut = new ReadOnlyCollectionRelay();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(contextResult, result);
            // Teardown
        }
    }
}
