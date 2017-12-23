using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UInt64SequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt64SequenceGenerator, ulong>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt64SequenceGenerator, ulong>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt64SequenceGenerator, ulong>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new UInt64SequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new UInt64SequenceGenerator();
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
            var sut = new UInt64SequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonUInt64RequestWillReturnCorrectResult()
        {
            // Arrange
            var nonUInt64Request = new object();
            var sut = new UInt64SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonUInt64Request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithUInt64RequestWillReturnCorrectResult()
        {
            // Arrange
            var uInt64Request = typeof(ulong);
            var sut = new UInt64SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uInt64Request, dummyContainer);
            // Assert
            Assert.Equal(1ul, result);
        }

        [Fact]
        public void CreateWithUInt64RequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var uInt64Request = typeof(ulong);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt64SequenceGenerator, ulong>(sut => (ulong)sut.Create(uInt64Request, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithUInt64RequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var uInt64Request = typeof(ulong);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt64SequenceGenerator, ulong>(sut => (ulong)sut.Create(uInt64Request, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
