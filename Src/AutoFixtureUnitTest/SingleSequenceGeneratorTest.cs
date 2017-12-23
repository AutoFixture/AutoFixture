using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SingleSequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<SingleSequenceGenerator, float>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new SingleSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new SingleSequenceGenerator();
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
            var sut = new SingleSequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonSingleRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonSingleRequest = new object();
            var sut = new SingleSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonSingleRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithSingleRequestWillReturnCorrectResult()
        {
            // Arrange
            var singleRequest = typeof(float);
            var sut = new SingleSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(singleRequest, dummyContainer);
            // Assert
            Assert.Equal(1f, result);
        }

        [Fact]
        public void CreateWithSingleRequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var singleRequest = typeof(float);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<SingleSequenceGenerator, float>(sut => (float)sut.Create(singleRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithSingleRequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var singleRequest = typeof(float);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<SingleSequenceGenerator, float>(sut => (float)sut.Create(singleRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
