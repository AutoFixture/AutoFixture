using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class EnumerableFavoringConstructorQueryTest
    {
        [Fact]
        public void SutIsConstructorQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new EnumerableFavoringConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IConstructorQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new EnumerableFavoringConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectConstructorsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new EnumerableFavoringConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectConstructors(null));
            // Teardown
        }

        [Fact]
        public void SelectFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new EnumerableFavoringConstructorQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Exercise system
            var result = sut.SelectConstructors(typeWithNoPublicConstructors);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectFromTypeReturnsAllAppropriateResults(Type type)
        {
            // Fixture setup
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new EnumerableFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(type);
            // Verify outcome
            Assert.True(expectedConstructors.All(m => result.Any(m.Equals)));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<Version>))]
        [InlineData(typeof(HashSet<object>))]
        [InlineData(typeof(HashSet<string>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(HashSet<Version>))]
        public void SelectConstructorsFromTypeReturnsFirstMethodThatTakesEnumerableAsArgument(Type type)
        {            
            var sut = new EnumerableFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(type);
            // Verify outcome
            var genericParameterType = type.GetGenericArguments().Single();
            Assert.True(result.First().Parameters.Any(p => typeof(IEnumerable<>).MakeGenericType(genericParameterType).IsAssignableFrom(p.ParameterType)));
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new EnumerableFavoringConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new EnumerableFavoringConstructorQuery();
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

            var sut = new EnumerableFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(expectedConstructors.All(m => result.Any(m.Equals)));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(List<object>))]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<Version>))]
        [InlineData(typeof(HashSet<object>))]
        [InlineData(typeof(HashSet<string>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(HashSet<Version>))]
        public void SelectMethodsFromTypeReturnsFirstMethodThatTakesEnumerableAsArgument(Type type)
        {
            var sut = new EnumerableFavoringConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            var genericParameterType = type.GetGenericArguments().Single();
            Assert.True(result.First().Parameters.Any(p => typeof(IEnumerable<>).MakeGenericType(genericParameterType).IsAssignableFrom(p.ParameterType)));
            // Teardown
        }
    }
}
