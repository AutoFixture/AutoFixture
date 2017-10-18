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
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteRequestHandler)));
            // Teardown
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenSubstituteConstructorIsNull()
        {
            // Fixture setup
            // Exercise system
            var e = Assert.Throws<ArgumentNullException>(() => new SubstituteRequestHandler((ISpecimenBuilder)null));
            // Verify outcome
            Assert.Equal("substituteFactory", e.ParamName);
            // Teardown
        }

        [Fact]
        public void SubstituteConstructorReturnsValueSpecifiedInConstructorToEnableTestingOfCustomizations()
        {
            // Fixture setup
            var substituteConstructor = Substitute.For<ISpecimenBuilder>();
            // Exercise system
            var sut = new SubstituteRequestHandler(substituteConstructor);
            // Verify outcome
            Assert.Same(substituteConstructor, sut.SubstituteFactory);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotAnExplicitSubstituteRequest()
        {
            // Fixture setup
            var constructor = Substitute.For<ISpecimenBuilder>();
            var sut = new SubstituteRequestHandler(constructor);
            var request = typeof(IComparable);
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void CreatePassesRequestedSubstituteTypeAndSpecimenContextToSubstituteConstructorAndReturnsInstanceItCreates()
        {
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            Type targetType = typeof(IComparable);
            var constructedSubstute = Substitute.For(new[] { targetType}, new object[0]);
            var constructor = Substitute.For<ISpecimenBuilder>();
            constructor.Create(targetType, context).Returns(constructedSubstute);
            var sut = new SubstituteRequestHandler(constructor);
            var request = new SubstituteRequest(targetType);
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            Assert.Same(constructedSubstute, result);
            // Teardown
        }
    }
}
