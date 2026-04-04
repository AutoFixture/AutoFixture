using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new FakeItEasyMethodQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(Fake<>))]
        public void SelectReturnsCorrectResultForNonFakeTypes(Type t)
        {
            // Arrange
            var sut = new FakeItEasyMethodQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(typeof(Fake<AbstractType>))]
        [InlineData(typeof(Fake<ConcreteType>))]
        [InlineData(typeof(Fake<MultiUnorderedConstructorType>))]
        public void SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type t)
        {
            // Arrange
            var fakeType = t.GetTypeInfo().GetGenericArguments().Single();
            var expectedCount = fakeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new FakeItEasyMethodQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(typeof(Fake<IInterface>))]
        [InlineData(typeof(Fake<AbstractType>))]
        [InlineData(typeof(Fake<ConcreteType>))]
        [InlineData(typeof(Fake<MultiUnorderedConstructorType>))]
        public void MethodsDefineCorrectParameters(Type t)
        {
            // Arrange
            var fakeType = t.GetTypeInfo().GetGenericArguments().Single();
            var fakeTypeCtorArgs = from ci in fakeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                   select ci.GetParameters();
            var sut = new FakeItEasyMethodQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            var actualArgs = from ci in result
                             select ci.Parameters;
            Assert.True(fakeTypeCtorArgs.All(expectedParams =>
                actualArgs.Any(expectedParams.SequenceEqual)));
        }

        [Theory]
        [InlineData(typeof(Fake<AbstractType>))]
        [InlineData(typeof(Fake<ConcreteType>))]
        [InlineData(typeof(Fake<MultiUnorderedConstructorType>))]
        public void MethodsAreReturnedInCorrectOrder(Type t)
        {
            // Arrange
            var fakeType = t.GetTypeInfo().GetGenericArguments().Single();
            var fakeTypeCtorArgCounts = from ci in fakeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        let paramCount = ci.GetParameters().Length
                                        orderby paramCount ascending
                                        select paramCount;
            var sut = new FakeItEasyMethodQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            var actualArgCounts = from ci in result
                                  select ci.Parameters.Count();
            Assert.True(fakeTypeCtorArgCounts.SequenceEqual(actualArgCounts));
        }
    }
}