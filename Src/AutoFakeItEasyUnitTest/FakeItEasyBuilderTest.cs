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
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(AbstractType))]
        public void CreateWithFakeRequestReturnsCorrectResultWhenContextReturnsFake(Type type)
        {
            // Fixture setup
            var context = new Fake<ISpecimenContext>().FakedObject;
            var builderStub = new Fake<ISpecimenBuilder>();
            var fake = Activator.CreateInstance(typeof(Fake<>).MakeGenericType(type));
            builderStub.CallsTo(b => b.Create(type, context))
                .Returns(fake);
            var sut = new FakeItEasyBuilder(builderStub.FakedObject);
            // Exercise system
            var result = sut.Create(type, context);
            // Verify outcome
            Assert.IsAssignableFrom(type, result);
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
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
