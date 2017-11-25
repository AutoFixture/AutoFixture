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
            // Fixture setup
            var dummyFrom = typeof(object);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(dummyFrom, dummyTo);
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorArgumentsShouldBeExposedByProperties()
        {
            // Fixture setup
            var from = typeof(object);
            var to = typeof(string);

            // Exercise system
            var sut = new TypeRelay(from, to);

            // Verify outcome
            Assert.Equal(from, sut.From);
            Assert.Equal(to, sut.To);
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
            var from = typeof(ConcreteType);
            var dummyTo = typeof(object);
            var sut = new TypeRelay(from, dummyTo);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen();
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
            // Fixture setup
            var openFrom = typeof(IEnumerable<>);
            var openTo = typeof(List<>);
            
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => 
                new TypeRelay(openFrom, openTo)));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IEnumerable<>), typeof(string))]
        [InlineData(typeof(string), typeof(IEnumerable<>))]
        public void ShouldFailIfConstructedWithOpenAndNonOpenType(Type from, Type to)
        {
            // Exercise system and verify outcome
            var ex = Assert.Throws<ArgumentException>(() =>
                new TypeRelay(from, to));
            Assert.Contains("open generic", ex.Message);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IEnumerable<>), typeof(List<>), typeof(IEnumerable<string>), typeof(List<string>))]
        [InlineData(typeof(IReadOnlyDictionary<,>), typeof(IDictionary<,>), typeof(IReadOnlyDictionary<string, byte>), typeof(IDictionary<string, byte>))]
        [InlineData(typeof(Nullable<>), typeof(IEnumerable<>), typeof(int?), typeof(IEnumerable<int>))]
        [InlineData(typeof(IList<>), typeof(List<>), typeof(IList<string>), typeof(List<string>))]
        [InlineData(typeof(IReadOnlyCollection<>), typeof(List<>), typeof(IReadOnlyCollection<string>), typeof(List<string>))]
        [InlineData(typeof(IReadOnlyList<>), typeof(List<>), typeof(IReadOnlyList<string>), typeof(List<string>))]
        public void ShouldRelayOpenGenericsCorrectly(Type from, Type to, Type request, Type expectedRelay)
        {
            // Fixture setup
            var sut = new TypeRelay(from, to);

            var expectedResult = new object();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => expectedRelay.Equals(r) ? expectedResult : new NoSpecimen()
            };

            // Exercise system
            var result = sut.Create(request, context);

            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void IgnoresRequestIfGenericTypeDoesNotMatchExactly()
        {
            // Fixture setup
            var from = typeof(IEnumerable<>);
            var to = typeof(List<>);
            var sut = new TypeRelay(from, to);

            var request = typeof(int[]);
            var dummyContext = new DelegatingSpecimenContext
            {
                OnResolve = _ => new object()
            };

            // Exercise system
            var result = sut.Create(request, dummyContext);

            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void FailsAtResolveIfImproperMappingIsSpecified()
        {
            // Fixture setup
            var from = typeof(IEnumerable<>);
            var to = typeof(Nullable<>);
            var sut = new TypeRelay(from, to);

            var request = typeof(IEnumerable<string>);
            var dummyContext = new DelegatingSpecimenContext();

            // Exercise system and Verify outcome
            Assert.Throws<ArgumentException>(() =>
                sut.Create(request, dummyContext));
            // Teardown
        }
        
        private abstract class BaseType { }

        private class DerivedType : BaseType { }
    }
}
