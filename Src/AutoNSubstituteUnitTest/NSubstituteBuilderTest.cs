using System;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Exercise system
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>());

            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);

            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NSubstituteBuilder(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = Substitute.For<ISpecimenBuilder>();
            var sut = new NSubstituteBuilder(expectedBuilder);

            // Exercise system
            var result = sut.Builder;

            // Verify outcome
            Assert.Equal(expectedBuilder, result);

            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrect()
        {
            // Fixture setup
            IRequestSpecification specification = new TrueRequestSpecification();
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), specification);

            // Exercise system
            var result = sut.SubstitutionSpecification;

            // Verify outcome
            Assert.Equal(specification, result);

            // Teardown
        }

        [Fact]
        public void SpecificationIsAbstractTypeSpecificationWhenInitializedWithConstructorWithoutSpecificationParameter()
        {
            // Fixture setup
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>());

            // Exercise system
            var result = sut.SubstitutionSpecification;

            // Verify outcome
            Assert.IsType<AbstractTypeSpecification>(result);

            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequest_ReturnsNoSpecimen_WhenDecoratedBuilderReturnsNull(Type request)
        {
            // Fixture setup
            var builder = Substitute.For<ISpecimenBuilder>();
            var context = Substitute.For<ISpecimenContext>();
            builder.Create(request, context).Returns(null);
            var sut = new NSubstituteBuilder(builder);

            // Exercise system
            var result = sut.Create(request, context);

            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequest_ReturnsResultFromDecoratedBuilder(Type request)
        {
            // Fixture setup
            var builder = Substitute.For<ISpecimenBuilder>();
            var context = Substitute.For<ISpecimenContext>();
            var expected = new object();
            builder.Create(request, context).Returns(expected);
            var sut = new NSubstituteBuilder(builder);

            // Exercise system
            var result = sut.Create(request, context);

            // Verify outcome
            Assert.Same(expected, result);
        }

        [Fact]
        public void Create_WithRequestThatDoesNotMatchSpecification_ReturnsNoSpecimen()
        {
            // Fixture setup
            var specification = new FalseRequestSpecification();
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), specification);
            var context = Substitute.For<ISpecimenContext>();
            var request = typeof(ConcreteType);

            // Exercise system
            var result = sut.Create(request, context);

            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData("")]
        public void Create_WithRequestThatIsNotAType_ReturnsNoSpecimen(object request)
        {
            // Fixture setup
            var specification = new TrueRequestSpecification();
            var sut = new NSubstituteBuilder(Substitute.For<ISpecimenBuilder>(), specification);
            var context = Substitute.For<ISpecimenContext>();

            // Exercise system
            var result = sut.Create(request, context);

            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, result);
        }
    }
}
