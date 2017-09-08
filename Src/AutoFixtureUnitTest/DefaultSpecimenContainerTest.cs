using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class DefaultSpecimenContainerTest
    {
        [TestMethod]
        public void SutIsSpecimenContainer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DefaultSpecimenContainer(dummyBuilder);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenContainer));
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullBuilderWillThrow()
        {
            // Fixture setup
            // Exercise system
            new DefaultSpecimenContainer(null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new DefaultSpecimenContainer(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.AreEqual(expectedBuilder, result, "Builder");
            // Teardown
        }

        [TestMethod]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var sut = new DefaultSpecimenContainer(builder);
            // Exercise system
            var dummyRequest = new object();
            var result = sut.Create(dummyRequest);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.AreEqual(expectedRequest, r, "Create");
                    mockVerified = true;
                    return new object();
                };

            var sut = new DefaultSpecimenContainer(builderMock);
            // Exercise system
            sut.Create(expectedRequest);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }

        [TestMethod]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Fixture setup
            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();

            var sut = new DefaultSpecimenContainer(builderMock);

            builderMock.OnCreate = (r, c) =>
            {
                Assert.AreEqual(sut, c, "Create");
                mockVerified = true;
                return new object();
            };
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
