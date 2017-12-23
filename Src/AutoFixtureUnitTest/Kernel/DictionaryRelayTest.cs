using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class DictionaryRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new DictionaryRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new DictionaryRelay();
            var dummyRequest = new object();
            // Act & assert
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
        public void CreateWithNonDictionaryRequestReturnsCorrectResult(object request)
        {
            // Arrange
            var sut = new DictionaryRelay();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(IDictionary<object, object>), typeof(object), typeof(object))]
        [InlineData(typeof(IDictionary<int, string>), typeof(int), typeof(string))]
        [InlineData(typeof(IDictionary<string, int>), typeof(string), typeof(int))]
        [InlineData(typeof(IDictionary<Version, ConcreteType>), typeof(Version), typeof(ConcreteType))]
        public void CreateWithListRequestReturnsCorrectResult(Type request, Type keyType, Type itemType)
        {
            // Arrange
            var expectedRequest = typeof(Dictionary<,>).MakeGenericType(keyType, itemType);
            object contextResult = typeof(Dictionary<,>).MakeGenericType(keyType, itemType).GetTypeInfo().GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedRequest.Equals(r) ? contextResult : new NoSpecimen() };

            var sut = new DictionaryRelay();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Equal(contextResult, result);
        }
    }
}
