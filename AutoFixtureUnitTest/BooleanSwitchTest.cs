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
    public class BooleanSwitchTest
    {
        public BooleanSwitchTest()
        {
        }

        [TestMethod]
        public void CreateAnonymousWillReturnTrueOnFirstCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillReturnFalseOnSecondCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsFalse(result, "CreateAnonymous called an even number of times");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillReturnTrueOnThirdCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(result, "CreateAnonymous called an uneven number of times");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillReturnFalseOnFourthCall()
        {
            // Fixture setup
            BooleanSwitch sut = new BooleanSwitch();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            sut.CreateAnonymous();
            // Exercise system
            bool result = sut.CreateAnonymous();
            // Verify outcome
            Assert.IsFalse(result, "CreateAnonymous called an even number of times");
            // Teardown
        }

        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new BooleanSwitch();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new BooleanSwitch();
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
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonBooleanRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonBooleanRequest = new object();
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonBooleanRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonBooleanRequest);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnFirstCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(true, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(false, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnThirdCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(true, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithBooleanRequestWillReturnCorrectResultOnFourthCall()
        {
            // Fixture setup
            var booleanRequest = typeof(bool);
            var sut = new BooleanSwitch();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            sut.Create(booleanRequest, dummyContainer);
            var result = sut.Create(booleanRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(false, result, "Create");
            // Teardown
        }
    }
}
