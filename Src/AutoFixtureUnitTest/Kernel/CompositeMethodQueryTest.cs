using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class CompositeMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new CompositeMethodQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void QueriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new CompositeMethodQuery();
            // Act
            IEnumerable<IMethodQuery> result = sut.Queries;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Arrange
            IEnumerable<IMethodQuery> nullEnumerable = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMethodQuery(nullEnumerable));
        }

        [Fact]
        public void QueriesWillMatchListParameter()
        {
            // Arrange
            var expectedQueries = new IMethodQuery[]
            {
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery()
            }.AsEnumerable();
            var sut = new CompositeMethodQuery(expectedQueries);
            // Act
            var result = sut.Queries;
            // Assert
            Assert.True(expectedQueries.SequenceEqual(result));
        }

        [Fact]
        public void CreateWithNullArrayWillThrow()
        {
            // Arrange
            IMethodQuery[] nullArray = null;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMethodQuery(nullArray));
        }

        [Fact]
        public void QueriesWillMatchParamsArray()
        {
            // Arrange
            var expectedQueries = new IMethodQuery[]
            {
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery()
            };
            var sut = new CompositeMethodQuery(expectedQueries);
            // Act
            var result = sut.Queries;
            // Assert
            Assert.True(expectedQueries.SequenceEqual(result));
        }

        [Fact]
        public void QueriesWillNotMatchParamsArray()
        {
            // Arrange
            var expectedQueries = new IMethodQuery[]
            {
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery()
            };
            var sut = new CompositeMethodQuery(expectedQueries[0], expectedQueries[2], expectedQueries[1]);
            // Act
            var result = sut.Queries;
            // Assert
            Assert.False(expectedQueries.SequenceEqual(result));
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectWillReturnResultsInCorrectSequence(Type type)
        {
            // Arrange
            IEnumerable<IMethod> modestConstructors = from ci in type.GetConstructors()
                                                      let parameters = ci.GetParameters()
                                                      orderby parameters.Length ascending
                                                      select new ConstructorMethod(ci) as IMethod;

            IEnumerable<IMethod> greedyConstructors = from ci in type.GetConstructors()
                                                      let parameters = ci.GetParameters()
                                                      orderby parameters.Length descending
                                                      select new ConstructorMethod(ci) as IMethod;

            var expectedConstructors = new List<IMethod>();
            expectedConstructors.AddRange(modestConstructors);
            expectedConstructors.AddRange(greedyConstructors);

            var queries = new IMethodQuery[]
            {
                new DelegatingMethodQuery { OnSelectMethods = t => modestConstructors },
                new DelegatingMethodQuery { OnSelectMethods = t => greedyConstructors },
                new DelegatingMethodQuery()
            };

            var sut = new CompositeMethodQuery(queries);
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.SequenceEqual(result));
        }
    }
}
