using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class EnumerableFavoringConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new EnumerableFavoringConstructorQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Arrange
            var sut = new EnumerableFavoringConstructorQuery();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Arrange
            var sut = new EnumerableFavoringConstructorQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Act
            var result = sut.SelectMethods(typeWithNoPublicConstructors);
            // Assert
            Assert.False(result.Any());
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectMethodsFromTypeReturnsAllAppropriateResults(Type type)
        {
            // Arrange
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new EnumerableFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.All(m => result.Any(m.Equals)));
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
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            var genericParameterType = type.GetGenericArguments().Single();
            Assert.Contains(result.First().Parameters, p => typeof(IEnumerable<>).MakeGenericType(genericParameterType).IsAssignableFrom(p.ParameterType));
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        [InlineData(typeof(ItemHolder<object>))]
        public void SelectMethodsFromTypeReturnsCorrectlyOrderedResultWhenNoConstructorContainsEnumerableArguments(Type type)
        {
            // Arrange
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       orderby parameters.Length ascending
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new EnumerableFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.SequenceEqual(result));
        }

        [Theory]
        [InlineData(typeof(ItemHolder<IEnumerable<object>, object[]>), typeof(IEnumerable<object>))]
        [InlineData(typeof(ItemHolder<object[], IEnumerable<object>>), typeof(IEnumerable<object>))]
        [InlineData(typeof(ItemContainer<SingleParameterType<object>>), typeof(IEnumerable<SingleParameterType<object>>))]
        public void SelectMethodsPrefersSpecificEnumerableParameterOverDerivedParameter(Type type, Type expected)
        {
            // Arrange
            var sut = new EnumerableFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.Contains(result.First().Parameters, p => expected == p.ParameterType);
        }

        [Fact]
        public void DoesNotReturnConstructorsWithParametersOfEnclosingType()
        {
            // Arrange
            var sut = new EnumerableFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(typeof(TypeWithCopyConstructorsOnly));

            // Assert
            Assert.Empty(result);
        }

        private class TypeWithCopyConstructorsOnly
        {
            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other)
            {
            }

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, IEnumerable<int> nums)
            {
            }
        }
    }
}
