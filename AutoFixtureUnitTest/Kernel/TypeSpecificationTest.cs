using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class TypeSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system
            var sut = new TypeSpecification(dummyType);
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new TypeSpecification(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            var sut = new TypeSpecification(dummyType);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(string), typeof(int), false)]
        [InlineData(typeof(PropertyHolder<string>), typeof(FieldHolder<string>), false)]
        public void IsSatisfiedByReturnsCorrectResult(Type specType, Type requestType, bool expectedResult)
        {
            // Fixture setup
            var sut = new TypeSpecification(specType);
            // Exercise system
            var result = sut.IsSatisfiedBy(requestType);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
