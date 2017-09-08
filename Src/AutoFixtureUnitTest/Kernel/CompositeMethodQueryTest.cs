using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class CompositeMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeMethodQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void QueriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeMethodQuery();
            // Exercise system
            IEnumerable<IMethodQuery> result = sut.Queries;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Fixture setup
            IEnumerable<IMethodQuery> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMethodQuery(nullEnumerable));
            // Teardown
        }

        [Fact]
        public void QueriesWillMatchListParameter()
        {
            // Fixture setup
            var expectedQueries = new IMethodQuery[]
            {
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery()
            }.AsEnumerable();
            var sut = new CompositeMethodQuery(expectedQueries);
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
            IMethodQuery[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMethodQuery(nullArray));
            // Teardown
        }

        [Fact]
        public void QueriesWillMatchParamsArray()
        {
            // Fixture setup
            var expectedQueries = new IMethodQuery[]
            {
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery()
            };
            var sut = new CompositeMethodQuery(expectedQueries);
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
            var expectedQueries = new IMethodQuery[]
            {
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery(),
                new DelegatingMethodQuery()
            };
            var sut = new CompositeMethodQuery(expectedQueries[0], expectedQueries[2], expectedQueries[1]);
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

            var queries = new IMethodQuery[]
            {
                new DelegatingMethodQuery { OnSelectMethods = t => modestConstructors },
                new DelegatingMethodQuery { OnSelectMethods = t => greedyConstructors },
                new DelegatingMethodQuery()
            };

            var sut = new CompositeMethodQuery(queries);
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(expectedConstructors.SequenceEqual(result));
            // Teardown
        }
    }
}
