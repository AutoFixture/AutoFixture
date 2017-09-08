using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ByteSequenceGeneratorTest
    {
        [Fact][Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new ByteSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new ByteSequenceGenerator();
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
            var sut = new ByteSequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonByteRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonByteRequest = new object();
            var sut = new ByteSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonByteRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var byteRequest = typeof(byte);
            var sut = new ByteSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(byteRequest, dummyContainer);
            // Verify outcome
            Assert.Equal((byte)1, result);
            // Teardown
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var byteRequest = typeof(byte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<ByteSequenceGenerator, byte>(sut => (byte)sut.Create(byteRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var byteRequest = typeof(byte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<ByteSequenceGenerator, byte>(sut => (byte)sut.Create(byteRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
