using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class Int32SequenceCreatorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int32SequenceGenerator, int>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new Int32SequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new Int32SequenceGenerator();
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
            var sut = new Int32SequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonInt32RequestWillReturnCorrectResult()
        {
            // Arrange
            var nonInt32Request = new object();
            var sut = new Int32SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonInt32Request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithInt32RequestWillReturnCorrectResult()
        {
            // Arrange
            var int32Request = typeof(int);
            var sut = new Int32SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(int32Request, dummyContainer);
            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void CreateWithInt32RequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var int32Request = typeof(int);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int32SequenceGenerator, int>(sut => (int)sut.Create(int32Request, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithInt32RequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var int32Request = typeof(int);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int32SequenceGenerator, int>(sut => (int)sut.Create(int32Request, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
