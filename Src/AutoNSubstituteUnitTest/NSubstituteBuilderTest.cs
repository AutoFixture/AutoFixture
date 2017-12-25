using System;
using AutoFixture.Kernel;
using NSubstitute;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Act
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>());
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new NSubstituteBuilder(null));
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = Substitute.For<ISpecimenBuilder>();
            var sut = new NSubstituteBuilder(expectedBuilder);
            // Act
            var result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void SpecificationIsCorrect()
        {
            // Arrange
            IRequestSpecification specification = new TrueRequestSpecification();
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), specification);
            // Act
            var result = sut.SubstitutionSpecification;
            // Assert
            Assert.Equal(specification, result);
        }

        [Fact]
        public void SpecificationIsAbstractTypeSpecificationWhenInitializedWithConstructorWithoutSpecificationParameter()
        {
            // Arrange
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>());
            // Act
            var result = sut.SubstitutionSpecification;
            // Assert
            Assert.IsType<AbstractTypeSpecification>(result);
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequest_ReturnsNoSpecimen_WhenDecoratedBuilderReturnsNull(Type request)
        {
            // Arrange
            var builder = Substitute.For<ISpecimenBuilder>();
            var context = Substitute.For<ISpecimenContext>();
            builder.Create(request, context).Returns(null);
            var sut = new NSubstituteBuilder(builder);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequest_ReturnsResultFromDecoratedBuilder(Type request)
        {
            // Arrange
            var builder = Substitute.For<ISpecimenBuilder>();
            var context = Substitute.For<ISpecimenContext>();
            var expected = new object();
            builder.Create(request, context).Returns(expected);
            var sut = new NSubstituteBuilder(builder);
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.Same(expected, result);
        }

        [Fact]
        public void Create_WithRequestThatDoesNotMatchSpecification_ReturnsNoSpecimen()
        {
            // Arrange
            var specification = new FalseRequestSpecification();
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), specification);
            var context = Substitute.For<ISpecimenContext>();
            var request = typeof(ConcreteType);
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("")]
        public void Create_WithRequestThatIsNotAType_ReturnsNoSpecimen(object request)
        {
            // Arrange
            var specification = new TrueRequestSpecification();
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), specification);
            var context = Substitute.For<ISpecimenContext>();
            // Act
            var result = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }
    }
}
