using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteMethodQueryTest
    {
        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void MethodsAreReturnedInCorrectOrder(Type type)
        {
            // Arrange
            var typeCtorArgCounts = from ci in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                    let paramCount = ci.GetParameters().Length
                                    orderby paramCount ascending
                                    select paramCount;
            var sut = new NSubstituteMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            var actualArgCounts = from ci in result select ci.Parameters.Count();
            Assert.True(typeCtorArgCounts.SequenceEqual(actualArgCounts));
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void MethodsDefineCorrectParameters(Type type)
        {
            // Arrange
            var typeCtorArgs = from ci in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) select ci.GetParameters();
            var sut = new NSubstituteMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            var actualArgs = from ci in result select ci.Parameters;
            Assert.True(typeCtorArgs.All(expectedParams => actualArgs.Any(expectedParams.SequenceEqual)));
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type type)
        {
            // Arrange
            var expectedCount = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new NSubstituteMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void SelectMethodReturnsMethodForInterface()
        {
            // Arrange
            var type = typeof(IInterface);
            var sut = new NSubstituteMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void SelectMethodReturnsMethodWithoutParametersForInterface()
        {
            // Arrange
            var type = typeof(IInterface);
            var sut = new NSubstituteMethodQuery();
            // Act
            var result = sut.SelectMethods(type);
            // Assert
            Assert.Empty(result.First().Parameters);
        }

        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new NSubstituteMethodQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }
    }
}
