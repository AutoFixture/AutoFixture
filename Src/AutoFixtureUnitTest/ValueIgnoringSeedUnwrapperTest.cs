using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ValueIgnoringSeedUnwrapperTest
    {
        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ValueIgnoringSeedUnwrapper();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new ValueIgnoringSeedUnwrapper();
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
            var sut = new ValueIgnoringSeedUnwrapper();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateFromSeedWhenContainerCannotSatisfyWrappedRequestWillReturnNull()
        {
            // Fixture setup
            var anonymousSeed = new SeededRequest(typeof(object), new object());
            var unableContainer = new DelegatingSpecimenContainer { OnCreate = r => null };
            var sut = new ValueIgnoringSeedUnwrapper();
            // Exercise system
            var result = sut.Create(anonymousSeed, unableContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromSeedWhenContainerCanSatisfyWrappedRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var anonymousSeed = new SeededRequest(typeof(object), new object());

            var expectedResult = new object();
            var container = new DelegatingSpecimenContainer { OnCreate = r => expectedResult };

            var sut = new ValueIgnoringSeedUnwrapper();
            // Exercise system
            var result = sut.Create(anonymousSeed, container);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromSeedWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new ValueIgnoringSeedUnwrapper();
            var seededRequest = new SeededRequest(typeof(int), 1);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnCreate = r =>
            {
                Assert.AreEqual(typeof(int), r, "Create");
                mockVerified = true;
                return null;
            };
            // Exercise system
            sut.Create(seededRequest, containerMock);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
