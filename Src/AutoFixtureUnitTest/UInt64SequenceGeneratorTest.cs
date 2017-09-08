using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UInt64SequenceGeneratorTest
    {
        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt64SequenceGenerator, ulong>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt64SequenceGenerator, ulong>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt64SequenceGenerator, ulong>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new UInt64SequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new UInt64SequenceGenerator();
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
            var sut = new UInt64SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonUInt64RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonUInt64Request = new object();
            var sut = new UInt64SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonUInt64Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt64RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var uInt64Request = typeof(ulong);
            var sut = new UInt64SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uInt64Request, dummyContainer);
            // Verify outcome
            Assert.Equal(1ul, result);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt64RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var uInt64Request = typeof(ulong);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt64SequenceGenerator, ulong>(sut => (ulong)sut.Create(uInt64Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithUInt64RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var uInt64Request = typeof(ulong);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt64SequenceGenerator, ulong>(sut => (ulong)sut.Create(uInt64Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
