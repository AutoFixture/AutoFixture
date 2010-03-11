using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
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
            Assert.IsNull(result, "Create");
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
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonStringRequestSeedWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonStringRequestSeed = new Seed(typeof(object), "Anonymous value");
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringRequestSeed, dummyContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonStringSeedWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonStringSeed = new Seed(typeof(string), new object());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringSeed, dummyContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithStringSeedWhenContainerCannotCreateStringsWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var stringSeed = new Seed(typeof(string), "Anonymous value");
            var unableContainer = new DelegatingSpecimenContainer { OnCreate = r => null };
            // Exercise system
            var result = sut.Create(stringSeed, unableContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithStringSeedWhenContainerCanCreateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var seedString = Guid.NewGuid().ToString();
            var containerString = Guid.NewGuid().ToString();

            var sut = new StringSeedUnwrapper();
            var stringSeed = new Seed(typeof(string), seedString);
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
            var stringSeed = new Seed(typeof(string), "Anonymous value");

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
