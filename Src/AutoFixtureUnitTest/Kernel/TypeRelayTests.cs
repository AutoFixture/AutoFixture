using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TypeRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyFrom = typeof(object);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(dummyFrom, dummyTo);
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(Version))]
        public void CreateFromNonMatchingRequestReturnsCorrectResult(
            object request)
        {
            // Fixture setup
            var from = typeof(OperatingSystem);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(from, dummyTo);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string), typeof(int))]
        [InlineData(typeof(int), typeof(string))]
        [InlineData(typeof(Version), typeof(Guid))]
        [InlineData(typeof(Guid), typeof(Version))]
        public void CreateFromMatchingRequestReturnsCorrectResult(
            Type from,
            Type to)
        {
            // Fixture setup
            var sut = new TypeRelay(from, to);
            var expected = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => to.Equals(r) ? expected : new object()
            };
            // Exercise system
            var actual = sut.Create(from, context);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullFromThrows()
        {
            // Fixture setup
            var dummyTo = typeof(object);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TypeRelay(null, dummyTo));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullToThrows()
        {
            // Fixture setup
            var dummyFrom = typeof(object);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TypeRelay(dummyFrom, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var dummyFrom = typeof(object);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(dummyFrom, dummyTo);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }
    }
}
