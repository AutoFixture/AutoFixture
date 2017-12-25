using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class Int64SequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int64SequenceGenerator, long>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new Int64SequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new Int64SequenceGenerator();
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
            var sut = new Int64SequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonInt64RequestWillReturnCorrectResult()
        {
            // Arrange
            var nonInt64Request = new object();
            var sut = new Int64SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonInt64Request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithInt64RequestWillReturnCorrectResult()
        {
            // Arrange
            var int64Request = typeof(long);
            var sut = new Int64SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(int64Request, dummyContainer);
            // Assert
            Assert.Equal(1L, result);
        }

        [Fact]
        public void CreateWithInt64RequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var int64Request = typeof(long);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithInt64RequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var int64Request = typeof(long);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int64SequenceGenerator, long>(sut => (long)sut.Create(int64Request, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
