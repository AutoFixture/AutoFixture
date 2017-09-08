using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class StringGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringGenerator(() => new object());
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullFactoryWillThrow()
        {
            // Fixture setup
            Func<object> nullFactory = null;
            // Exercise system
            // Verify outcome (expected exception)
            Assert.Throws<ArgumentNullException>(() =>
                new StringGenerator(nullFactory));
            // Teardown
        }

        [Fact]
        public void FactoryIsCorrect()
        {
            // Fixture setup
            Func<object> expectedFactory = () => new object();
            var sut = new StringGenerator(expectedFactory);
            // Exercise system
            Func<object> result = sut.Factory;
            // Verify outcome
            Assert.Equal(expectedFactory, result);
            // Teardown
        }

        [Fact]
        public void CreateFromNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringGenerator(() => new object());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerWillNotThrow()
        {
            // Fixture setup
            var sut = new StringGenerator(() => string.Empty);
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateFromNonStringRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringGenerator(() => string.Empty);
            var nonStringRequest = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonStringRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromStringRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var specimen = 1;
            var expectedResult = specimen.ToString();

            var sut = new StringGenerator(() => specimen);
            var stringRequest = typeof(string);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(stringRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromStringRequestWhenFactoryReturnsNullWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringGenerator(() => null);
            var stringRequest = typeof(string);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(stringRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateFromStringRequestWhenFactoryReturnsNoSpecimenWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedResult = new NoSpecimen();
            var sut = new StringGenerator(() => expectedResult);
            var stringRequest = typeof(string);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(stringRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
