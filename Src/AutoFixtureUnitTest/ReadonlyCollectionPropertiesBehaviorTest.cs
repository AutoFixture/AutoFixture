using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class ReadonlyCollectionPropertiesBehaviorTest
    {
        [Fact]
        public void SutIsBuilderTransformation()
        {
            // Arrange
            // Act
            var sut = new ReadonlyCollectionPropertiesBehavior();

            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesBehavior();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Transform(null));
        }

        [Fact]
        public void TransformReturnsPostprocessor()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesBehavior();
            var dummyBuilder = new DelegatingSpecimenBuilder();

            // Act
            var result = sut.Transform(dummyBuilder);

            // Assert
            Assert.IsAssignableFrom<Postprocessor>(result);
        }

        [Fact]
        public void TransformReturnsPostprocessorWhichDecoratesInput()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();

            // Act
            var result = sut.Transform(expectedBuilder);

            // Assert
            var p = Assert.IsAssignableFrom<Postprocessor>(result);
            Assert.IsAssignableFrom<ISpecimenBuilder>(p.Builder);
        }

        [Fact]
        public void TransformReturnsPostprocessorWhichContainsAppropriateCommand()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesBehavior();
            var dummyBuilder = new DelegatingSpecimenBuilder();

            // Act
            var result = sut.Transform(dummyBuilder);

            // Assert
            var p = Assert.IsAssignableFrom<Postprocessor>(result);
            Assert.IsAssignableFrom<ReadonlyCollectionPropertiesCommand>(p.Command);
        }

        [Fact]
        public void TransformReturnsPostprocessorWhichContainsAppropriateSpecification()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesBehavior();
            var dummyBuilder = new DelegatingSpecimenBuilder();

            // Act
            var result = sut.Transform(dummyBuilder);

            // Assert
            var p = Assert.IsAssignableFrom<Postprocessor>(result);
            Assert.IsAssignableFrom<AndRequestSpecification>(p.Specification);
        }
    }
}