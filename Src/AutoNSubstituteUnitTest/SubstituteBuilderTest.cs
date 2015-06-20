using System;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteBuilderTest
    {
        [Fact]
        public void ClassImplementsISpecimenBuilderToServeAsFixtureCustomization()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteBuilder)));
            // Teardown
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenSubstituteConstructorIsNull()
        {
            // Fixture setup
            // Exercise system
            var e = Assert.Throws<ArgumentNullException>(() => new SubstituteBuilder((ISpecimenBuilder)null));
            // Verify outcome
            Assert.Equal("substituteConstructor", e.ParamName);
            // Teardown
        }

        [Fact]
        public void SubstituteConstructorReturnsValueSpecifiedInConstructorToEnableTestingOfCustomizations()
        {
            // Fixture setup
            var substituteConstructor = Substitute.For<ISpecimenBuilder>();
            // Exercise system
            var sut = new SubstituteBuilder(substituteConstructor);
            // Verify outcome
            Assert.Same(substituteConstructor, sut.SubstituteConstructor);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotAnExplicitSubstituteRequest()
        {
            // Fixture setup
            var constructor = Substitute.For<ISpecimenBuilder>();
            var sut = new SubstituteBuilder(constructor);
            var request = typeof(IComparable);
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            var noSpecimen = Assert.IsType<NoSpecimen>(result);
            Assert.Same(request, noSpecimen.Request);
            // Teardown
        }

        [Fact]
        public void CreatePassesRequestedSubstituteTypeAndSpecimenContextToSubstituteConstructorAndReturnsInstanceItCreates()
        {
            // Fixture setup
            Type targetType = typeof(IComparable);
            var constructedSubstute = Substitute.For(new[] { targetType}, new object[0]);
            var constructor = Substitute.For<ISpecimenBuilder>();
            constructor.Create(Arg.Any<object>(), Arg.Any<ISpecimenContext>()).Returns(constructedSubstute);
            var sut = new SubstituteBuilder(constructor);
            var request = new SubstituteRequest(targetType);
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            constructor.Received().Create(targetType, context);
            Assert.Same(constructedSubstute, result);
            // Teardown
        }
    }
}
