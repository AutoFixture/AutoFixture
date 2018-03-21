using System;
using System.Reflection;
using AutoFixture.Kernel;
using NSubstitute;
using NSubstitute.Exceptions;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteRelayTest
    {
        [Fact]
        public void ClassImplementsISpecimenBuilderToServeAsResidueCollector()
        {
            // Arrange
            // Act
            var sut = new SubstituteRelay();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void DefaultSpecificationShouldBeValid()
        {
            // Arrange
            // Act
            var sut = new SubstituteRelay();
            // Assert
            Assert.IsType<AbstractTypeSpecification>(sut.Specification);
        }

        [Fact]
        public void CustomSpecificationIsPreserved()
        {
            // Arrange
            var specification = new TrueRequestSpecification();
            // Act
            var sut = new SubstituteRelay(specification);
            // Assert
            Assert.Same(specification, sut.Specification);
        }

        [Fact]
        public void CreateThrowsArgumentNullExceptionWhenContextIsNullBecauseItsRequired()
        {
            // Arrange
            var sut = new SubstituteRelay();
            var request = new object();
            // Act
            var e = Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
            // Assert
            Assert.Equal("context", e.ParamName);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotAType()
        {
            // Arrange
            var sut = new SubstituteRelay();
            object request = "beer";
            var context = Substitute.For<ISpecimenContext>();
            // Act
            object result = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestedTypeIsNotAbstract()
        {
            // Arrange
            var sut = new SubstituteRelay();
            object request = typeof(string);
            var context = Substitute.For<ISpecimenContext>();
            // Act
            object result = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateReturnsObjectResolvedFromContextWhenRequestedTypeIsAbstractOrInterface(Type requestedType)
        {
            // Arrange
            var sut = new SubstituteRelay();
            object request = requestedType;
            object substitute = Substitute.For(new Type[] { requestedType }, new object[0]);
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == requestedType)).Returns(substitute);
            // Act
            object result = sut.Create(request, context);
            // Assert
            Assert.Same(substitute, result);
        }

        [Fact]
        public void CreateThrowsInvalidOperationExceptionWhenResolvedObjectIsNotSubstituteAssumingInvalidConfiguration()
        {
            // Arrange
            var sut = new SubstituteRelay();
            var request = typeof(IComparable);
            var notASubstitute = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Any<object>()).Returns(notASubstitute);
            // Act
            var e = Assert.Throws<InvalidOperationException>(() => sut.Create(request, context));
            // Assert
            Assert.Contains(request.FullName, e.Message);
            Assert.Contains(typeof(SubstituteRequestHandler).FullName, e.Message); 
            Assert.IsType<NotASubstituteException>(e.InnerException);
        }

        [Fact]
        public void ShouldNotRelayRequestIfSpecificationDoesNotMatch()
        {
            // Arrange
            var falseSpecification = new FalseRequestSpecification();
            var sut = new SubstituteRelay(falseSpecification);
            var request = typeof(IInterface);
            var context = Substitute.For<ISpecimenContext>();

            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
        }
    }
}
