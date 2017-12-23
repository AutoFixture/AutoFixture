using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public abstract class LazyRelayTest<T>
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new LazyRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new LazyRelay();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
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
            // Arrange
            var sut = new LazyRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        public void CreateWithNonGenericTypeRequestReturnsNoSpecimen(
            Type request)
        {
            // Arrange
            var sut = new LazyRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<Version>))]
        public void CreateWithNonLazyRequestReturnsNoSpecimen(
            Type request)
        {
            // Arrange
            var sut = new LazyRelay();
            var dummyContext = new DelegatingSpecimenContext();
            // Act
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateWithLazyRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new LazyRelay();
            var @delegate = new Func<T>(() => default(T));
            var contextStub =
                new DelegatingSpecimenContext { OnResolve = t => @delegate };
            // Act
            var result = sut.Create(typeof(Lazy<T>), contextStub);
            var actual = Assert.IsAssignableFrom<Lazy<T>>(result);
            // Assert
            var expected = new Lazy<T>(@delegate);
            Assert.Equal(expected.Value, actual.Value);
        }
    }

    public class LazyRelayTestOfInt32
        : LazyRelayTest<int>
    { }

    public class LazyRelayTestOfString
        : LazyRelayTest<string>
    { }

    public class LazyRelayTestOfVersion
        : LazyRelayTest<Version>
    { }

    public class LazyRelayTestOfSingleParameterType
        : LazyRelayTest<SingleParameterType<int>>
    { }

    public class LazyRelayTestOfDoubleParameterType
        : LazyRelayTest<DoubleParameterType<int, string>>
    { }

    public class LazyRelayTestOfTripleParameterType
        : LazyRelayTest<TripleParameterType<int, string, Version>>
    { }
}
