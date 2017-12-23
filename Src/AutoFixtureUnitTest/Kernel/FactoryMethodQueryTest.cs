using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class FactoryMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new FactoryMethodQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Arrange
            var sut = new FactoryMethodQuery();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Arrange
            var sut = new FactoryMethodQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Act
            var result = sut.SelectMethods(typeWithNoPublicConstructors);
            // Assert
            Assert.False(result.Any());
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod))]
        [InlineData(typeof(TypeWithFactoryProperty))]
        public void SelectMethodsFromTypeReturnsCorrectResult(Type type)
        {
            // Arrange
            var expectedMethods =
                from mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where mi.ReturnType == type
                let parameters = mi.GetParameters()
                orderby parameters.Length ascending
                select new StaticMethod(mi) as IMethod;

            var sut = new FactoryMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedMethods.SequenceEqual(result));
        }

        [Fact]
        public void SelectMethodsFromTypeWithParameterOfSameTypeReturnsEmptyResult()
        {
            // Arrange
            var type = typeof(TypeWithPseudoFactoryMethod);
            var sut = new FactoryMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void SelectMethodsFromTypeWithConversionOperatorsOnlyReturnsEmptyResult()
        {
            // Arrange
            var type = typeof(TypeWithCastOperatorsWithoutPublicConstructor);
            var sut = new FactoryMethodQuery();

            // Act
            var result = sut.SelectMethods(type);

            // Assert
            Assert.Empty(result);

        }
    }
}
