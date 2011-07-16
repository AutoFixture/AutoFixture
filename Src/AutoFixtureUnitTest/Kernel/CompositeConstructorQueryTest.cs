using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class CompositeConstructorQueryTest
    {
        [Fact]
        public void SutIsConstructorQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IConstructorQuery>(sut);
            // Teardown
        }

        [Fact]
        public void QueriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeConstructorQuery();
            // Exercise system
            IEnumerable<IConstructorQuery> result = sut.Queries;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Fixture setup
            IEnumerable<IConstructorQuery> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeConstructorQuery(nullEnumerable));
            // Teardown
        }

        [Fact]
        public void QueriesWillMatchListParameter()
        {
            // Fixture setup
            var expectedQueries = new IConstructorQuery[]
            {
                new DelegatingConstructorQuery(),
                new DelegatingConstructorQuery(),
                new DelegatingConstructorQuery()
            }.AsEnumerable();
            var sut = new CompositeConstructorQuery(expectedQueries);
            // Exercise system
            var result = sut.Queries;
            // Verify outcome
            Assert.True(expectedQueries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateWithNullArrayWillThrow()
        {
            // Fixture setup
            IConstructorQuery[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeConstructorQuery(nullArray));
            // Teardown
        }

        [Fact]
        public void QueriesWillMatchParamsArray()
        {
            // Fixture setup
            var expectedQueries = new IConstructorQuery[]
            {
                new DelegatingConstructorQuery(),
                new DelegatingConstructorQuery(),
                new DelegatingConstructorQuery()
            };
            var sut = new CompositeConstructorQuery(expectedQueries);
            // Exercise system
            var result = sut.Queries;
            // Verify outcome
            Assert.True(expectedQueries.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void QueriesWillNotMatchParamsArray()
        {
            // Fixture setup
            var expectedQueries = new IConstructorQuery[]
            {
                new DelegatingConstructorQuery(),
                new DelegatingConstructorQuery(),
                new DelegatingConstructorQuery()
            };
            var sut = new CompositeConstructorQuery(expectedQueries[0], expectedQueries[2], expectedQueries[1]);
            // Exercise system
            var result = sut.Queries;
            // Verify outcome
            Assert.False(expectedQueries.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectWillReturnResultsInCorrectSequence(Type type)
        {
            // Fixture setup
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

            var queries = new IConstructorQuery[]
            {
                new DelegatingConstructorQuery { OnSelectConstructors = t => modestConstructors },
                new DelegatingConstructorQuery { OnSelectConstructors = t => greedyConstructors },
                new DelegatingConstructorQuery()
            };

            var sut = new CompositeConstructorQuery(queries);
            // Exercise system
            var result = sut.SelectConstructors(type);
            // Verify outcome
            Assert.True(expectedConstructors.SequenceEqual(result));
            // Teardown
        }
    }
}
