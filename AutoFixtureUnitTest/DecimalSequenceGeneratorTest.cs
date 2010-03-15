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
    public class DecimalSequenceGeneratorTest
    {
        public DecimalSequenceGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [TestMethod]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [TestMethod]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DecimalSequenceGenerator();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new DecimalSequenceGenerator();
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
            var sut = new DecimalSequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonDecimalRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonDecimalRequest = new object();
            var sut = new DecimalSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonDecimalRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonDecimalRequest);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithDecimalRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var decimalRequest = typeof(decimal);
            var sut = new DecimalSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(decimalRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(1m, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateDecimalRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var decimalRequest = typeof(decimal);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [TestMethod]
        public void CreateWithDecimalRequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var decimalRequest = typeof(decimal);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
