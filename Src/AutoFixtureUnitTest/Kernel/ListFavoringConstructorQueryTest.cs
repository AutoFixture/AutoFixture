using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ListFavoringConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new ListFavoringConstructorQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Arrange
            var sut = new ListFavoringConstructorQuery();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Arrange
            var sut = new ListFavoringConstructorQuery();
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

            var sut = new ListFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.All(m => result.Any(m.Equals)));
        }

        [Theory]
        [InlineData(typeof(Collection<object>))]
        [InlineData(typeof(Collection<string>))]
        [InlineData(typeof(Collection<int>))]
        [InlineData(typeof(Collection<Version>))]
        public void SelectMethodsFromTypeReturnsFirstMethodThatTakesListAsArgument(Type type)
        {
            // Arrange
            var sut = new ListFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            var genericParameterType = type.GetGenericArguments().Single();
            Assert.Contains(result.First().Parameters, p => typeof(IList<>).MakeGenericType(genericParameterType).IsAssignableFrom(p.ParameterType));
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

            var sut = new ListFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.SequenceEqual(result));
        }

        [Theory]
        [InlineData(typeof(ItemHolder<IList<object>, Collection<object>>))]
        [InlineData(typeof(ItemHolder<Collection<object>, IList<object>>))]
        public void SelectMethodsPrefersSpecificListParameterOverDerivedParameter(Type type)
        {
            // Arrange
            var sut = new ListFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.Contains(result.First().Parameters, p => typeof(IList<object>) == p.ParameterType);
        }

        [Fact]
        public void DoesNotReturnConstructorsWithParametersOfEnclosingType()
        {
            // Arrange
            var sut = new ListFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(typeof(TypeWithCopyConstructorsOnly));
            // Assert
            Assert.Empty(result);
        }

        public class TypeWithCopyConstructorsOnly
        {
            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other)
            {
            }

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, List<int> nums)
            {
            }
        }
    }
}
