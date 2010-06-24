using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class Int64SequenceGeneratorTest
    {
        public Int64SequenceGeneratorTest()
        {
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new Int64SequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Int64SequenceGenerator();
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
            var sut = new Int64SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonInt64RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonInt64Request = new object();
            var sut = new Int64SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonInt64Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonInt64Request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt64RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var int64Request = typeof(long);
            var sut = new Int64SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(int64Request, dummyContainer);
            // Verify outcome
            Assert.Equal(1L, result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt64RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var int64Request = typeof(long);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithInt64RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var int64Request = typeof(long);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
