using System;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = Substitute.For<ISpecimenBuilder>();
            // Exercise system
            var sut = new NSubstituteBuilder(dummyBuilder);
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
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = Substitute.For<ISpecimenBuilder>();
            var sut = new NSubstituteBuilder(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateWithNonFakeRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var dummyBuilder = Substitute.For<ISpecimenBuilder>();
            var sut = new NSubstituteBuilder(dummyBuilder);
            // Exercise system
            var dummyContext = Substitute.For<ISpecimenContext>();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithFakeRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(IMockable);
            var context = Substitute.For<ISpecimenContext>();

            var builderStub = Substitute.For<ISpecimenBuilder>();
            var fakeResult = new object();
            builderStub.Create(request, context).Returns(fakeResult);

            var sut = new NSubstituteBuilder(builderStub);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(fakeResult, result);
            //Assert.IsAssignableFrom<Fake<object>>(result);
            // Teardown
        }
/*
        [Theory]
        [InlineData(typeof(AbstractType), typeof(object))]
        [InlineData(typeof(AbstractType), null)]
        public void CreateWithFakeRequestReturnsCorrectResultWhenContextReturnsNonFake(Type request, object contextValue)
        {
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var builderStub = Substitute.For<ISpecimenBuilder>();
            builderStub.Create(request, context).Returns(contextValue);
            var sut = new NSubstituteBuilder(builderStub);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }
 */
/*
        [Theory]
        [InlineData(typeof(Fake<object>), "")]
        [InlineData(typeof(Fake<object>), null)]
        public void CreateFromFakeRequestWhenDecoratedBuilderReturnsNoFakeReturnsCorrectResult(object request, object innerResult)
        {
            // Fixture setup
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(innerResult);

            var sut = new NSubstituteBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromFakeRequestWhenDecoratedBuilderReturnsFakeOfWrongGenericTypeReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(Fake<IInterface>);
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(new Fake<AbstractType>());

            var sut = new NSubstituteBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }
 */
    }
}