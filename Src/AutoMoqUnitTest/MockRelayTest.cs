using System;
using System.Collections;
using System.Collections.Generic;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MockRelay();
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
                new MockRelay(null));
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithSpecification()
        {
            // Fixture setup
            var expected = new Mock<IRequestSpecification>().Object;
            var sut = new MockRelay(expected);
            // Exercise system
            IRequestSpecification result = sut.MockableSpecification;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new MockRelay();
            // Exercise system
            var result = sut.MockableSpecification;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Fixture setup
            var sut = new MockRelay();
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
            var sut = new MockRelay();
            var dummyContext = new Mock<ISpecimenContext>().Object;
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
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var mock = (Mock)Activator.CreateInstance(mockType);
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(mock);

            var sut = new MockRelay();
            // Exercise system
            var result = sut.Create(request, contextStub.Object);
            // Verify outcome
            Assert.Equal(mock.Object, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenContextReturnsNonMock()
        {
            // Fixture setup
            var request = typeof(IInterface);
            var mockType = typeof(Mock<>).MakeGenericType(request);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(new object());

            var sut = new MockRelay();
            // Exercise system
            var result = sut.Create(request, contextStub.Object);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        public class ValidNonMockSpecimens : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {new NoSpecimen()};
                yield return new object[] {new OmitSpecimen()};
                yield return new object[] {null};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void CreateReturnsCorrectResultWhenContextReturnsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Fixture setup
            var request = typeof (IInterface);
            var mockType = typeof (Mock<>).MakeGenericType(request);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(mockType)).Returns(validNonMockSpecimen);

            var sut = new MockRelay();
            // Exercise system
            var result = sut.Create(request, contextStub.Object);
            // Verify outcome
            Assert.Equal(validNonMockSpecimen, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void CreateReturnsCorrectResultWhenSpecificationIsSatisfied(
            Type request)
        {
            // Fixture setup
            var specificationStub = new Mock<IRequestSpecification>();
            specificationStub
                .Setup(s => s.IsSatisfiedBy(request))
                .Returns(true);

            var expected = (Mock)typeof(Mock<>)
                .MakeGenericType(request)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);
            var contextStub = new Mock<ISpecimenContext>();
            contextStub
                .Setup(c => c.Resolve(typeof(Mock<>).MakeGenericType(request)))
                .Returns(expected);

            var sut = new MockRelay(specificationStub.Object);
            // Exercise system
            var actual = sut.Create(request, contextStub.Object);
            // Verify outcome
            Assert.Equal(expected.Object, actual);
            // Teardown
        }

        [Fact]
        public void CreateWhenSpecificationIsSatisfiedButRequestIsNotTypeReturnsCorrectResult()
        {
            // Fixture setup
            var specificationStub = new Mock<IRequestSpecification>();
            specificationStub
                .Setup(s => s.IsSatisfiedBy(It.IsAny<object>()))
                .Returns(true);

            var sut = new MockRelay(specificationStub.Object);

            var request = new object();
            // Exercise system
            var dummyContext = new Mock<ISpecimenContext>().Object;
            var actual = sut.Create(request, dummyContext);
            // Verify outcome
            var expected = new NoSpecimen(request);
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}
