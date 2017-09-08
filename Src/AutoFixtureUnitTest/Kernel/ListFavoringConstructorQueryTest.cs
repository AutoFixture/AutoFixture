using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ListFavoringConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new ListFavoringConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new ListFavoringConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ListFavoringConstructorQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Exercise system
            var result = sut.SelectMethods(typeWithNoPublicConstructors);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectMethodsFromTypeReturnsAllAppropriateResults(Type type)
        {
            // Fixture setup
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new ListFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(expectedConstructors.All(m => result.Any(m.Equals)));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Collection<object>))]
        [InlineData(typeof(Collection<string>))]
        [InlineData(typeof(Collection<int>))]
        [InlineData(typeof(Collection<Version>))]
        public void SelectMethodsFromTypeReturnsFirstMethodThatTakesListAsArgument(Type type)
        {
            // Fixture setup
            var sut = new ListFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            var genericParameterType = type.GetGenericArguments().Single();
            Assert.True(result.First().Parameters.Any(p => typeof(IList<>).MakeGenericType(genericParameterType).IsAssignableFrom(p.ParameterType)));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        [InlineData(typeof(ItemHolder<object>))]
        public void SelectMethodsFromTypeReturnsCorrectlyOrderedResultWhenNoConstructorContainsEnumerableArguments(Type type)
        {
            // Fixture setup
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       orderby parameters.Length ascending
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new ListFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(expectedConstructors.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ItemHolder<IList<object>, Collection<object>>))]
        [InlineData(typeof(ItemHolder<Collection<object>, IList<object>>))]
        public void SelectMethodsPrefersSpecificListParameterOverDerivedParameter(Type type)
        {
            // Fixture setup
            var sut = new ListFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(result.First().Parameters.Any(p => typeof(IList<object>) == p.ParameterType));
            // Teardown
        }
    }
}
