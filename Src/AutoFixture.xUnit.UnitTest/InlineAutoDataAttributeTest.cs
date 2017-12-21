using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.Xunit.UnitTest
{
    public class InlineAutoDataAttributeTest
    {
        [Fact]
        public void SutIsCompositeDataAttribute()
        {
            // Arrange
            // Act
            var sut = new InlineAutoDataAttribute();
            // Assert
            Assert.IsAssignableFrom<CompositeDataAttribute>(sut);
        }

        [Fact]
        public void SutComposesDataAttributesInCorrectOrder()
        {
            // Arrange
            var sut = new InlineAutoDataAttribute();
            var expected = new[] { typeof(InlineDataAttribute), typeof(AutoDataAttribute) };
            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Assert
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
        }

        [Fact]
        public void AttributesContainsAttributeWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new DerivedInlineAutoDataAttribute(autoDataAttribute);
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.Contains(autoDataAttribute, result);
        }

        [Fact]
        public void AttributesContainsCorrectAttributeTypesWhenConstructorWithExplicitAutoDataAttribute()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(autoDataAttribute);
            // Act
            var result = sut.Attributes;
            // Assert
            var expected = new[] { typeof(InlineDataAttribute), autoDataAttribute.GetType() };
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
        }

        [Fact]
        public void ValuesWillBeEmptyWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new InlineAutoDataAttribute();
            var expected = Enumerable.Empty<object>();
            // Act
            var result = sut.Values;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValuesWillNotBeEmptyWhenSutIsCreatedWithConstructorArguments()
        {
            // Arrange
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new InlineAutoDataAttribute(expectedValues);
            // Act
            var result = sut.Values;
            // Assert
            Assert.True(result.SequenceEqual(expectedValues));
        }

        [Fact]
        public void ValuesAreCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var dummyAutoDataAttribute = new AutoDataAttribute();
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new DerivedInlineAutoDataAttribute(dummyAutoDataAttribute, expectedValues);
            // Act
            var result = sut.Values;
            // Assert
            Assert.True(expectedValues.SequenceEqual(result));
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenCreatedWithModestConstructor()
        {
            // Arrange
            var sut = new InlineAutoDataAttribute();
            // Act
            DataAttribute result = sut.AutoDataAttribute;
            // Assert
            Assert.NotNull(result);
            Assert.IsType<AutoDataAttribute>(result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenCreatedExplicitlyByConstructor()
        {
            // Arrange
            var expected = new AutoDataAttribute();
            var sut = new DerivedInlineAutoDataAttribute(expected);
            // Act
            var result = sut.AutoDataAttribute;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            bool wasInvoked = false;
            var autoData = new DerivedAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return null;
            });

            // Act
            var sut = new DerivedInlineAutoDataAttribute(autoData);

            // Assert
            Assert.False(wasInvoked);
        }

        private class DerivedInlineAutoDataAttribute : InlineAutoDataAttribute
        {
            public DerivedInlineAutoDataAttribute(
                DataAttribute autoDataAttribute,
                params object[] values)
                : base(autoDataAttribute, values)
            {
            }
        }

        private class DerivedAutoDataAttribute : AutoDataAttribute
        {
            public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
                : base(fixtureFactory)
            {
            }
        }
    }
}
