using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FactoryMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new FactoryMethodQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new FactoryMethodQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FactoryMethodQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Exercise system
            var result = sut.SelectMethods(typeWithNoPublicConstructors);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod))]
        [InlineData(typeof(TypeWithFactoryProperty))]
        public void SelectMethodsFromTypeReturnsCorrectResult(Type type)
        {
            // Fixture setup
            var expectedMethods =
                from mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where mi.ReturnType == type
                let parameters = mi.GetParameters()
                orderby parameters.Length ascending
                select new StaticMethod(mi) as IMethod;

            var sut = new FactoryMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(expectedMethods.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromTypeWithParameterOfSameTypeReturnsEmptyResult()
        {
            // Fixture setup
            var type = typeof(TypeWithPseudoFactoryMethod);
            var sut = new FactoryMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }
    }
}
