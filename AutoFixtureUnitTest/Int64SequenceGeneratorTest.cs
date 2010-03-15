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
    public class Int64SequenceGeneratorTest
    {
        public Int64SequenceGeneratorTest()
        {
        }

        [TestMethod]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [TestMethod]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [TestMethod]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new Int64SequenceGenerator();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Int64SequenceGenerator();
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
            var sut = new Int64SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonInt64RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonInt64Request = new object();
            var sut = new Int64SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonInt64Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonInt64Request);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithInt64RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var int64Request = typeof(long);
            var sut = new Int64SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(int64Request, dummyContainer);
            // Verify outcome
            Assert.AreEqual(1L, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithInt64RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var int64Request = typeof(long);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [TestMethod]
        public void CreateWithInt64RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var int64Request = typeof(long);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
