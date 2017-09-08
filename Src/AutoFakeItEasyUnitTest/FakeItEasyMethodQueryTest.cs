using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new FakeItEasyMethodQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(Fake<>))]
        public void SelectReturnsCorrectResultForNonFakeTypes(Type t)
        {
            // Fixture setup
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Fake<AbstractType>))]
        [InlineData(typeof(Fake<ConcreteType>))]
        [InlineData(typeof(Fake<MultiUnorderedConstructorType>))]
        public void SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors(Type t)
        {
            // Fixture setup
            var fakeType = t.GetTypeInfo().GetGenericArguments().Single();
            var expectedCount = fakeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Fake<IInterface>))]
        [InlineData(typeof(Fake<AbstractType>))]
        [InlineData(typeof(Fake<ConcreteType>))]
        [InlineData(typeof(Fake<MultiUnorderedConstructorType>))]
        public void MethodsDefineCorrectParameters(Type t)
        {
            // Fixture setup
            var fakeType = t.GetTypeInfo().GetGenericArguments().Single();
            var fakeTypeCtorArgs = from ci in fakeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                   select ci.GetParameters();
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var actualArgs = from ci in result
                             select ci.Parameters;
            Assert.True(fakeTypeCtorArgs.All(expectedParams =>
                actualArgs.Any(expectedParams.SequenceEqual)));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Fake<AbstractType>))]
        [InlineData(typeof(Fake<ConcreteType>))]
        [InlineData(typeof(Fake<MultiUnorderedConstructorType>))]
        public void MethodsAreReturnedInCorrectOrder(Type t)
        {
            // Fixture setup
            var fakeType = t.GetTypeInfo().GetGenericArguments().Single();
            var fakeTypeCtorArgCounts = from ci in fakeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        let paramCount = ci.GetParameters().Length
                                        orderby paramCount ascending
                                        select paramCount;
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var actualArgCounts = from ci in result
                                  select ci.Parameters.Count();
            Assert.True(fakeTypeCtorArgCounts.SequenceEqual(actualArgCounts));
            // Teardown
        }
    }
}