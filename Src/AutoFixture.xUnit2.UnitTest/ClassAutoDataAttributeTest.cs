using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class ClassAutoDataAttributeTest
    {
        [Fact]
        public void SutIsCompositeDataAttribute()
        {
            // Arrange
            // Act
            var sut = new ClassAutoDataAttribute(typeof(ClassData));
            // Assert
            Assert.IsAssignableFrom<CompositeDataAttribute>(sut);
        }

        [Theory]
        [ClassData(typeof(ClassData))]
        public void AttributesAreInCorrectOrder(ClassAutoDataAttribute sut)
        {
            // Arrange
            var expected = new[] { typeof(ClassDataAttribute), typeof(AutoDataAttribute) };
            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Assert
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
        }

        [Fact]
        public void AttributesContainsDefaultAttributeWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(ClassData));
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.Contains(result, a => a is AutoDataAttribute);
        }

        [Fact]
        public void AttributesContainsAttributeWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new DerivedClassAutoDataAttribute(autoDataAttribute, typeof(ClassData));
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.Contains(autoDataAttribute, result);
        }

        [Theory]
        [ClassData(typeof(ClassData))]
        public void AttributesContainsClassDataAttribute(ClassAutoDataAttribute sut)
        {
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.Contains(
                result,
                a => a is ClassDataAttribute classDataAttribute && classDataAttribute.Class == typeof(ClassData));
        }

        [Theory]
        [ClassData(typeof(ClassData))]
        public void ClassIsCorrect(ClassAutoDataAttribute sut)
        {
            // Arrange
            var expected = typeof(ClassData);
            // Act
            var result = sut.Class;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var sut = new ClassAutoDataAttribute(typeof(ClassData));
            // Act
            var result = sut.AutoDataAttribute;
            // Assert
            Assert.IsType<AutoDataAttribute>(result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var expected = new AutoDataAttribute();
            var sut = new DerivedClassAutoDataAttribute(expected, typeof(ClassData));
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
            var sut = new DerivedClassAutoDataAttribute(autoData, typeof(ClassData));

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void PreDiscoveryShouldBeDisabled()
        {
            // Arrange
            var expectedDiscovererType = typeof(NoPreDiscoveryDataDiscoverer).GetTypeInfo();
            var discovererAttr = typeof(ClassAutoDataAttribute).GetTypeInfo()
                .CustomAttributes
                .Single(x => x.AttributeType == typeof(DataDiscovererAttribute));

            var expectedType = expectedDiscovererType.FullName;
            var expectedAssembly = expectedDiscovererType.Assembly.GetName().Name;

            // Act
            var actualType = (string)discovererAttr.ConstructorArguments[0].Value;
            var actualAssembly = (string)discovererAttr.ConstructorArguments[1].Value;

            // Assert
            Assert.Equal(expectedType, actualType);
            Assert.Equal(expectedAssembly, actualAssembly);
        }

        private class DerivedClassAutoDataAttribute : ClassAutoDataAttribute
        {
            public DerivedClassAutoDataAttribute(
                DataAttribute autoDataAttribute,
                Type @class)
                : base(autoDataAttribute, @class)
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

        private class ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new[] { new ClassAutoDataAttribute(typeof(ClassData)) };
                yield return new[] { new DerivedClassAutoDataAttribute(new AutoDataAttribute(), typeof(ClassData)) };
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}