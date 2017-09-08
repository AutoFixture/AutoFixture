using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SByteSequenceGeneratorTest
    {
        [Fact][Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new SByteSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new SByteSequenceGenerator();
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
            var sut = new SByteSequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonSByteRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonSByteRequest = new object();
            var sut = new SByteSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonSByteRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithSByteRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sbyteRequest = typeof(sbyte);
            var sut = new SByteSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(sbyteRequest, dummyContainer);
            // Verify outcome
            Assert.Equal((sbyte)1, result);
            // Teardown
        }

        [Fact]
        public void CreateWithSByteRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var sbyteRequest = typeof(sbyte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<SByteSequenceGenerator, sbyte>(sut => (sbyte)sut.Create(sbyteRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithSByteRequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var sbyteRequest = typeof(sbyte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<SByteSequenceGenerator, sbyte>(sut => (sbyte)sut.Create(sbyteRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
