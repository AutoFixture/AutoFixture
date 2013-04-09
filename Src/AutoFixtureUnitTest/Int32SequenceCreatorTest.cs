﻿using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class Int32SequenceCreatorTest
    {
        [Fact][Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact][Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new Int32SequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Int32SequenceGenerator();
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
            var sut = new Int32SequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonInt32RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonInt32Request = new object();
            var sut = new Int32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonInt32Request, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonInt32Request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt32RequestWillReturnCorrectResult()
        {
            // Fixture setup
            var int32Request = typeof(int);
            var sut = new Int32SequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(int32Request, dummyContainer);
            // Verify outcome
            Assert.Equal(1, result);
            // Teardown
        }

        [Fact]
        public void CreateWithInt32RequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var int32Request = typeof(int);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int32SequenceGenerator, int>(sut => (int)sut.Create(int32Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithInt32RequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var int32Request = typeof(int);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int32SequenceGenerator, int>(sut => (int)sut.Create(int32Request, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
