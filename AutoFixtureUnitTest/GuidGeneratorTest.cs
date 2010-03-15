using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class GuidGeneratorTest
    {
        public GuidGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillReturnNonDefaultGuid()
        {
            // Fixture setup
            var unexpectedGuid = default(Guid);
            // Exercise system
            var result = GuidGenerator.CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<Guid>(unexpectedGuid, result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousTwiceWillReturnDifferentValues()
        {
            // Fixture setup
            var unexpectedGuid = GuidGenerator.CreateAnonymous();
            // Exercise system
            var result = GuidGenerator.CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<Guid>(unexpectedGuid, result, "CreateAnonymous");
            // Teardown
        }

        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new GuidGenerator();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new GuidGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.AreEqual(new NoSpecimen(), result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Fixture setup
            var sut = new GuidGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonGuidRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonGuidRequest = new object();
            var sut = new GuidGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonGuidRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonGuidRequest);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithGuidRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var guidRequest = typeof(Guid);
            var sut = new GuidGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(guidRequest, dummyContainer);
            // Verify outcome
            Assert.AreNotEqual(default(Guid), result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithGuidRequestTwiceWillReturnDifferentResults()
        {
            // Fixture setup
            var sut = new GuidGenerator();

            var guidRequest = typeof(Guid);
            var dummyContainer = new DelegatingSpecimenContainer();
            var unexpectedResult = sut.Create(guidRequest, dummyContainer);
            // Exercise system
            var result = sut.Create(guidRequest, dummyContainer);
            // Verify outcome
            Assert.AreNotEqual(unexpectedResult, result, "Create");
            // Teardown
        }
    }
}
