using System;
using System.Reflection;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteRequestHandlerTest
    {
        [Fact]
        public void ClassImplementsISpecimenBuilderToServeAsFixtureCustomization()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteRequestHandler)));
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenSubstituteConstructorIsNull()
        {
            // Arrange
            // Act
            var e = Assert.Throws<ArgumentNullException>(() => new SubstituteRequestHandler((ISpecimenBuilder)null));
            // Assert
            Assert.Equal("substituteFactory", e.ParamName);
        }

        [Fact]
        public void SubstituteConstructorReturnsValueSpecifiedInConstructorToEnableTestingOfCustomizations()
        {
            // Arrange
            var substituteConstructor = Substitute.For<ISpecimenBuilder>();
            // Act
            var sut = new SubstituteRequestHandler(substituteConstructor);
            // Assert
            Assert.Same(substituteConstructor, sut.SubstituteFactory);
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotAnExplicitSubstituteRequest()
        {
            // Arrange
            var constructor = Substitute.For<ISpecimenBuilder>();
            var sut = new SubstituteRequestHandler(constructor);
            var request = typeof(IComparable);
            var context = Substitute.For<ISpecimenContext>();
            // Act
            object result = sut.Create(request, context);
            // Assert
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreatePassesRequestedSubstituteTypeAndSpecimenContextToSubstituteConstructorAndReturnsInstanceItCreates()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            Type targetType = typeof(IComparable);
            var constructedSubstute = Substitute.For(new[] { targetType}, new object[0]);
            var constructor = Substitute.For<ISpecimenBuilder>();
            constructor.Create(targetType, context).Returns(constructedSubstute);
            var sut = new SubstituteRequestHandler(constructor);
            var request = new SubstituteRequest(targetType);
            // Act
            object result = sut.Create(request, context);
            // Assert
            Assert.Same(constructedSubstute, result);
        }
    }
}
