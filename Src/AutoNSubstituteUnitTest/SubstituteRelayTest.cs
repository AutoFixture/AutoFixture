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
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteRelay)));
            // Teardown
        }

        [Fact]
        public void CreateThrowsArgumentNullExceptionWhenContextIsNullBecauseItsRequired()
        {
            // Fixture setup
            var sut = new SubstituteRelay();
            var request = new object();
            // Exercise system
            var e = Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
            // Verify outcome
            Assert.Equal("context", e.ParamName);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotAType()
        {
            // Fixture setup
            var sut = new SubstituteRelay();
            object request = "beer";
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestedTypeIsNotAbstract()
        {
            // Fixture setup
            var sut = new SubstituteRelay();
            object request = typeof(string);
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            var expected = new NoSpecimen();
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateReturnsObjectResolvedFromContextWhenRequestedTypeIsAbstractOrInterface(Type requestedType)
        {
            // Fixture setup
            var sut = new SubstituteRelay();
            object request = requestedType;
            object substitute = Substitute.For(new Type[] { requestedType }, new object[0]);
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == requestedType)).Returns(substitute);
            // Exercise system
            object result = sut.Create(request, context);
            // Verify outcome
            Assert.Same(substitute, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsInvalidOperationExceptionWhenResolvedObjectIsNotSubstituteAssumingInvalidConfiguration()
        {
            // Fixture setup
            var sut = new SubstituteRelay();
            var request = typeof(IComparable);
            var notASubstitute = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Any<object>()).Returns(notASubstitute);
            // Exercise system
            var e = Assert.Throws<InvalidOperationException>(() => sut.Create(request, context));
            // Verify outcome
            Assert.Contains(request.FullName, e.Message);
            Assert.Contains(typeof(SubstituteRequestHandler).FullName, e.Message); 
            Assert.IsType<NotASubstituteException>(e.InnerException);
            // Teardown
        }
    }
}
