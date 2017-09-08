using System;
using System.Reflection;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class SubstituteAttributeRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilderToServeAsFixtureCustomization()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(ISpecimenBuilder).IsAssignableFrom(typeof(SubstituteAttributeRelay)));
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNull()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object specimen = sut.Create(null, context);
            // Verify outcome
#pragma warning disable 618
            var expected = new NoSpecimen(null);
#pragma warning restore 618
            Assert.Equal(expected, specimen);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenRequestIsNotICustomAttributeProvider()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var request = new object();
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object specimen = sut.Create(request, context);
            // Verify outcome
#pragma warning disable 618
            var expected = new NoSpecimen(request);
#pragma warning restore 618
            Assert.Equal(expected, specimen);
            // Teardown
        }

        [Fact]
        public void CreateReturnsNoSpecimenWhenICustomAttributeProviderDoesNotReturnExpectedAttributeType()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<ICustomAttributeProvider>();
            request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[0]);
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            object specimen = sut.Create(request, context);
            // Verify outcome
#pragma warning disable 618
            var expected = new NoSpecimen(request);
#pragma warning restore 618
            Assert.Equal(expected, specimen);
            // Teardown
        }

        [Fact]
        public void CreateResolvesSubstituteRequestForParameterWithSubstituteAttribute()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<ParameterInfo>();
            request.ParameterType.Returns(typeof(IInterface));
            request.GetCustomAttributes(typeof(SubstituteAttribute), true).Returns(new[] { new SubstituteAttribute() });
            var expectedSpecimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == request.ParameterType)).Returns(expectedSpecimen);
            // Exercise system
            object actualSpecimen = sut.Create(request, context);
            // Verify outcome
            Assert.Same(expectedSpecimen, actualSpecimen);
            // Teardown
        }

        [Fact]
        public void CreateResolvesSubstituteRequestForPropertyWithSubstituteAttribute()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<PropertyInfo>();
            request.PropertyType.Returns(typeof(IInterface));
            request.GetCustomAttributes(typeof(SubstituteAttribute), true).Returns(new[] { new SubstituteAttribute() });
            var expectedSpecimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == request.PropertyType)).Returns(expectedSpecimen);
            // Exercise system
            object actualSpecimen = sut.Create(request, context);
            // Verify outcome
            Assert.Same(expectedSpecimen, actualSpecimen);
            // Teardown
        }

        [Fact]
        public void CreateResolvesSubstituteRequestForFieldWithSubstituteAttribute()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<FieldInfo>();
            request.FieldType.Returns(typeof(IInterface));
            request.GetCustomAttributes(typeof(SubstituteAttribute), true).Returns(new[] { new SubstituteAttribute() });
            var expectedSpecimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(Arg.Is<SubstituteRequest>(r => r.TargetType == request.FieldType)).Returns(expectedSpecimen);
            // Exercise system
            object actualSpecimen = sut.Create(request, context);
            // Verify outcome
            Assert.Same(expectedSpecimen, actualSpecimen);
            // Teardown
        }

        [Fact]
        public void CreateRelayedRequestThrowsNotSupportedExceptionWhenAttributeIsAppliedToUnexpectedCodeElement()
        {
            // Fixture setup
            var sut = new SubstituteAttributeRelay();
            var request = Substitute.For<EventInfo>();
            var attribute = new SubstituteAttribute();
            request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new[] { attribute });
            var context = Substitute.For<ISpecimenContext>();
            // Exercise system
            var e = Assert.Throws<NotSupportedException>(() => sut.Create(request, context));
            // Verify outcome
            Assert.Contains(attribute.ToString(), e.Message);
            Assert.Contains(request.ToString(), e.Message);
            // Teardown
        }
    }
}
