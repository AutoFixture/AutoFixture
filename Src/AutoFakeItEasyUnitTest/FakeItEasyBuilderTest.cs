using System;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = A.Fake<ISpecimenBuilder>();
            // Exercise system
            var sut = new FakeItEasyBuilder(dummyBuilder);
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
                new FakeItEasyBuilder(null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = A.Fake<ISpecimenBuilder>();
            var sut = new FakeItEasyBuilder(expectedBuilder);
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
            var dummyBuilder = A.Fake<ISpecimenBuilder>();
            var sut = new FakeItEasyBuilder(dummyBuilder);
            // Exercise system
            var dummyContext = A.Fake<ISpecimenContext>();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithFakeRequestReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(Fake<object>);
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(new Fake<object>());

            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsAssignableFrom<Fake<object>>(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType), typeof(object))]
        [InlineData(typeof(AbstractType), null)]
        public void CreateWithFakeRequestReturnsCorrectResultWhenContextReturnsNonFake(Type request, object contextValue)
        {
            // Fixture setup
            var context = new Fake<ISpecimenContext>().FakedObject;
            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context))
                .Returns(contextValue);
            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(Fake<object>), "")]
        [InlineData(typeof(Fake<object>), null)]
        public void CreateFromFakeRequestWhenDecoratedBuilderReturnsNoFakeReturnsCorrectResult(object request, object innerResult)
        {
            // Fixture setup
            var context = new Fake<ISpecimenContext>().FakedObject;

            var builderStub = new Fake<ISpecimenBuilder>();
            builderStub.CallsTo(b => b.Create(request, context)).Returns(innerResult);

            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
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

            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
