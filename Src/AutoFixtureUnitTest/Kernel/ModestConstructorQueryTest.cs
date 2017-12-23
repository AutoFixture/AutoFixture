using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ModestConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new ModestConstructorQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Arrange
            var sut = new ModestConstructorQuery();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Arrange
            var sut = new ModestConstructorQuery();
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
        public void SelectMethodsFromTypeReturnsCorrectResult(Type type)
        {
            // Arrange
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       orderby parameters.Length ascending
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new ModestConstructorQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.True(expectedConstructors.SequenceEqual(result));
        }

        [Fact]
        public void DoesNotReturnConstructorsWithParametersOfEnclosingType()
        {
            // Arrange
            var sut = new ModestConstructorQuery();
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

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, int num)
            {
            }

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, string str)
            {
            }
        }
    }
}
