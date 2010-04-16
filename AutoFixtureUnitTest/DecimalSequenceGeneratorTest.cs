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
    public class DecimalSequenceGeneratorTest
    {
        public DecimalSequenceGeneratorTest()
        {
        }

        [Fact]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new DecimalSequenceGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new DecimalSequenceGenerator();
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
            var sut = new DecimalSequenceGenerator();
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWithNonDecimalRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonDecimalRequest = new object();
            var sut = new DecimalSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonDecimalRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonDecimalRequest);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithDecimalRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var decimalRequest = typeof(decimal);
            var sut = new DecimalSequenceGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(decimalRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(1m, result);
            // Teardown
        }

        [Fact]
        public void CreateDecimalRequestWillReturnCorrectResultOnSecondCall()
        {
            // Fixture setup
            var decimalRequest = typeof(decimal);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(2);
            // Teardown
        }

        [Fact]
        public void CreateWithDecimalRequestWillReturnCorrectResultOnTenthCall()
        {
            // Fixture setup
            var decimalRequest = typeof(decimal);
            var dummyContainer = new DelegatingSpecimenContainer();
            var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
            // Exercise system and verify outcome
            loopTest.Execute(10);
            // Teardown
        }
    }
}
