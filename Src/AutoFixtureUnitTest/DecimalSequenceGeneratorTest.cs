using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DecimalSequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DecimalSequenceGenerator, decimal>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new DecimalSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new DecimalSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Arrange
            var sut = new DecimalSequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonDecimalRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonDecimalRequest = new object();
            var sut = new DecimalSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDecimalRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithDecimalRequestWillReturnCorrectResult()
        {
            // Arrange
            var decimalRequest = typeof(decimal);
            var sut = new DecimalSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(decimalRequest, dummyContainer);
            // Assert
            Assert.Equal(1m, result);
        }

        [Fact]
        public void CreateDecimalRequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var decimalRequest = typeof(decimal);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithDecimalRequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var decimalRequest = typeof(decimal);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<DecimalSequenceGenerator, decimal>(sut => (decimal)sut.Create(decimalRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
