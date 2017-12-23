using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class Int16SequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<Int16SequenceGenerator, short>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int16SequenceGenerator, short>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<Int16SequenceGenerator, short>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<Int16SequenceGenerator, short>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<Int16SequenceGenerator, short>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<Int16SequenceGenerator, short>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new Int16SequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new Int16SequenceGenerator();
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
            var sut = new Int16SequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonInt16RequestWillReturnCorrectResult()
        {
            // Arrange
            var nonInt16Request = new object();
            var sut = new Int16SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonInt16Request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithInt16RequestWillReturnCorrectResult()
        {
            // Arrange
            var int16Request = typeof(short);
            var sut = new Int16SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(int16Request, dummyContainer);
            // Assert
            Assert.Equal((short)1, result);
        }

        [Fact]
        public void CreateWithInt16RequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var int16Request = typeof(short);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int16SequenceGenerator, short>(sut => (short)sut.Create(int16Request, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithInt16RequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var int16Request = typeof(short);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<Int16SequenceGenerator, short>(sut => (short)sut.Create(int16Request, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
