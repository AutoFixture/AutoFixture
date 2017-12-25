using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Moq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class GreedyMockConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new GreedyMockConstructorQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(Mock))]
        [InlineData(typeof(Mock<>))]
        public void SelectMethodsReturnsCorrectResultForNonMockTypes(Type t)
        {
            // Arrange
            var sut = new GreedyMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type t)
        {
            // Arrange
            var mockType = t.GetTypeInfo().GetGenericArguments().Single();
            var expectedCount = mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;

            var sut = new GreedyMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(typeof(Mock<IInterface>))]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void MethodsDefineCorrectParameters(Type t)
        {
            // Arrange
            var mockType = t.GetTypeInfo().GetGenericArguments().Single();
            var mockTypeCtorArgs = from ci in mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                   select ci.GetParameters();

            var sut = new GreedyMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            var actualArgs = from ci in result
                             select ci.Parameters;
            Assert.True(mockTypeCtorArgs.All(expectedParams =>
                actualArgs.Any(expectedParams.SequenceEqual)));
        }

        [Theory]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void MethodsAreReturnedInCorrectOrder(Type t)
        {
            // Arrange
            var mockType = t.GetTypeInfo().GetGenericArguments().Single();
            var mockTypeCtorArgCounts = from ci in mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        let paramCount = ci.GetParameters().Length
                                        orderby paramCount descending
                                        select paramCount;

            var sut = new GreedyMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            var actualArgCounts = from ci in result
                                  select ci.Parameters.Count();
            Assert.True(mockTypeCtorArgCounts.SequenceEqual(actualArgCounts));
        }
    }
}
