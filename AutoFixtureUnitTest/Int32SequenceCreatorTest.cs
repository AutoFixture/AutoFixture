using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class Int32SequenceCreatorTest
    {
        public Int32SequenceCreatorTest()
        {
        }

        [TestMethod]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [TestMethod]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [TestMethod]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new Int32SequenceGenerator();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Int32SequenceGenerator();
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
            var sut = new Int32SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [TestMethod]
        public void CreateWithNonInt32RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonInt32Request = new object();
            var sut = new Int32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonInt32Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonInt32Request);
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithInt32RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var int32Request = typeof(int);
            var sut = new Int32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(int32Request, dummyContainer);
            // Verify outcome
            Assert.AreEqual(1, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWithInt32RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var int32Request = typeof(int);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<Int32SequenceGenerator, int>(sut => (int)sut.Create(int32Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [TestMethod]
        public void CreateWithInt32RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var int32Request = typeof(int);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<Int32SequenceGenerator, int>(sut => (int)sut.Create(int32Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
