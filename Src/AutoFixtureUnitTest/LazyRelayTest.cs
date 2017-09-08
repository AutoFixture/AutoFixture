using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public abstract class LazyRelayTest<T>
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new LazyRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new LazyRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(123)]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        public void CreateWithNonTypeRequestReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var sut = new LazyRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        public void CreateWithNonGenericTypeRequestReturnsNoSpecimen(
            Type request)
        {
            // Fixture setup
            var sut = new LazyRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<Version>))]
        public void CreateWithNonLazyRequestReturnsNoSpecimen(
            Type request)
        {
            // Fixture setup
            var sut = new LazyRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWithLazyRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new LazyRelay();
            var @delegate = new Func<T>(() => default(T));
            var contextStub =
                new DelegatingSpecimenContext { OnResolve = t => @delegate };
            // Exercise system
            var result = sut.Create(typeof(Lazy<T>), contextStub);
            var actual = Assert.IsAssignableFrom<Lazy<T>>(result);
            // Verify outcome
            var expected = new Lazy<T>(@delegate);
            Assert.Equal(expected.Value, actual.Value);
            // Teardown
        }
    }

    public class LazyRelayTestOfInt32
        : LazyRelayTest<int> { }

    public class LazyRelayTestOfString
        : LazyRelayTest<string> { }

    public class LazyRelayTestOfVersion
        : LazyRelayTest<Version> { }

    public class LazyRelayTestOfSingleParameterType
        : LazyRelayTest<SingleParameterType<int>> { }

    public class LazyRelayTestOfDoubleParameterType
        : LazyRelayTest<DoubleParameterType<int, string>> { }

    public class LazyRelayTestOfTripleParameterType
        : LazyRelayTest<TripleParameterType<int, string, Version>> { }
}
