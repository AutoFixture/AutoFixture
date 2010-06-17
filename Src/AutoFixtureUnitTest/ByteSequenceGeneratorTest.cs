using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ByteSequenceGeneratorTest
    {
        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(10);
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
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonByteRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonByteRequest);
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
            var dummyContainer = new DelegatingSpecimenContainer();
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
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<ByteSequenceGenerator, decimal>(sut => (byte)sut.Create(byteRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var byteRequest = typeof(byte);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<ByteSequenceGenerator, decimal>(sut => (byte)sut.Create(byteRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
