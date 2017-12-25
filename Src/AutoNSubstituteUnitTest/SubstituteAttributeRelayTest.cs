using System;
using System.Reflection;
using AutoFixture.Kernel;
using NSubstitute;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilderToServeAsFixtureCustomization()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteAttributeRelay)));
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNull()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var context = Substitute.For<ISpecimenContext>();
            // Act
            object specimen = sut.Create(null, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, specimen);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotICustomAttributeProvider()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var request = new object();
            var context = Substitute.For<ISpecimenContext>();
            // Act
            object specimen = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, specimen);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenICustomAttributeProviderDoesNotReturnExpectedAttributeType()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<ICustomAttributeProvider>();
            request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[0]);
            var context = Substitute.For<ISpecimenContext>();
            // Act
            object specimen = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, specimen);
        }

        [Fact]
        public void CreateResolvesSubstituteRequestForParameterWithSubstituteAttribute()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<ParameterInfo>();
            request.ParameterType.Returns(typeof(IInterface));
            request.GetCustomAttributes(typeof(SubstituteAttribute), true).Returns(new[] { new SubstituteAttribute() });
            var expectedSpecimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == request.ParameterType)).Returns(expectedSpecimen);
            // Act
            object actualSpecimen = sut.Create(request, context);
            // Assert
            Assert.Same(expectedSpecimen, actualSpecimen);
        }

        [Fact]
        public void CreateResolvesSubstituteRequestForPropertyWithSubstituteAttribute()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<PropertyInfo>();
            request.PropertyType.Returns(typeof(IInterface));
            request.GetCustomAttributes(typeof(SubstituteAttribute), true).Returns(new[] { new SubstituteAttribute() });
            var expectedSpecimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == request.PropertyType)).Returns(expectedSpecimen);
            // Act
            object actualSpecimen = sut.Create(request, context);
            // Assert
            Assert.Same(expectedSpecimen, actualSpecimen);
        }

        [Fact]
        public void CreateResolvesSubstituteRequestForFieldWithSubstituteAttribute()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<FieldInfo>();
            request.FieldType.Returns(typeof(IInterface));
            request.GetCustomAttributes(typeof(SubstituteAttribute), true).Returns(new[] { new SubstituteAttribute() });
            var expectedSpecimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == request.FieldType)).Returns(expectedSpecimen);
            // Act
            object actualSpecimen = sut.Create(request, context);
            // Assert
            Assert.Same(expectedSpecimen, actualSpecimen);
        }

        [Fact]
        public void CreateRelayedRequestThrowsNotSupportedExceptionWhenAttributeIsAppliedToUnexpectedCodeElement()
        {
            // Arrange
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<ICustomAttributeProvider>();
            var attribute = new SubstituteAttribute();
            request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[] { attribute });
            var context = Substitute.For<ISpecimenContext>();
            // Act
            var e = Assert.Throws<NotSupportedException>(() => sut.Create(request, context));
            // Assert
            Assert.Contains(attribute.ToString(), e.Message);
            Assert.Contains(request.ToString(), e.Message);
        }
    }
}
