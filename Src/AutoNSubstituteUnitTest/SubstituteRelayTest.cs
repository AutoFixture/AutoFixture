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
            // Assert
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteRelay)));
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
    }
}
