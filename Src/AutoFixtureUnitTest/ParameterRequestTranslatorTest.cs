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
    public class ParameterRequestTranslatorTest
    {
        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ParameterRequestTranslator();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new ParameterRequestTranslator();
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
            var sut = new ParameterRequestTranslator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateFromNonParameterRequestWillReturnNull()
        {
            // Fixture setup
            var nonParameterRequest = new object();
            var sut = new ParameterRequestTranslator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonParameterRequest, dummyContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromParameterRequestWillReturnNullWhenContainerCannotSatisfyRequest()
        {
            // Fixture setup
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var container = new DelegatingSpecimenContainer { OnCreate = r => null };
            var sut = new ParameterRequestTranslator();
            // Exercise system
            var result = sut.Create(parameterInfo, container);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromParameterRequestWillReturnCorrectResultWhenContainerCanSatisfyRequest()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var container = new DelegatingSpecimenContainer { OnCreate = r => expectedSpecimen };
            var sut = new ParameterRequestTranslator();
            // Exercise system
            var result = sut.Create(parameterInfo, container);
            // Verify outcome
            Assert.AreEqual(expectedSpecimen, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateFromParameterRequestWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new ParameterRequestTranslator();
            var parameterInfo = typeof(SingleParameterType<string>).GetConstructors().First().GetParameters().First();
            var expectedRequest = new SeededRequest(parameterInfo.ParameterType, parameterInfo.Name);

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnCreate = r =>
            {
                Assert.AreEqual(expectedRequest, r, "Create");
                mockVerified = true;
                return null;
            };
            // Exercise system
            sut.Create(parameterInfo, containerMock);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
