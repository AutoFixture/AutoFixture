using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UInt16SequenceGeneratorTest
    {
        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new UInt16SequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new UInt16SequenceGenerator();
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
            var sut = new UInt16SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonUInt16RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonUInt16Request = new object();
            var sut = new UInt16SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonUInt16Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt16RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var uInt16Request = typeof(ushort);
            var sut = new UInt16SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uInt16Request, dummyContainer);
            // Verify outcome
            Assert.Equal((ushort)1, result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt16RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var uInt16Request = typeof(ushort);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt16SequenceGenerator, ushort>(sut => (ushort)sut.Create(uInt16Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt16RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var uInt16Request = typeof(ushort);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt16SequenceGenerator, ushort>(sut => (ushort)sut.Create(uInt16Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
