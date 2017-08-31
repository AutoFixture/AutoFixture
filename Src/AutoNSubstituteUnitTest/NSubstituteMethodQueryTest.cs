using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteMethodQueryTest
    {
        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void MethodsAreReturnedInCorrectOrder(Type type)
        {
            // Fixture setup
            var typeCtorArgCounts = from ci in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                    let paramCount = ci.GetParameters().Length
                                    orderby paramCount ascending
                                    select paramCount;
            var sut = new NSubstituteMethodQuery();

            // Exercise system
            var result = sut.SelectMethods(type);

            // Verify outcome
            var actualArgCounts = from ci in result select ci.Parameters.Count();
            Assert.True(typeCtorArgCounts.SequenceEqual(actualArgCounts));

            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void MethodsDefineCorrectParameters(Type type)
        {
            // Fixture setup
            var typeCtorArgs = from ci in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) select ci.GetParameters();
            var sut = new NSubstituteMethodQuery();

            // Exercise system
            var result = sut.SelectMethods(type);

            // Verify outcome
            var actualArgs = from ci in result select ci.Parameters;
            Assert.True(typeCtorArgs.All(expectedParams => actualArgs.Any(expectedParams.SequenceEqual)));

            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type type)
        {
            // Fixture setup
            var expectedCount = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new NSubstituteMethodQuery();

            // Exercise system
            var result = sut.SelectMethods(type);

            // Verify outcome
            Assert.Equal(expectedCount, result.Count());

            // Teardown
        }

        [Fact]
        public void SelectMethodReturnsMethodForInterface()
        {
            // Fixture setup
            var type = typeof(IInterface);
            var sut = new NSubstituteMethodQuery();

            // Exercise system
            var result = sut.SelectMethods(type);

            // Verify outcome
            Assert.NotEmpty(result);
        }

        [Fact]
        public void SelectMethodReturnsMethodWithoutParametersForInterface()
        {
            // Fixture setup
            var type = typeof(IInterface);
            var sut = new NSubstituteMethodQuery();

            // Exercise system
            var result = sut.SelectMethods(type);

            // Verify outcome
            Assert.Empty(result.First().Parameters);
        }

        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new NSubstituteMethodQuery();

            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);

            // Teardown
        }
    }
}
