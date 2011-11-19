using System;
using FakeItEasy;
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
            Func<Type, bool> expectedSpec = t => true;
            var sut = new FakeItEasyRelay(expectedSpec);
            // Exercise system
            Func<Type, bool> result = sut.FakeableSpecification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
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
            A.CallTo(() => contextStub.Resolve(fakeType)).Returns(fake);
            var sut = new FakeItEasyRelay();
            // Exercise system
            var result = sut.Create(request, contextStub);
            // Verify outcome
            Assert.Equal(fake.GetType().GetProperty("FakedObject").GetValue(fake, null), result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonMock()
        {
            // Fixture setup
            var request = typeof(IInterface);
            var fakeType = typeof(Fake<>).MakeGenericType(request);

            var contextStub = A.Fake<ISpecimenContext>();
            A.CallTo(() => contextStub.Resolve(fakeType)).Returns(new object());

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
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateCorrectlyInvokesSpecification(Type request)
        {
            // Fixture setup
            var verified = false;
            Func<Type, bool> fakeSpec = t => verified = t == request;
            var sut = new FakeItEasyRelay(fakeSpec);
            // Exercise system
            var contextDummy = A.Fake<ISpecimenContext>();
            sut.Create(request, contextDummy);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }
    }
}
