using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;
using Moq;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockConstructorQueryTest
    {
        [Fact]
        public void SutIsConstructorQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new MockConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IConstructorQuery>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(Mock))]
        [InlineData(typeof(Mock<>))]
        public void SelectReturnsCorrectResultForNonMockTypes(Type t)
        {
            // Fixture setup
            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }

        [Fact]
        public void SelectReturnsCorrectConstructorForTypeWithConstructor()
        {
            // Fixture setup
            var type = typeof(Mock<IInterface>);
            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(type);
            // Verify outcome
            Assert.True(result.Single().DeclaringType == type);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void SelectReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type t)
        {
            // Fixture setup
            var mockType = t.GetGenericArguments().Single();
            var expectedCount = mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;

            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<IInterface>))]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void SelectReturnsCorrectConstructorInfoForMockType(Type t)
        {
            // Fixture setup
            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            Assert.True(result.All(ci => ci.DeclaringType == t));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<IInterface>))]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void ConstructorsDefineCorrectParameters(Type t)
        {
            // Fixture setup
            var mockType = t.GetGenericArguments().Single();
            var mockTypeCtorArgs = from ci in mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                   select ci.GetParameters();

            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            var actualArgs = from ci in result
                             select ci.GetParameters();
            Assert.True(mockTypeCtorArgs.All(expectedParams =>
                actualArgs.Any(actualParams =>
                    expectedParams.SequenceEqual(actualParams))));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Mock<AbstractType>))]
        [InlineData(typeof(Mock<ConcreteType>))]
        [InlineData(typeof(Mock<MultiUnorderedConstructorType>))]
        public void ConstructorsAreReturnedInCorrectOrder(Type t)
        {
            // Fixture setup
            var mockType = t.GetGenericArguments().Single();
            var mockTypeCtorArgCounts = from ci in mockType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        let paramCount = ci.GetParameters().Length
                                        orderby paramCount ascending
                                        select paramCount;

            var sut = new MockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            var actualArgCounts = from ci in result
                                  select ci.GetParameters().Length;
            Assert.True(mockTypeCtorArgCounts.SequenceEqual(actualArgCounts));
            // Teardown
        }
    }
}
