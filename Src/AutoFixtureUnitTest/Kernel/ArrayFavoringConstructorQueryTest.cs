using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ArrayFavoringConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new ArrayFavoringConstructorQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Arrange
            var sut = new ArrayFavoringConstructorQuery();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null).ToList());
        }

        [Fact]
        public void SelectFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArrayFavoringConstructorQuery();
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

            var sut = new ArrayFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.All(m => result.Any(m.Equals)));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(BitArray))]
        public void SelectMethodsFromTypeReturnsFirstMethodThatTakesArrayAsArgument(Type type)
        {
            // Arrange
            var sut = new ArrayFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.Contains(result.First().Parameters, p => p.ParameterType.IsArray);
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

            var sut = new ArrayFavoringConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.SequenceEqual(result));
        }

        [Fact]
        public void DoesNotReturnConstructorsWithParametersOfEnclosingType()
        {
            // Arrange
            var sut = new ArrayFavoringConstructorQuery();
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

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, int[] nums)
            {
            }
        }
    }
}
