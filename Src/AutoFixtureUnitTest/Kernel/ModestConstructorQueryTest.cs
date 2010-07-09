using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ModestConstructorQueryTest
    {
        [Fact]
        public void SutIsConstructorPicker()
        {
            // Fixture setup
            // Exercise system
            var sut = new ModestConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IConstructorQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectConstructorsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new ModestConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectConstructors(null));
            // Teardown
        }

        [Fact]
        public void SelectFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ModestConstructorQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Exercise system
            var result = sut.SelectConstructors(typeWithNoPublicConstructors);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectFromTypeReturnsCorrectResult(Type type)
        {
            // Fixture setup
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       orderby parameters.Length ascending
                                       select ci;

            var sut = new ModestConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(type);
            // Verify outcome
            Assert.True(expectedConstructors.SequenceEqual(result));
            // Teardown
        }
    }
}
