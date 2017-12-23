using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TypeRelayTests
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyFrom = typeof(object);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(dummyFrom, dummyTo);
            // Act
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void ConstructorArgumentsShouldBeExposedByProperties()
        {
            // Arrange
            var from = typeof(object);
            var to = typeof(string);

            // Act
            var sut = new TypeRelay(from, to);

            // Assert
            Assert.Equal(from, sut.From);
            Assert.Equal(to, sut.To);
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
            // Arrange
            var from = typeof(ConcreteType);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(from, dummyTo);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, actual);
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
            // Arrange
            var sut = new TypeRelay(from, to);
            var expected = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => to.Equals(r) ? expected : new object()
            };
            // Act
            var actual = sut.Create(from, context);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullFromThrows()
        {
            // Arrange
            var dummyTo = typeof(object);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new TypeRelay(null, dummyTo));
        }

        [Fact]
        public void ConstructWithNullToThrows()
        {
            // Arrange
            var dummyFrom = typeof(object);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new TypeRelay(dummyFrom, null));
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var dummyFrom = typeof(object);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(dummyFrom, dummyTo);
            // Act & assert
            var dummyRequest = new object();
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
        }

        [Fact]
        public void DocumentationExample()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(BaseType),
                    typeof(DerivedType)));

            var actual = fixture.Create<BaseType>();

            Assert.IsAssignableFrom<DerivedType>(actual);
        }


        [Fact]
        public void ShouldNotFailIfOpenGenericsIsPassedToConstructor()
        {
            // Arrange
            var openFrom = typeof(IEnumerable<>);
            var openTo = typeof(List<>);

            // Act & assert
            Assert.Null(Record.Exception(() =>
                new TypeRelay(openFrom, openTo)));
        }

        [Theory]
        [InlineData(typeof(IEnumerable<>), typeof(string))]
        [InlineData(typeof(string), typeof(IEnumerable<>))]
        public void ShouldFailIfConstructedWithOpenAndNonOpenType(Type from, Type to)
        {
            // Act & assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new TypeRelay(from, to));
            Assert.Contains("open generic", ex.Message);
        }

        [Theory]
        [InlineData(typeof(IEnumerable<>), typeof(List<>), typeof(IEnumerable<string>), typeof(List<string>))]
        [InlineData(typeof(IReadOnlyDictionary<,>), typeof(IDictionary<,>), typeof(IReadOnlyDictionary<string, byte>), typeof(IDictionary<string, byte>))]
        [InlineData(typeof(Nullable<>), typeof(IEnumerable<>), typeof(int?), typeof(IEnumerable<int>))]
        public void ShouldRelayOpenGenericsCorrectly(Type from, Type to, Type request, Type expectedRelay)
        {
            // Arrange
            var sut = new TypeRelay(from, to);

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRelay.Equals(r) ? expectedResult : new NoSpecimen()
            };

            // Act
            var result = sut.Create(request, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IgnoresRequestIfGenericTypeDoesNotMatchExactly()
        {
            // Arrange
            var from = typeof(IEnumerable<>);
            var to = typeof(List<>);
            var sut = new TypeRelay(from, to);

            var request = typeof(int[]);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = _ => new object()
            };

            // Act
            var result = sut.Create(request, dummyContext);

            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void FailsAtResolveIfImproperMappingIsSpecified()
        {
            // Arrange
            var from = typeof(IEnumerable<>);
            var to = typeof(Nullable<>);
            var sut = new TypeRelay(from, to);

            var request = typeof(IEnumerable<string>);
            var dummyContext = new DelegatingSpecimenContext();

            // Act & assert
            Assert.Throws<ArgumentException>(() =>
                sut.Create(request, dummyContext));
        }

        private abstract class BaseType { }

        private class DerivedType : BaseType { }
    }
}
