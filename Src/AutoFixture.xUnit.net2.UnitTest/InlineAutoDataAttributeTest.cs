﻿using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
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
        public void AttributesContainsAttributeWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Fixture setup
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(autoDataAttribute);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.Contains(autoDataAttribute, result);
            // Teardown
        }

        [Fact]
        public void AttributesContainsCorrectAttributeTypesWhenConstructorWithExplicitAutoDataAttribute()
        {
            // Fixture setup
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(autoDataAttribute);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            var expected = new[] { typeof(InlineDataAttribute), autoDataAttribute.GetType() };
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

        [Fact]
        public void ValuesAreCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Fixture setup
            var dummyAutoDataAttribute = new AutoDataAttribute();
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new InlineAutoDataAttribute(dummyAutoDataAttribute, expectedValues);
            // Exercise system
            var result = sut.Values;
            // Verify outcome
            Assert.True(expectedValues.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenCreatedWithModestConstructor()
        {
            // Fixture setup
            var sut = new InlineAutoDataAttribute();
            // Exercise system
            AutoDataAttribute result = sut.AutoDataAttribute;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenCreatedExplicitlyByConstructor()
        {
            // Fixture setup
            var expected = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(expected);
            // Exercise system
            var result = sut.AutoDataAttribute;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}