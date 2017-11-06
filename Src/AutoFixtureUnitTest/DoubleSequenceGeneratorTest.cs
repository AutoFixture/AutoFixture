using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DoubleSequenceGeneratorTest
    {
        [Fact][Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact][Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.Create()).Execute(1);
        }

        [Fact][Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.Create()).Execute(2);
        }

        [Fact][Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DoubleSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new DoubleSequenceGenerator();
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
            var sut = new DoubleSequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonDoubleRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonDoubleRequest = new object();
            var sut = new DoubleSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDoubleRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDoubleRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var doubleRequest = typeof(double);
            var sut = new DoubleSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(doubleRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(1d, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDoubleRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var doubleRequest = typeof(double);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<DoubleSequenceGenerator, double>(sut => (double)sut.Create(doubleRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithDoubleRequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var doubleRequest = typeof(double);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<DoubleSequenceGenerator, double>(sut => (double)sut.Create(doubleRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
