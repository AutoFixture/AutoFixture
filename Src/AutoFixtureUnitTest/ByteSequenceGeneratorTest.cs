using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class ByteSequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<ByteSequenceGenerator, byte>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new ByteSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new ByteSequenceGenerator();
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
            var sut = new ByteSequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonByteRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonByteRequest = new object();
            var sut = new ByteSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonByteRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResult()
        {
            // Arrange
            var byteRequest = typeof(byte);
            var sut = new ByteSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(byteRequest, dummyContainer);
            // Assert
            Assert.Equal((byte)1, result);
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var byteRequest = typeof(byte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<ByteSequenceGenerator, byte>(sut => (byte)sut.Create(byteRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithByteRequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var byteRequest = typeof(byte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<ByteSequenceGenerator, byte>(sut => (byte)sut.Create(byteRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
