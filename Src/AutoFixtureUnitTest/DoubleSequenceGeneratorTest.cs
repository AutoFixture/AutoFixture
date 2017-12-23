using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DoubleSequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<DoubleSequenceGenerator, double>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new DoubleSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new DoubleSequenceGenerator();
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
            var sut = new DoubleSequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonDoubleRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonDoubleRequest = new object();
            var sut = new DoubleSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonDoubleRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithDoubleRequestWillReturnCorrectResult()
        {
            // Arrange
            var doubleRequest = typeof(double);
            var sut = new DoubleSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(doubleRequest, dummyContainer);
            // Assert
            Assert.Equal(1d, result);
        }

        [Fact]
        public void CreateWithDoubleRequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var doubleRequest = typeof(double);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<DoubleSequenceGenerator, double>(sut => (double)sut.Create(doubleRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithDoubleRequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var doubleRequest = typeof(double);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<DoubleSequenceGenerator, double>(sut => (double)sut.Create(doubleRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
