using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class InlineAutoDataAttributeTest
    {
        [Fact]
        public void SutIsCompositeDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new InlineAutoDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<CompositeDataAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void SutComposesDataAttributesInCorrectOrder()
        {
            // Fixture setup
            var sut = new InlineAutoDataAttribute();
            var expected = new[] { typeof(InlineDataAttribute), typeof(AutoDataAttribute) };
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
            // Teardown
        }

        [Fact]
        public void ValuesWillBeEmptyWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new InlineAutoDataAttribute();
            var expected = Enumerable.Empty<object>();
            // Exercise system
            var result = sut.Values;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ValuesWillNotBeEmptyWhenSutIsCreatedWithConstructorArguments()
        {
            // Fixture setup
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new InlineAutoDataAttribute(expectedValues);
            // Exercise system
            var result = sut.Values;
            // Verify outcome
            Assert.True(result.SequenceEqual(expectedValues));
            // Teardown
        }
    }
}
