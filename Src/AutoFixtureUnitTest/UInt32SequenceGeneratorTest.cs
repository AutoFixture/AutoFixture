using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UInt32SequenceGeneratorTest
    {
        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new UInt32SequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new UInt32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Fixture setup
            var sut = new UInt32SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonUInt32RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonUInt32Request = new object();
            var sut = new UInt32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonUInt32Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt32RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var uInt32Request = typeof(uint);
            var sut = new UInt32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uInt32Request, dummyContainer);
            // Verify outcome
            Assert.Equal(1u, result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt32RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var uInt32Request = typeof(uint);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt32SequenceGenerator, uint>(sut => (uint)sut.Create(uInt32Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt32RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var uInt32Request = typeof(uint);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt32SequenceGenerator, uint>(sut => (uint)sut.Create(uInt32Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
