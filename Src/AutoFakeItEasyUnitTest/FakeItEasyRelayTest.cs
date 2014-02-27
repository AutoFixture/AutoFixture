using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new FakeItEasyRelay();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyRelay(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Fixture setup
            var expected = A.Fake<IRequestSpecification>();
            var sut = new FakeItEasyRelay(expected);
            // Exercise system
            IRequestSpecification result = sut.FakeableSpecification;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new FakeItEasyRelay();
            // Exercise system
            var result = sut.FakeableSpecification;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new FakeItEasyRelay();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void CreateWithNonAbstractionRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new FakeItEasyRelay();
            var dummyContext = A.Fake<ISpecimenContext>();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWithAbstractionRequestReturnsCorrectResult(Type request)
        {
            // Fixture setup
            var fakeType = typeof(Fake<>).MakeGenericType(request);

            var fake = Activator.CreateInstance(fakeType);
            var contextStub = A.Fake<ISpecimenContext>();
            contextStub.CallsTo(ctx => ctx.Resolve(fakeType)).Returns(fake);

            var sut = new FakeItEasyRelay();
            // Exercise system
            var result = sut.Create(request, contextStub);
            // Verify outcome
            var expected = fake.GetType().GetProperty("FakedObject").GetValue(fake, null);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonFake()
        {
            // Fixture setup
            var request = typeof(IInterface);
            var fakeType = typeof(Fake<>).MakeGenericType(request);

            var contextStub = A.Fake<ISpecimenContext>();
            contextStub.CallsTo(ctx => ctx.Resolve(fakeType)).Returns(new object());

            var sut = new FakeItEasyRelay();
            // Exercise system
            var result = sut.Create(request, contextStub);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateWhenSpecificationIsSatisfiedReturnsCorrectResult(
            Type request)
        {
            // Fixture setup
            var specificationStub = A.Fake<IRequestSpecification>();
            A.CallTo(() => specificationStub.IsSatisfiedBy(request))
                .Returns(true);

            var expected = typeof(Fake<>)
                .MakeGenericType(request)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);
            var contextStub = A.Fake<ISpecimenContext>();
            A.CallTo(() => contextStub.Resolve(typeof(Fake<>).MakeGenericType(request)))
                .Returns(expected);

            var sut = new FakeItEasyRelay(specificationStub);
            // Exercise system
            var actual = sut.Create(request, contextStub);
            // Verify outcome
            Assert.Equal(
                expected.GetType().GetProperty("FakedObject").GetValue(expected, null),
                actual);
            // Teardown
        }

        [Fact]
        public void CreateWhenSpecificationIsSatisfiedButRequestIsNotTypeReturnsCorrectResult()
        {
            // Fixture setup
            var specificationStub = A.Fake<IRequestSpecification>();
            A.CallTo(() => specificationStub.IsSatisfiedBy(A<object>.Ignored))
                .Returns(true);

            var sut = new FakeItEasyRelay(specificationStub);

            var request = new object();
            // Exercise system
            var dummyContext = A.Fake<ISpecimenContext>();
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}
