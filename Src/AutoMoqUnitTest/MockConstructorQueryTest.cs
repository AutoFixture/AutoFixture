using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Moq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class MockConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new MockConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
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
            // Fixture setup
            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type t)
        {
            // Fixture setup
            var mockType = t.GetTypeInfo().GetGenericArguments().Single();
            var expectedCount = mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;

            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<IInterface>))]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void MethodsDefineCorrectParameters(Type t)
        {
            // Fixture setup
            var mockType = t.GetTypeInfo().GetGenericArguments().Single();
            var mockTypeCtorArgs = from ci in mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                   select ci.GetParameters();

            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var actualArgs = from ci in result
                             select ci.Parameters;
            Assert.True(mockTypeCtorArgs.All(expectedParams =>
                actualArgs.Any(actualParams =>
                    expectedParams.SequenceEqual(actualParams))));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void MethodsAreReturnedInCorrectOrder(Type t)
        {
            // Fixture setup
            var mockType = t.GetTypeInfo().GetGenericArguments().Single();
            var mockTypeCtorArgCounts = from ci in mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        let paramCount = ci.GetParameters().Length
                                        orderby paramCount ascending
                                        select paramCount;

            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var actualArgCounts = from ci in result
                                  select ci.Parameters.Count();
            Assert.True(mockTypeCtorArgCounts.SequenceEqual(actualArgCounts));
            // Teardown
        }

        [Fact]
        public void FiltersOutPrivateConstructor()
        {
            // Fixture setup
            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(typeof(Mock<ConcreteTypeWithPrivateParameterlessConstructor>));
            // Verify outcome
            Assert.Single(result);
            // Teardown
        }
    }
}
