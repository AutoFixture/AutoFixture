using System;
using System.Linq;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class DataAnnotationsSupportNodeTest
    {
        [Fact]
        public void ShouldBeASpecimenBuilder()
        {
            // Arrange
            var sut = new DataAnnotationsSupportNode(new DelegatingSpecimenBuilder());
            // Act & Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void ShouldBeASpecimenBuilderNode()
        {
            // Arrange
            var sut = new DataAnnotationsSupportNode(new DelegatingSpecimenBuilder());
            // Act & Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void ShouldThrowArgumentExceptionForNullBuilder()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DataAnnotationsSupportNode(null));
        }

        [Fact]
        public void ShouldStorePassedSpecimenBuilderNode()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new DataAnnotationsSupportNode(builder);
            // Assert
            Assert.Equal(builder, sut.Builder);
        }

        [Fact]
        public void ShouldDelegateCreateRequestToInnerBuilder()
        {
            // Arrange
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expectedResult = new object();

            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, ctx) => r == request ? expectedResult : new NoSpecimen()
            };

            var sut = new DataAnnotationsSupportNode(builder);
            // Act
            var actualResult = sut.Create(request, context);
            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ShouldComposeCorrectly()
        {
            // Arrange
            var sut = new DataAnnotationsSupportNode(new DelegatingSpecimenBuilder());
            var newBuilder = new DelegatingSpecimenBuilder();
            // Act
            var result = sut.Compose(new []{newBuilder});
            // Assert
            var castedResult = Assert.IsType<DataAnnotationsSupportNode>(result);
            Assert.Equal(newBuilder, castedResult.Builder);
        }

        [Fact]
        public void ShouldYieldOwnedBuilder()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            var sut = new DataAnnotationsSupportNode(builder);
            // Act
            var enumeratedBuilders = sut.ToArray();
            // Assert
            Assert.Single(enumeratedBuilders);
            Assert.Equal(builder, enumeratedBuilders[0]);
        }
    }
}