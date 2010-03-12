using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class StringGeneratorTest
    {
        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringGenerator(() => new object());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullFactoryWillThrow()
        {
            // Fixture setup
            Func<object> nullFactory = null;
            // Exercise system
            new StringGenerator(nullFactory);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateFromNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringGenerator(() => new object());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
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

        [TestMethod]
        public void CreateFromNonStringRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringGenerator(() => string.Empty);
            var nonStringRequest = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonStringRequest);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromStringRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var specimen = 1;
            var expectedResult = specimen.ToString();

            var sut = new StringGenerator(() => specimen);
            var stringRequest = typeof(string);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(stringRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromStringRequestWhenFactoryReturnsNullWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringGenerator(() => null);
            var stringRequest = typeof(string);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(stringRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(stringRequest);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromStringRequestWhenFactoryReturnsNoSpecimenWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedResult = new NoSpecimen();
            var sut = new StringGenerator(() => expectedResult);
            var stringRequest = typeof(string);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(stringRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }
    }
}
