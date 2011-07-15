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
        public void SutIsFactoryMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new FactoryMethodQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IConstructorQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectConstructorsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new FactoryMethodQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectConstructors(null));
            // Teardown
        }

        [Fact]
        public void SelectFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FactoryMethodQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Exercise system
            var result = sut.SelectConstructors(typeWithNoPublicConstructors);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod))]
        [InlineData(typeof(TypeWithFactoryProperty))]
        public void SelectFromTypeReturnsCorrectResult(Type type)
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
            var result = sut.SelectConstructors(type);
            // Verify outcome
            Assert.True(expectedMethods.SequenceEqual(result));
            // Teardown
        }
    }
}
