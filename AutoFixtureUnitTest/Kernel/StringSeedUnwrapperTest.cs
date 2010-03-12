using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    [TestClass]
    public class StringSeedUnwrapperTest
    {
        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringSeedUnwrapper();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonSeedWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonSeed = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonSeed, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonSeed);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonStringRequestSeedWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonStringRequestSeed = new SeededRequest(typeof(object), "Anonymous value");
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringRequestSeed, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonStringRequestSeed);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonStringSeedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonStringSeed = new SeededRequest(typeof(string), new object());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringSeed, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonStringSeed);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithStringSeedWhenContainerCannotCreateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var stringSeed = new SeededRequest(typeof(string), "Anonymous value");
            var unableContainer = new DelegatingSpecimenContainer { OnCreate = r => new NoSpecimen(stringSeed) };
            // Exercise system
            var result = sut.Create(stringSeed, unableContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(stringSeed);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithStringSeedWhenContainerCanCreateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var seedString = Guid.NewGuid().ToString();
            var containerString = Guid.NewGuid().ToString();

            var sut = new StringSeedUnwrapper();
            var stringSeed = new SeededRequest(typeof(string), seedString);
            var container = new DelegatingSpecimenContainer { OnCreate = r => containerString };
            // Exercise system
            var result = sut.Create(stringSeed, container);
            // Verify outcome
            var expectedString = seedString + containerString;
            Assert.AreEqual(expectedString, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithStringSeedWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var stringSeed = new SeededRequest(typeof(string), "Anonymous value");

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnCreate = r =>
                {
                    Assert.AreEqual(typeof(string), r, "Create");
                    mockVerified = true;
                    return null;
                };
            // Exercise system
            sut.Create(stringSeed, containerMock);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
