using System;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Dsl
{
    public class NullComposerTest
    {
        [Fact]
        public void SutIsCustomizationComposer()
        {
            // Arrange
            // Act
            var sut = new NullComposer<object>();
            // Assert
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new NullComposer<object>((ISpecimenBuilder)null));
        }

        [Fact]
        public void InitializeWithNullFuncThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new NullComposer<object>((Func<ISpecimenBuilder>)null));
        }

        [Fact]
        public void InitializedWithDefaultConstructorCorrectlyComposes()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.Compose();
            // Assert
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.Empty(composite.Builders);
        }

        [Fact]
        public void InitializedWithBuilderCorrectlyComposes()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullComposer<object>(expectedBuilder);
            // Act
            var result = sut.Compose();
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void InitializedWithFuncCorrectlyComposes()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NullComposer<object>(() => expectedBuilder);
            // Act
            var result = sut.Compose();
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.FromSeed(s => s);
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void FromBuilderFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<Version>();
            // Act
            var dummyFactory = new DelegatingSpecimenBuilder();
            var result = sut.FromFactory(dummyFactory);
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void FromFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.FromFactory(() => new object());
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void FromSingleParameterFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.FromFactory((object x) => new object());
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void FromDoubleParameterFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.FromFactory((object x, object y) => new object());
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void FromTripeParameterFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.FromFactory((object x, object y, object z) => new object());
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void FromQuadrupleParameterFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.FromFactory((object x, object y, object z, object æ) => new object());
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.Do(x => { });
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.OmitAutoProperties();
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void AnonymousWithReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<PropertyHolder<object>>();
            // Act
            var result = sut.With(x => x.Property);
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void WithReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<PropertyHolder<object>>();
            // Act
            var result = sut.With(x => x.Property, new object());
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<object>();
            // Act
            var result = sut.WithAutoProperties();
            // Assert
            Assert.Same(sut, result);
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Arrange
            var sut = new NullComposer<PropertyHolder<object>>();
            // Act
            var result = sut.Without(x => x.Property);
            // Assert
            Assert.Same(sut, result);
        }
    }
}
