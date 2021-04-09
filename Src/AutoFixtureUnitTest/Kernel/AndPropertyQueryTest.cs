using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class AndPropertyQueryTest
    {
        [Fact]
        public void SutIsPropertyQuery()
        {
            // Arrange
            // Act
            var sut = new AndPropertyQuery();

            // Assert
            Assert.IsAssignableFrom<IPropertyQuery>(sut);
        }

        [Fact]
        public void QueriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new AndPropertyQuery();

            // Act
            var result = sut.Queries;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void QueriesWillMatchProvidedParameters()
        {
            // Arrange
            var parameters = new IPropertyQuery[]
            {
                new ReadonlyPropertyQuery(),
                new GenericCollectionPropertyQuery()
            };
            var sut = new AndPropertyQuery(parameters);

            // Act
            var result = sut.Queries;

            // Assert
            Assert.Equal(parameters, result);
        }

        [Fact]
        public void SelectPropertiesWillSelectPropertiesMeetingProvidedQueryParameters()
        {
            // Arrange
            var sut = new AndPropertyQuery(
                new ReadonlyPropertyQuery(),
                new GenericCollectionPropertyQuery());

            // Act
            var result = sut.SelectProperties(typeof(CollectionHolder<string>));

            // Assert
            Assert.Equal(result, typeof(CollectionHolder<string>).GetTypeInfo().GetProperties());
        }

        [Theory]
        [InlineData(typeof(StaticPropertyHolder<List<string>>))]
        [InlineData(typeof(SingleParameterType<string>))]
        public void SelectPropertiesWillNotSelectPropertiesNotMeetingProvidedQueryParameters(Type type)
        {
            // Arrange
            var sut = new AndPropertyQuery(
                new ReadonlyPropertyQuery(),
                new GenericCollectionPropertyQuery());

            // Act
            var result = sut.SelectProperties(type);

            // Assert
            Assert.NotEqual(result, type.GetTypeInfo().GetProperties());
        }
    }
}